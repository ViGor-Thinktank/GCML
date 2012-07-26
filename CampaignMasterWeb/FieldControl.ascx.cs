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

        protected void Page_PreRender(object sender, EventArgs e)
        {
            drawPlayerContext();
        }

        /// <summary>
        /// Erzeugt die statischen Formularelemente für die Spielfeldansicht (alles von der Spielersicht abhängt)
        /// </summary>
        private void drawForm()
        {
            if (panelField.Controls.Count > 1)
                return;

            List<SektorControl> sektorStack = new List<SektorControl>();
            CampaignController controller = GcmlClientWeb.getCampaignController(Session);
            
            Table tab = new Table();

            // Feld zeilenweise erstellen
            for (int y = 0; y <= controller.campaignEngine.FieldField.FieldDimension.Y; y++)
            {
                tab.Rows.Add(new TableRow());

                for (int x = 0; x <= controller.campaignEngine.FieldField.FieldDimension.X; x++)
                {
                    clsSektorKoordinaten getkoord = new clsSektorKoordinaten(x, y, 0);
                    Sektor sektor = controller.campaignEngine.FieldField.get(getkoord);

                    TableCell c = new TableCell();
                    tab.Rows[y].Cells.Add(c);


                    SektorControl sc = (SektorControl)Page.LoadControl("SektorControl.ascx");
                    sc.Sektor = sektor;
                    c.Controls.Add(sc);

                    sektorStack.Add(sc);
                }
            }
            
            panelField.Controls.Add(tab);

            Session[GcmlClientKeys.SEKTORSTACK] = sektorStack;
        }

        public void drawPlayerContext()
        {
            List<SektorControl> sektorStack = (List<SektorControl>) Session[GcmlClientKeys.SEKTORSTACK] ;
            if (sektorStack == null)
                return;

            foreach (SektorControl sektorCtrl in sektorStack)
                sektorCtrl.drawContext();
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

    }
}