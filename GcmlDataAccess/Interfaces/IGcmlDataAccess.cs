using System;
using System.Collections.Generic;
using GenericCampaignMasterModel;

namespace GenericCampaignMasterLib
{
    public interface IGcmlDataAccess
    {
        List<CampaignInfo> getCampaignsForPlayer(string playerId);
        CampaignController getCampaignController(string campaignId);
        PlayerInfo getPlayerInfo(string playerId);
        PlayerInfo getPlayerByName(string playername);
        List<PlayerInfo> getPlayers();
        string createNewCampaign(CampaignInfo info);
        bool safeCampaignState(CampaignState state);
        bool safePlayer(PlayerInfo info);
    }
}
