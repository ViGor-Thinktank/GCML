using System;
namespace GenericCampaignMasterLib
{
    public interface IGcmlDataAccess
    {
        System.Collections.Generic.List<GenericCampaignMasterModel.CampaignInfo> getCampaignsForPlayer(string playerId);
        GenericCampaignMasterLib.CampaignController getCampaignController(string campaignId);
        GenericCampaignMasterModel.PlayerInfo getPlayerInfo(string playerId);
        System.Collections.Generic.List<GenericCampaignMasterModel.PlayerInfo> getPlayers();
        string createNewCampaign(GenericCampaignMasterModel.CampaignInfo info);
        bool safeCampaignState(GenericCampaignMasterModel.CampaignState state);
        bool safePlayer(GenericCampaignMasterModel.PlayerInfo info);
    }
}
