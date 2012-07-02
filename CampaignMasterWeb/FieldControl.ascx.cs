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
			
            CampaignController controller = GcmlClient.getCampaignController(this.Session);
            Field field = GcmlClient.getField(this.Session);
            Player currentPlayer = controller.getPlayer(aktplayerid);

            drawField(panelField, controller);
            drawPlayerPanel(panelPlayer, currentPlayer);
        }
		
		/// <summary>
		/// Zeichnet das Menü und den Statusbereich für einen Spieler
		/// </summary>
        private void drawPlayerPanel(Panel panel, Player player)
        {
            Label labelInfo = new Label();
            labelInfo.Text = "Spieler: " + player.Id + " - " + player.Playername + "<br />";
            panel.Controls.Add(labelInfo);

            Panel unitPanel = getUnitPanel(player.ListUnits);
            panel.Controls.Add(unitPanel);
        }
		
		/// <summary>
		/// Zeichnet die Spielfeldansicht für einen Spieler
		/// </summary>
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

        private Panel getUnitPanel(List<IUnit> lstUnits)
        {
            ListBox listUnits = new ListBox();
            foreach (IUnit unit in lstUnits)
            {
                ListItem it = new ListItem();
                it.Text = unit.Id.ToString() + " : " + unit.Bezeichnung;
                it.Value = unit.Id.ToString();
                listUnits.Items.Add(it);
            }

            Button btnUnitActions = new Button();
            btnUnitActions.Text = "Unit aktivieren";
            btnUnitActions.Click += new EventHandler(unitSelected);


            Label lbUnitActions = new Label();
            lbUnitActions.Text = "AKtionen: ";

            ListBox listUnitActions = new ListBox();
            listUnitActions.ID = "lbUnitActions";

            Panel unitPanel = new Panel();
            unitPanel.Controls.Add(listUnits);
            unitPanel.Controls.Add(btnUnitActions);
            unitPanel.Controls.Add(lbUnitActions);
            unitPanel.Controls.Add(listUnitActions);

            return unitPanel;
        }

		private void drawSektor(TableRow row, Sektor s, CampaignController controller)
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
                Panel unitPanel = getUnitPanel(s.ListUnits);
                cell.Controls.Add(unitPanel);
            }
        }

        protected void btnSelectPlayer_Click(object sender, EventArgs e)
        {
            CampaignController controller = GcmlClient.getCampaignController(this.Session);
            string id = dropDownPlayer.SelectedValue;
            Player player = controller.getPlayer(id);
            ViewState[CampaignMasterClientKeys.CONTEXTPLAYERID] = player.Id;
        }
		
		protected void unitSelected (object sender, EventArgs e)
		{
			Button btn = sender as Button;
            ControlCollection ctrls = btn.Parent.Controls;
            ListBox lb = ctrls.OfType<ListBox>().First();


			string unitId = lb.SelectedValue;
			
			IUnit unit = GcmlClient.getUnitById (unitId, this.Session);
			
			
		}


    }
}