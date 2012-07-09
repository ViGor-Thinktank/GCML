using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GenericCampaignMasterLib;

namespace CampaignMasterWeb
{
    public partial class SektorControl : System.Web.UI.UserControl
    {
        public Sektor Sektor { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {

          drawSektor();
        }

        private void drawSektor()
        {
            if (this.Sektor == null)
            {
                LabelSektorname.Text = "Kein Sektor zugewiesen";
                return;
            }
            else
            {
                LabelSektorname.Text = "bla";
            }

            CampaignController controller = GcmlClient.getCampaignController(Session);
            System.Drawing.Color bgcolor = System.Drawing.Color.LightCyan;
            if (controller.getUnitCollisions().Contains(this.Sektor))
                bgcolor = System.Drawing.Color.Orange;
            this.TableUnits.BackColor = bgcolor;

            LabelSektorname.Text = this.Sektor.Id;
              
            foreach (IUnit unit in this.Sektor.ListUnits)
            {
                TableRow row = createSelectableRow("buttonSelectUnit#" + unit.Id, unit.Id.ToString() + " : " + unit.Bezeichnung);
                TableUnits.Rows.Add(row);
            }
        }

        private TableRow createSelectableRow(string id, string bezeichnung)
        {
            TableRow row = new TableRow();
            TableCell cell1 = new TableCell();
            TableCell cell2 = new TableCell();
            row.Cells.Add(cell1);
            row.Cells.Add(cell2);

            Button btnSelectObject = new Button();
            btnSelectObject.ID = id;

            cell1.Text = bezeichnung;
            cell2.Controls.Add(btnSelectObject);

            TableUnits.Rows.Add(row);

            return row;
        }

        protected void unitSelected(object sender, EventArgs e)
        {
            Button btnSender = sender as Button;
            string selUnitId = btnSender.ID.Substring(16);
            CampaignController controller = GcmlClient.getCampaignController(this.Session);
            IUnit unit = controller.getUnit(selUnitId);

            if ((unit != null) &&
                (this.Sektor.ListUnits.Contains(unit)))
            {
                // Die aktuell auswählbaren Commands werden mit einer ID im State gespeichert um Sie mit einem Listitem auswählen zu können
                Dictionary<string, ICommand> contextCmdList = new Dictionary<string, ICommand>();
                Session[GcmlClientKeys.CONTEXTCOMMANDLIST] = contextCmdList;
                Session[GcmlClientKeys.CONTEXTUNITID] = unit.Id.ToString();
                
                List<ICommand> lstCmds = controller.getCommandsForUnit(unit);
                foreach (ICommand cmd in lstCmds)
                {
                    string cmdId = new Guid().ToString();       // Temporäre ID für die Zuordnung des ListItems
                    ListItem li = new ListItem();
                    li.Text = cmd.strInfo;
                    li.Value = cmdId;


                    contextCmdList.Add(cmdId, cmd);
                }
            }

            //drawSektor(); 
        }
    }
}