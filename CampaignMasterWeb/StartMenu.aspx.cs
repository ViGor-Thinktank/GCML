using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GcmlWebService;

namespace CampaignMasterWeb
{
    public partial class StartMenu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // Wird nach dem (Login-) Clickevent ausgelöst
        protected void Page_PreRender(object sender, EventArgs e)
        {
            drawPlayerCampaignData();

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string playername = tbPlayername.Text;

            if(!String.IsNullOrEmpty(playername))
            {
                string intPlayername = GcmlClientWeb.getService(this.Session).getPlayer(playername);
                if (!String.IsNullOrEmpty(intPlayername))
                    this.Session[GcmlClientKeys.CONTEXTPLAYERID] = intPlayername;
            }
        }

        protected void BtnNewCampaign_Click(object sender, EventArgs e)
        {

        }

        protected void btnLoadCampaign_Click(object sender, EventArgs e)
        {

        }

        private void drawPlayerCampaignData()
        {
            lbCampaigns.Items.Clear();

            string currentPlayer = (string)this.Session[GcmlClientKeys.CONTEXTPLAYERID];
            if (String.IsNullOrEmpty(currentPlayer))
                return;

            CampaignMasterService gcmlservice = GcmlClientWeb.getService(this.Session);
            List<string> campaigns = gcmlservice.getPlayerCampaigns(currentPlayer);
            foreach (string strCampaign in campaigns)
            {
                lbCampaigns.Items.Add(strCampaign);


            }


        }
    }
}