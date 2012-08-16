using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using GenericCampaignMasterLib;
using System.Web.Script.Serialization;
using CampaignMasterWeb.GcmlWsReference;

namespace CampaignMasterWeb
{
    public partial class SektorControl : System.Web.UI.UserControl
    {
        private JavaScriptSerializer m_serializer = new JavaScriptSerializer();

        public SektorInfo Sektor { get; set; }
        private string campaignId;
        private string playerId;

        protected void Page_Init(object sender, EventArgs e)
        {
            campaignId = (string)Session[GcmlClientKeys.CAMPAIGNID];
            playerId = (string)Session[GcmlClientKeys.CONTEXTPLAYERID];

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

            LabelSektorname.Text = this.Sektor.sektorId;
        }

        public void drawContext()
        {
            CampaignMasterService service = GcmlClientWeb.getService(Session);
            System.Drawing.Color bgcolor = System.Drawing.Color.LightCyan;
            
            if (service.getUnitCollisions(campaignId).Contains(this.Sektor))
                bgcolor = System.Drawing.Color.Orange;
            
            this.TableUnits.BackColor = bgcolor;

            // Sektorinformationen aktualisieren
            if (this.Sektor != null)
            {
                string sektorid = this.Sektor.sektorId;
                SektorInfo s = service.getSektor(campaignId, Sektor.sektorKoordinaten);
                this.Sektor = s;
            }

            TableUnits.Rows.Clear();
            TableUnitActions.Rows.Clear();
            string selectedUnitId = (string)Session[GcmlClientKeys.CONTEXTUNITID];
            foreach (string unitId in this.Sektor.containedUnitIds)
            {
                BaseUnit unit = service.getUnit(campaignId, unitId);
                TableRow row = createSelectableRow("buttonSelectUnit_" + unit.Id, unit.Id.ToString() + " : " + unit.Bezeichnung, new EventHandler(unitSelected));
                TableUnits.Rows.Add(row);

                // Für Selektierte Unit Kommandos ausgeben
                if (unit.Id.ToString() == selectedUnitId)
                {
                    row.BackColor = System.Drawing.Color.LightBlue;

                    Dictionary<string, CommandInfo> contextCmdList = (Dictionary<string, CommandInfo>)Session[GcmlClientKeys.CONTEXTCOMMANDLIST];
                    foreach (string cmdkey in contextCmdList.Keys)
                    {
                        CommandInfo cmd = contextCmdList[cmdkey];

                        TableRow rowcmd = createSelectableRow(cmdkey, cmd.commandType, new EventHandler(executeUnitAction));
                        TableUnitActions.Rows.Add(rowcmd);
                    }
                }
            }
        }

        protected void unitSelected(object sender, EventArgs e)
        {
            CampaignMasterService service = GcmlClientWeb.getService(Session);
            Button btnSender = sender as Button;
            string selUnitId = btnSender.ID.Substring(17);
            BaseUnit unit = service.getUnit(campaignId, selUnitId);
            setSelectedUnitContext(unit);
        }

        protected void executeUnitAction(object sender, EventArgs e)
        {
            CampaignMasterService service = GcmlClientWeb.getService(Session);
            Button btnSender = sender as Button;
            string cmdId = btnSender.ID;
            Dictionary<string, CommandInfo> cmdList = (Dictionary<string, CommandInfo>)Session[GcmlClientKeys.CONTEXTCOMMANDLIST];
            if((cmdList != null) && cmdList.Keys.Contains(cmdId))
            {
                CommandInfo cmd = cmdList[cmdId];
                service.executeCommand(campaignId, cmd);
            }

            setSelectedUnitContext(null);       // Wenn die AKtion ausgeführt wurde keine Unit mehr selektiert
        }

        private void setSelectedUnitContext(BaseUnit selectedUnit)
        {
            CampaignMasterService service = GcmlClientWeb.getService(Session);
            string contextUnitId = null;      // Id der aktuell selektierten Einheit
            Dictionary<string, CommandInfo> contextCommandList = new Dictionary<string, CommandInfo>();

            if (selectedUnit != null)
            {
                // Die aktuell auswählbaren Commands werden mit einer ID im State gespeichert um Sie mit einem Listitem auswählen zu können
                List<CommandInfo> listCommands = service.getCommandsForUnit(campaignId, selectedUnit.Id).ToList<CommandInfo>();
                foreach (CommandInfo cmd in listCommands)
                {
                    string cmdId = cmd.commandId;
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