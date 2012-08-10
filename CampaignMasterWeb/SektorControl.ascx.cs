using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GenericCampaignMasterLib;
using GcmlWebService;
using System.Web.Script.Serialization;

namespace CampaignMasterWeb
{
    public partial class SektorControl : System.Web.UI.UserControl
    {
        private JavaScriptSerializer m_serializer = new JavaScriptSerializer();

        public Sektor Sektor { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            drawForm();
            drawContext();
        }

        private void drawForm()
        {
            if (this.Sektor == null)
            {
                LabelSektorname.Text = "Kein Sektor zugewiesen";
                return;
            }

            LabelSektorname.Text = this.Sektor.strUniqueID;
        }

        public void drawContext()
        {
            string campaignId = (string)Session[GcmlClientKeys.CAMPAIGNID];
            string playerId = (string)Session[GcmlClientKeys.CONTEXTPLAYERID];

            CampaignMasterService service = GcmlClientWeb.getService(Session);
            CampaignController controller = GcmlClientWeb.getCampaignController(Session);
            System.Drawing.Color bgcolor = System.Drawing.Color.LightCyan;
            
            if (service.getUnitCollisions(campaignId).Contains(this.Sektor.strUniqueID))
                bgcolor = System.Drawing.Color.Orange;
            
            this.TableUnits.BackColor = bgcolor;

            // Sektorinformationen aktualisieren
            if (this.Sektor != null)
            {
                string sektorid = this.Sektor.Id;
                string sektorStr = service.getSektor(campaignId, Sektor.objSektorKoord.ToJson());
                this.Sektor = m_serializer.Deserialize<Sektor>(sektorStr);
            }

            TableUnits.Rows.Clear();
            TableUnitActions.Rows.Clear();
            string selectedUnitId = (string)Session[GcmlClientKeys.CONTEXTUNITID];
            foreach (BaseUnit unit in this.Sektor.ListUnits)
            {
                TableRow row = createSelectableRow("buttonSelectUnit_" + unit.Id, unit.Id.ToString() + " : " + unit.Bezeichnung, new EventHandler(unitSelected));
                TableUnits.Rows.Add(row);

                // Für Selektierte Unit Kommandos ausgeben
                if (unit.Id.ToString() == selectedUnitId)
                {
                    row.BackColor = System.Drawing.Color.LightBlue;

                    Dictionary<string, ICommand> contextCmdList = (Dictionary<string, ICommand>)Session[GcmlClientKeys.CONTEXTCOMMANDLIST];
                    foreach (string cmdkey in contextCmdList.Keys)
                    {
                        ICommand cmd = contextCmdList[cmdkey];

                        TableRow rowcmd = createSelectableRow(cmdkey, cmd.strInfo, new EventHandler(executeUnitAction));
                        TableUnitActions.Rows.Add(rowcmd);
                    }
                }
            }
        }

        protected void unitSelected(object sender, EventArgs e)
        {
            Button btnSender = sender as Button;
            string selUnitId = btnSender.ID.Substring(17);
            CampaignController controller = GcmlClientWeb.getCampaignController(this.Session);
            BaseUnit unit = controller.getUnit(selUnitId);

            setSelectedUnitContext(unit);
        }

        protected void executeUnitAction(object sender, EventArgs e)
        {
            Button btnSender = sender as Button;
            string cmdId = btnSender.ID;
            Dictionary <string, ICommand> cmdList = (Dictionary<string, ICommand>) Session[GcmlClientKeys.CONTEXTCOMMANDLIST];
            if((cmdList != null) && cmdList.Keys.Contains(cmdId))
            {
                ICommand cmd = cmdList[cmdId];
                cmd.Execute();
            }

            setSelectedUnitContext(null);       // Wenn die AKtion ausgeführt wurde keine Unit mehr selektiert
        }

        private void setSelectedUnitContext(BaseUnit selectedUnit)
        {
            string contextUnitId = null;      // Id der aktuell selektierten Einheit
            Dictionary<string, ICommand> contextCommandList = new Dictionary<string, ICommand>();
            CampaignController controller = GcmlClientWeb.getCampaignController(this.Session);

            if (selectedUnit != null)
            {
                // Die aktuell auswählbaren Commands werden mit einer ID im State gespeichert um Sie mit einem Listitem auswählen zu können
                List<ICommand> listCommands = controller.getCommandsForUnit(selectedUnit);
                foreach (ICommand cmd in listCommands)
                {
                    string cmdId = Guid.NewGuid().ToString();
                    contextCommandList.Add(cmdId, cmd);
                }

                contextUnitId = selectedUnit.Id.ToString();
            }

            Session[GcmlClientKeys.CONTEXTUNITID] = contextUnitId;
            Session[GcmlClientKeys.CONTEXTCOMMANDLIST] = contextCommandList;
        }

        private TableRow createSelectableRow(string id, string bezeichnung, EventHandler handler)
        {
            TableRow row = new TableRow();
            TableCell cell1 = new TableCell();
            TableCell cell2 = new TableCell();
            row.Cells.Add(cell1);
            row.Cells.Add(cell2);

            Button btnSelectObject = new Button();
            btnSelectObject.ID = id;
            btnSelectObject.Click += handler;
            btnSelectObject.Height = Unit.Pixel(20);
            btnSelectObject.Width = Unit.Pixel(20);

            cell1.Text = bezeichnung;
            cell2.Controls.Add(btnSelectObject);

            TableUnits.Rows.Add(row);

            return row;
        }


    }
}