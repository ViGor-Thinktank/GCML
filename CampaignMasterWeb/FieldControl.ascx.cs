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
            string aktplayerid = (string) ViewState[CampaignMasterClientKeys.CONTEXTPLAYERID];
            if(String.IsNullOrEmpty(aktplayerid))
            {
                Label hinweis = new Label();
                hinweis.Text = "Bitte einloggen";
                panelPlayer.Controls.Add(hinweis);
                return;

            }

            CampaignController controller = CampaignMasterClientTest.getCampaignController(this.Session);
            Field field = CampaignMasterClientTest.getField(this.Session);
            Player currentPlayer = controller.getPlayer(aktplayerid);
            

            drawField(panelField, field);
            drawPlayerPanel(panelPlayer, currentPlayer);
        }

        private void drawPlayerPanel(Panel panel, Player player)
        {
            Label labelInfo = new Label();
            labelInfo.Text = player.Id + " : " + player.Playername;
            panel.Controls.Add(labelInfo);

            ListBox lb = new ListBox();
            foreach (IUnit unit in player.ListUnits)
            {
                ListItem it = new ListItem();
                it.Text = unit.Id.ToString() + " : " + unit.Bezeichnung;
                it.Value = unit.Id.ToString();
                lb.Items.Add(it);
            }
            panel.Controls.Add(lb);
        }


        private void drawField(Panel panel, GenericCampaignMasterLib.Field field)
        {
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
            panel.Controls.Add(tab);
        }

        protected void btnSelectPlayer_Click(object sender, EventArgs e)
        {
            CampaignController controller = CampaignMasterClientTest.getCampaignController(this.Session);
            string id = dropDownPlayer.SelectedValue;
            Player player = controller.getPlayer(id);
            ViewState[CampaignMasterClientKeys.CONTEXTPLAYERID] = player.Id;
        }


    }
}