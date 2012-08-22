using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CampaignMasterWeb.GcmlWsReference;


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
            if (String.IsNullOrEmpty((string)this.Session[GcmlClientKeys.CONTEXTPLAYERID]))
                setLoggedOff();
            else
                setLoggedIn();

            drawPlayerCampaignData();

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string playername = tbPlayername.Text;

            if(!String.IsNullOrEmpty(playername))
            {
                string playerId = GcmlClientWeb.getService(this.Session).getPlayerId(playername);
                if (!String.IsNullOrEmpty(playerId))
                    this.Session[GcmlClientKeys.CONTEXTPLAYERID] = playerId;
            }
        }

        protected void BtnNewCampaign_Click(object sender, EventArgs e)
        {
            CampaignMasterService service = GcmlClientWeb.getService(Session);
            string playerid = (string) this.Session[GcmlClientKeys.CONTEXTPLAYERID];
            string campaignid = service.createNewCampaign(playerid, "");

        }

        protected void btnLoadCampaign_Click(object sender, EventArgs e)
        {
            CampaignMasterService service = GcmlClientWeb.getService(Session);
            string campaignid = lbCampaigns.SelectedItem.Text;
            this.Session[GcmlClientKeys.CAMPAIGNID] = campaignid;

            Response.Redirect("test.aspx");

        }

        private void drawPlayerCampaignData()
        {
            lbCampaigns.Items.Clear();

            string currentPlayerId = (string)this.Session[GcmlClientKeys.CONTEXTPLAYERID];
            if (String.IsNullOrEmpty(currentPlayerId))
                return;

            CampaignMasterService gcmlservice = GcmlClientWeb.getService(this.Session);
            List<string> campaigns = gcmlservice.getPlayerCampaigns(currentPlayerId).ToList<string>();
            foreach (string strCampaign in campaigns)
            {
                lbCampaigns.Items.Add(strCampaign);


            }

        }

        protected void btnLogoff_Click(object sender, EventArgs e)
        {
            this.Session[GcmlClientKeys.CONTEXTPLAYERID] = "";
            setLoggedOff();
        }

        private void setLoggedIn()
        {
            tbPlayername.Enabled = false;
            btnLogin.Enabled = false;
            btnLogoff.Enabled = true;
            pnPlayerCampaigns.Enabled = true;
        }

        private void setLoggedOff()
        {
            tbPlayername.Text = "";
            tbPlayername.Enabled = true;
            btnLogin.Enabled = true;
            btnLogoff.Enabled = false;
            pnPlayerCampaigns.Enabled = false;
        }

        protected void btnAddPlayerToCampaign_Click(object sender, EventArgs e)
        {
            string addPlayername = tbAddPlayername.Text;
            if (!String.IsNullOrEmpty(addPlayername))
            {
                CampaignMasterService gcmlservice = GcmlClientWeb.getService(this.Session);
                string playerId = gcmlservice.getPlayerId(addPlayername);
                string campaignid = lbCampaigns.SelectedItem.Text;
                if (!String.IsNullOrEmpty(playerId) && !String.IsNullOrEmpty(campaignid))
                {
                    gcmlservice.addPlayerToCampaign(playerId, campaignid);

                }
            }
        } 
    }
}