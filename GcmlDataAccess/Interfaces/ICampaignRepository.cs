using System;
using System.Collections.Generic;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;

namespace GcmlDataAccess
{
    public interface ICampaignRepository : IDisposable
    {
        ICampaignController getCampaignController(string campaignId);
        ICampaignController createNewCampaign(CampaignInfo info);
        bool safeCampaignState(ICampaignController controller);
        PlayerInfo getPlayer(string playername);
        List<PlayerInfo> getPlayers();
        List<CampaignInfo> getCampaignsForPlayer(string playername);
    }
}