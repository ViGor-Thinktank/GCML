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
            playerId = (string)Session[GcmlClientKeys.CONTEXTPLAYERID];
            campaignId = (string)Session[GcmlClientKeys.CAMPAIGNID];

            drawPlayerResources();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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

        private void drawPlayerResources()
        {
            CampaignMasterService service = StartMenu.getService(this.Session);
            lbRessourcen.Items.Clear();
            foreach (var resource in service.getResourcesForPlayer(campaignId, playerId))
            {
                ListItem li = new ListItem();
                li.Text = resource.resourceableType;
                li.Value = resource.resourceId;
                lbRessourcen.Items.Add(li);
            }
        }

        private void drawResourceCommands(string resId)
        {
            lbRessourceActions.Items.Clear();
            CampaignMasterService service = StartMenu.getService(this.Session);
            if (!String.IsNullOrEmpty(resId))
            {
                foreach (CommandInfo cmdinf in service.getCommandsForResource(campaignId, resId))
                {
                    ListItem li = new ListItem();
                    li.Text = cmdinf.commandType + "#" + cmdinf.strInfo;
                    li.Value = cmdinf.commandId;
                    lbRessourceActions.Items.Add(li);
                }
            }
        }

        protected void lbRessourcen_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = sender as ListBox;
            ListItem selitem = lb.SelectedItem;
            string selectedResId = selitem.Value;
            drawResourceCommands(selectedResId);
        }

        protected void btnPlaceRessource_Click(object sender, EventArgs e)
        {
            CampaignMasterService service = StartMenu.getService(this.Session);
            ListItem selitem = lbRessourceActions.SelectedItem;
            if (selitem != null)
            {
                string cmdid = selitem.Value;
                CommandInfo cmd = service.getCommandInfo(campaignId, cmdid);
                service.executeCommand(campaignId, cmd);
            }
        }
    }

   
}