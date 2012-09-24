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
        private string campaignId;
        private string playerId;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            playerId = (string)Session[GcmlClientKeys.CONTEXTPLAYERID];
            campaignId = (string)Session[GcmlClientKeys.CAMPAIGNID];

            CampaignMasterService service = StartMenu.getService(this.Session);
            PlayerInfo playernfo = service.getPlayerInfo(playerId);
            CampaignInfo campaignnfo = service.getCampaignInfo(campaignId);

            lbPlayer.Text = playernfo.playerName;
            lbCampaign.Text = campaignnfo.campaignName;
        }

        protected void btnEndRound_Click(object sender, EventArgs e)
        {
            CampaignMasterService service = StartMenu.getService(this.Session);
            service.endRoundForPlayer(campaignId, playerId);
            Session[GcmlClientKeys.CONTEXTCOMMANDLIST] = new Dictionary<string, CommandInfo>();
        }
    }

   
}