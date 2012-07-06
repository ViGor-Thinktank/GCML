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
                TableRow row = new TableRow();
                TableCell cell1 = new TableCell();
                TableCell cell2 = new TableCell();
                row.Cells.Add(cell1);
                row.Cells.Add(cell2);

                Button btnSelectUnit = new Button();
                btnSelectUnit.ID = "buttonSelectUnit#" + unit.Id;

                cell1.Text = unit.Id.ToString() + " : " + unit.Bezeichnung;
                cell2.Controls.Add(btnSelectUnit);

                TableUnits.Rows.Add(row);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            LabelSektorname.Text = "djdjdjdj";
        }



        //protected void unitSelected(object sender, EventArgs e)
        //{
        //    CampaignController controller = GcmlClient.getCampaignController(this.Session);

        //    //Button btn = sender as Button;
        //    //ControlCollection ctrls = btn.Parent.Controls;
        //    //ListBox listUnits = ctrls.OfType<ListBox>().First(l => l.ID.Contains("listUnits"));

        //    ListBox listBoxUnits = sender as ListBox;
        //    //ListBox listUnitActions = ctrls.OfType<ListBox>().First(l => l.ID.Contains("listUnitActions"));

        //    if (listBoxUnits == null)
        //        return;


        //    string unitId = listBoxUnits.SelectedValue;
        //    IUnit unit = controller.getUnit(unitId);

        //    if (unit != null)
        //        Session[GcmlClientKeys.CONTEXTUNITID] = unit.Id.ToString();

        //    //drawPlayerView();
        //}
    }
}