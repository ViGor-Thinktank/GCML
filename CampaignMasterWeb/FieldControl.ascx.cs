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
            Field field = CampaignMasterClientTest.getField(this.Session);
            Player contextPlayer = CampaignMasterClientTest.getContextPlayer(this.Session);
            drawField(field, contextPlayer);
        }


        private Table drawField(GenericCampaignMasterLib.Field field, Player context)
        {
            if (context == null)
                context = new Player("King Power");

            Table tab = new Table();
            tab.BorderWidth = Unit.Pixel(1);
            tab.Width = Unit.Pixel(600);
            tab.Height = Unit.Pixel(400);

            TableRow row = new TableRow();
            foreach (Sektor s in field.getSektorList())
            {
                string bgcolor = "light-gray";

                TableCell cell = new TableCell();
                cell.Style.Add("vertical-align", "top");
                cell.Style.Add("horizontal-align", "center");
                cell.Style.Add("background", bgcolor);

                cell.BorderWidth = Unit.Pixel(1);
                cell.Text = s.Id;
                row.Cells.Add(cell);
            }

            tab.Rows.Add(row);
            return tab;
        }

    }
}