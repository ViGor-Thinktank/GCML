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
    public partial class GcmlClientPage : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string playerid = (string)Session[GcmlClientKeys.CONTEXTPLAYERID];
            string campaignid = (string)Session[GcmlClientKeys.CAMPAIGNID];

            CampaignMasterService service = StartMenu.getService(this.Session);
            PlayerInfo playernfo = service.getPlayerInfo(playerid);
            CampaignInfo campaignnfo = service.getCampaignInfo(campaignid);

            lbPlayer.Text = playernfo.playerName;
            lbCampaign.Text = campaignnfo.campaignName;
        }
    }

   
}