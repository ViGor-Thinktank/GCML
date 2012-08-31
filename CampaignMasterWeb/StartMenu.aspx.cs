using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using CampaignMasterWeb.GcmlWsReference;


namespace CampaignMasterWeb
{
    public struct GcmlClientKeys
    {
        public const string CAMPAIGNSTATE = "campaignstate";
        public const string CAMPAIGNID = "campaignid";
        public const string CONTEXTPLAYERID = "contextplayerid";
        public const string CAMPAIGNCONTROLLER = "campaigncontroller";
        public const string CONTEXTUNITID = "contextunitid";
        public const string CONTEXTCOMMANDLIST = "contextcommandlist";
        public const string SEKTORSTACK = "sektorstack";
    }

    public partial class StartMenu : System.Web.UI.Page
    {
        public static GcmlWsReference.CampaignMasterService getService(HttpSessionState state)
        {
            GcmlWsReference.CampaignMasterService gcmlservice = (GcmlWsReference.CampaignMasterService)state["gcmlservice"];
            if (gcmlservice == null)
            {
                gcmlservice = new GcmlWsReference.CampaignMasterService();
                state["gcmlservice"] = gcmlservice;
            }

            return gcmlservice;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        // Wird nach dem (Login-) Clickevent ausgelöst
        protected void Page_PreRender(object sender, EventArgs e)
        {
            string selectedCampaign = (string)this.Session[GcmlClientKeys.CAMPAIGNID];
            if (String.IsNullOrEmpty(selectedCampaign))
            {
                pnCampaignInfo.Enabled = false;
                ListItem it = lbCampaigns.Items.FindByText(selectedCampaign);
                if(it != null)
                    it.Selected = true;
            }
            else
                pnCampaignInfo.Enabled = true;

            if (String.IsNullOrEmpty((string)this.Session[GcmlClientKeys.CONTEXTPLAYERID]))
                setLoggedOff();
            else
                setLoggedIn();

            drawPlayerCampaignData();
            showCampaignInfoPanel();

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string playername = tbPlayername.Text;

            if(!String.IsNullOrEmpty(playername))
            {
                string playerId = StartMenu.getService(this.Session).getPlayerId(playername);
                if (!String.IsNullOrEmpty(playerId))
                    this.Session[GcmlClientKeys.CONTEXTPLAYERID] = playerId;
            }
        }

        protected void BtnNewCampaign_Click(object sender, EventArgs e)
        {
            CampaignMasterService service = StartMenu.getService(this.Session);
            string playerid = (string) this.Session[GcmlClientKeys.CONTEXTPLAYERID];
            string campaignid = service.createNewCampaign(playerid, "");

        }

        protected void btnLoadCampaign_Click(object sender, EventArgs e)
        {
            

            Response.Redirect("test.aspx");

        }

        private void drawPlayerCampaignData()
        {
            lbCampaigns.Items.Clear();

            string currentPlayerId = (string)this.Session[GcmlClientKeys.CONTEXTPLAYERID];
            if (String.IsNullOrEmpty(currentPlayerId))
                return;

            CampaignMasterService gcmlservice = StartMenu.getService(this.Session);
            List<string> campaigns = gcmlservice.getPlayerCampaigns(currentPlayerId).ToList<string>();
            foreach (string strCampaign in campaigns)
            {
                ListItem cmpItem = new ListItem();
                cmpItem.Text = strCampaign;
                cmpItem.Value = strCampaign;
                lbCampaigns.Items.Add(cmpItem);
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


        private void showCampaignInfoPanel()
        {
            string campaignId = (string)this.Session[GcmlClientKeys.CAMPAIGNID];
            if (String.IsNullOrEmpty(campaignId))
            {
                pnCampaignInfo.Visible = false;
            }
            else
            {
                CampaignMasterService service = StartMenu.getService(this.Session);
                CampaignInfo nfo = service.getCampaignInfo(campaignId);
                lbId.Text = nfo.campaignId;
                lbName.Text = nfo.campaignName;
                string players = "";
                foreach(var kvp in nfo.players)
                    players += (String.IsNullOrEmpty(players)) ? kvp.Value : ", " + kvp.Value;

                lbPlayer.Text = players;
                pnCampaignInfo.Visible = true;

                lbCampaigns.SelectedValue = campaignId;
            }

        }

        protected void btnAddPlayerToCampaign_Click(object sender, EventArgs e)
        {
            string addPlayername = tbAddPlayername.Text;
            if (!String.IsNullOrEmpty(addPlayername))
            {
                CampaignMasterService gcmlservice = StartMenu.getService(this.Session);
                string playerId = gcmlservice.getPlayerId(addPlayername);
                string campaignid = lbCampaigns.SelectedItem.Text;
                if (!String.IsNullOrEmpty(playerId) && !String.IsNullOrEmpty(campaignid))
                {
                    gcmlservice.addPlayerToCampaign(playerId, campaignid);

                }
            }
        }

        protected void lbCampaigns_SelectedIndexChanged(object sender, EventArgs e)
        {
            CampaignMasterService service = StartMenu.getService(this.Session);
            string campaignid = lbCampaigns.SelectedItem.Text;
            this.Session[GcmlClientKeys.CAMPAIGNID] = campaignid;
        } 
    }
}