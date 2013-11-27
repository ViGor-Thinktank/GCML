using System.Collections.Generic;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;

namespace GcmlDataAccess
{
    public interface IGcmlDataAccess
    {
        CampaignController getCampaignController(string campaignId);
        CampaignController createNewCampaign(CampaignInfo info);
        bool safeCampaignState(CampaignController controller);
        PlayerInfo getPlayer(string playername);
        List<PlayerInfo> getPlayers();
        List<CampaignInfo> getCampaignsForPlayer(string playername);
    }
}