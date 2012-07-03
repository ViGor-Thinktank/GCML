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
        protected void Page_Init(object sender, EventArgs e)
        {
            drawPlayerView();

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            
        }


        private void drawPlayerView()
        {
            Player aktplayer = getCurrentPlayer();
            if (aktplayer == null)
            {
                Label hinweis = new Label();
                hinweis.Text = "Bitte einloggen";
                panelPlayer.Controls.Add(hinweis);
            }
            else
            {
                //if (String.IsNullOrEmpty(panelControlSessionId.Value))
                //    panelControlSessionId.Value = new Random().Next(1000, 9999).ToString();

                CampaignController controller = GcmlClient.getCampaignController(this.Session);
                Field field = GcmlClient.getField(this.Session);
                drawField(panelField, controller);
                drawPlayerPanel(panelPlayer, aktplayer);
            }
        }

        protected void btnSelectPlayer_Click(object sender, EventArgs e)
        {
            setCurrentPlayer();
        }

        protected void setCurrentPlayer()
        {
            string id = dropDownPlayer.SelectedValue;
            Player player = GcmlClient.getCampaignController(this.Session).getPlayer(id);
            Session[GcmlClientKeys.CONTEXTPLAYERID] = id;
        }


        protected Player getCurrentPlayer()
        {
            string id = (string) Session[GcmlClientKeys.CONTEXTPLAYERID];
            Player player = GcmlClient.getCampaignController(this.Session).getPlayer(id);
            return player;
        }
        
		
		/// <summary>
		/// Zeichnet das Menü und den Statusbereich für einen Spieler
		/// </summary>
        private void drawPlayerPanel(Panel panel, Player player)
        {
            Label labelInfo = new Label();
            labelInfo.Text = "Spieler: " + player.Id + " - " + player.Playername + "<br />";
            panel.Controls.Add(labelInfo);

            //Panel unitPanel = getUnitPanel(player.ListUnits, panelControlSessionId.Value);
            //panel.Controls.Add(unitPanel);
        }
		
		/// <summary>
		/// Zeichnet die Spielfeldansicht für einen Spieler
		/// </summary>
		private void drawField(Panel panel, CampaignController controller)
        {
            string fieldTabId = "FieldTab#" + panelControlSessionId.Value;

            if (panel.Controls.OfType<Table>().Select(tb => tb.ID == fieldTabId).Count() > 0)
                return;

            Table tab = new Table();
            tab.BorderWidth = Unit.Pixel(1);
            tab.Width = Unit.Pixel(500);
            tab.Height = Unit.Pixel(250);
            tab.ID = "FieldTab#" + panelControlSessionId.Value;

            TableRow row = new TableRow();
            foreach (Sektor s in controller.campaignEngine.FieldField.getSektorList())
                drawSektor(row, s, controller);

            tab.Rows.Add(row);
            panel.Controls.Add(tab);
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
                //unitPanel.ID = "unitPanel#" + panelControlSessionId + "#Sektor#" + s.Id;
                cell.Controls.Add(unitPanel);
            }
        }

        private Panel getUnitPanel(List<IUnit> lstUnits)
        {
            //string panelSessionId = panelControlSessionId.Value;
            //string panelSessionId = new Random().Next(1000, 9999).ToString();

            ListBox listUnits = new ListBox();
            //listUnits.ID = "listUnits#" + panelSessionId;
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
            lbUnitActions.Text = "Aktionen: ";

            ListBox listUnitActions = new ListBox();
            //listUnitActions.ID = "listUnitActions#" + panelSessionId;

            Panel unitPanel = new Panel();
            //unitPanel.ID = "unitPanel#" + panelSessionId;
            unitPanel.Controls.Add(listUnits);
            unitPanel.Controls.Add(btnUnitActions);
            unitPanel.Controls.Add(lbUnitActions);
            unitPanel.Controls.Add(listUnitActions);
            return unitPanel;
        }

		

		protected void unitSelected (object sender, EventArgs e)
		{
            //CampaignController controller = GcmlClient.getCampaignController(this.Session);
            
            //Button btn = sender as Button;
            //ControlCollection ctrls = btn.Parent.Controls;
            //ListBox listUnits = ctrls.OfType<ListBox>().First(l => l.ID.Contains("listUnits"));
            //ListBox listUnitActions = ctrls.OfType<ListBox>().First(l => l.ID.Contains("listUnitActions"));

            //string unitId = listUnits.SelectedValue;
            //IUnit unit = controller.getUnit(unitId);

            //List<ICommand> lstCmds = controller.getCommandsForUnit(unit);
            
            
			
		}

    }
}