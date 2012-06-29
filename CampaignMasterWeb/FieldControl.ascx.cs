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

            drawField(panelField, controller);
            drawPlayerPanel(panelPlayer, currentPlayer);
        }

        private void drawPlayerPanel(Panel panel, Player player)
        {
            Label labelInfo = new Label();
            labelInfo.Text = "Spieler: " + player.Id + " - " + player.Playername + "<br />";
            panel.Controls.Add(labelInfo);

            ListBox lb = getUnitList(player.ListUnits);
            panel.Controls.Add(lb);
        }

        private static ListBox getUnitList(List<IUnit> lstUnits)
        {
            ListBox lb = new ListBox();
            foreach (IUnit unit in lstUnits)
            {
                ListItem it = new ListItem();
                it.Text = unit.Id.ToString() + " : " + unit.Bezeichnung;
                it.Value = unit.Id.ToString();
                lb.Items.Add(it);
            }
            return lb;
        }

        private void drawField(Panel panel, CampaignController controller)
        {
            Table tab = new Table();
            tab.BorderWidth = Unit.Pixel(1);
            tab.Width = Unit.Pixel(500);
            tab.Height = Unit.Pixel(250);

            TableRow row = new TableRow();
            foreach (Sektor s in controller.campaignEngine.FieldField.getSektorList())
                drawSektor(row, s, controller);

            tab.Rows.Add(row);
            panel.Controls.Add(tab);
        }

        private static void drawSektor(TableRow row, Sektor s, CampaignController controller)
        {
            string bgcolor = "light-gray";
            if (controller.getUnitCollisions().Contains(s))
                bgcolor = "light-orange";

            TableCell cell = new TableCell();
            cell.Style.Add("vertical-align", "top");
            cell.Style.Add("horizontal-align", "center");
            cell.Style.Add("background", bgcolor);
            cell.BorderWidth = Unit.Pixel(1);
            row.Cells.Add(cell);

            Label sektorinfo = new Label();
            sektorinfo.Text = "Sektor: " + s.Id + "<br />";
            cell.Controls.Add(sektorinfo);

            if (s.ListUnits.Count() > 0)
            {
                ListBox lb = getUnitList(s.ListUnits);
                cell.Controls.Add(lb);
            }
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