using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GenericCampaignMasterLib;

namespace CampaignMasterWeb
{
    public partial class FieldControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Controls.Clear();
            CampaignController controller = CampaignMasterClientTest.getCampaignController(Session);

            string contextPlayerId = (string) Session[CampaignMasterClientKeys.CONTEXTPLAYERID];
            //Player 


            //Table fldTab = drawField(controller.campaignEngine.FieldField, 
            //this.Controls.Add(fldTab);
        }


        private Table drawField(GenericCampaignMasterLib.Field field, Player context)
        {
            Table tab = new Table();
            tab.BorderWidth = Unit.Pixel(1);
            tab.Width = Unit.Pixel(600);
            tab.Height = Unit.Pixel(400);

            TableRow row = new TableRow();
            

            foreach (Sektor s in field.getSektorList())
            {
                TableCell cell = new TableCell();
                cell.Style.Add("vertical-align", "top");
                cell.Style.Add("horizontal-align", "center");

                cell.BorderWidth = Unit.Pixel(1);
                cell.Text = s.Id;
                row.Cells.Add(cell);
            }

            tab.Rows.Add(row);
            return tab;
        }

    }
}