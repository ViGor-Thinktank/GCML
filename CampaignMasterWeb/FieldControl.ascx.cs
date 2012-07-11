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
                drawForm();   
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void setCurrentPlayer()
        {
            string id = dropDownPlayer.SelectedValue;
            Player player = GcmlClientWeb.getCampaignController(this.Session).getPlayer(id);
            Session[GcmlClientKeys.CONTEXTPLAYERID] = id;
        }


        protected Player getCurrentPlayer()
        {
            string id = (string) Session[GcmlClientKeys.CONTEXTPLAYERID];
            Player player = GcmlClientWeb.getCampaignController(this.Session).getPlayer(id);
            return player;
        }

        /// <summary>
        /// Erzeugt die statischen Formularelemente für die Spielfeldansicht
        /// </summary>
        private void drawForm()
        {

            Player aktplayer = getCurrentPlayer();
            if (aktplayer == null)
            {
                Label hinweis = new Label();
                hinweis.Text = "Bitte einloggen";
                panelPlayer.Controls.Add(hinweis);
                return;
            }

            CampaignController controller = GcmlClientWeb.getCampaignController(Session);
            Label labelInfo = new Label();
            labelInfo.Text = "Spieler: " + aktplayer.Id + " - " + aktplayer.Playername + "<br />";
            panelPlayer.Controls.Add(labelInfo);

            Table tab = new Table();
            tab.Rows.Add(new TableRow());
            foreach (Sektor sektor in controller.campaignEngine.FieldField.getSektorList())
            {
                TableCell c = new TableCell();
                tab.Rows[0].Cells.Add(c);

                SektorControl sc = (SektorControl)Page.LoadControl("SektorControl.ascx");
                sc.Sektor = sektor;
                c.Controls.Add(sc);

            }

            panelField.Controls.Add(tab);
        }



        private void drawPlayerContext()
        {
            List<Panel> sektorStack = (List<Panel>) Session[GcmlClientKeys.SEKTORSTACK];
            foreach (Panel sektor in sektorStack)
            {



            }


            CampaignController controller = GcmlClientWeb.getCampaignController(Session);
            Field field = GcmlClientWeb.getField(Session);
            Player player = getCurrentPlayer();
            foreach (IUnit unit in player.ListUnits)
            {
                Sektor containingSek = controller.getSektorForUnit(unit);
                Panel panelSek = sektorStack.First(p => p.ID == "sektor#" + containingSek.Id);



            }
        }
		
		
		/// <summary>
		/// Zeichnet die Spielfeldansicht für einen Spieler
		/// </summary>
		private void drawField(Panel panel, CampaignController controller)
        {
            
        }


        //private Panel drawSektor(TableRow row, Sektor s, CampaignController controller)
        //{
        //    Panel sektorPanel = new Panel();
        //    sektorPanel.ID = "sektor#" + s.Id;

            

        //    Label sektorinfo = new Label();
        //    sektorinfo.Text = "Sektor: " + s.Id + "<br />";
        //    sektorPanel.Controls.Add(sektorinfo);

        //    if (s.ListUnits.Count() > 0)
        //    {
        //        Panel unitPanel = getUnitPanel(s.ListUnits);
        //        sektorPanel.Controls.Add(unitPanel);
        //    }

        //    return sektorPanel;
        //}

        //private Panel getUnitPanel(List<IUnit> lstUnits)
        //{
        //    string selectedUnitId = (string) Session[GcmlClientKeys.CONTEXTUNITID];
        //    IUnit selectedUnit = null;
			
        //    //string panelSessionId = panelControlSessionId.Value;
        //    //string panelSessionId = new Random().Next(1000, 9999).ToString();

           
        //    listUnits.AutoPostBack = true;
        //    listUnits.SelectedIndexChanged += new EventHandler(unitSelected);

        //    Button btnUnitActions = new Button();
        //    btnUnitActions.Text = "Unit aktivieren";
        //    btnUnitActions.Click += new EventHandler(unitSelected);

        //    Label lbUnitActions = new Label();
        //    lbUnitActions.Text = "Aktionen: ";

        //    ListBox listUnitActions = new ListBox();
        //    if(selectedUnit != null)
        //    {
        //        CampaignController controller = GcmlClient.getCampaignController(this.Session);
        //        List<ICommand> cmds = controller.getCommandsForUnit(selectedUnit);
        //        foreach (ICommand cmd in cmds)	
        //        {
        //            listUnitActions.Items.Add (cmd.strInfo);
        //        }
        //    }
			   

        //    Panel unitPanel = new Panel();
        //    //unitPanel.ID = "unitPanel#" + panelSessionId;
        //    unitPanel.Controls.Add(listUnits);
        //    unitPanel.Controls.Add(btnUnitActions);
        //    unitPanel.Controls.Add(lbUnitActions);
        //    unitPanel.Controls.Add(listUnitActions);
        //    return unitPanel;
        //}

		

		

        protected void btnSelectPlayer_Click(object sender, EventArgs e)
        {
            setCurrentPlayer();
            drawForm();
        }

    }
}