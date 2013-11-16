using System;
namespace GenericCampaignMasterLib
{
    public interface IGcmlDataDb
    {
        System.Collections.Generic.List<GenericCampaignMasterModel.CampaignInfo> getCampaignsForPlayer(string playerId);
        GenericCampaignMasterModel.CampaignState getCampaignState(string campaignId);
        GenericCampaignMasterModel.PlayerInfo getPlayerInfo(string playerId);
        System.Collections.Generic.List<GenericCampaignMasterModel.PlayerInfo> getPlayers();
        bool safeCampaignState(GenericCampaignMasterModel.CampaignState state);
        bool safePlayer(GenericCampaignMasterModel.PlayerInfo info);
    }
}
