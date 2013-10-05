using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.IO;
using GenericCampaignMasterModel;

namespace GenericCampaignMasterLib
{
    public interface IGcmlDataManager
    {
        List<string> getRunningCampaignIds();
        CampaignController getController(string campaignId);
        PlayerInfo getPlayerByName(string playername);
        string getPlayerId(string playername);     // TODO: Kann raus
        PlayerInfo getPlayer(string playerId);
        Dictionary<string, PlayerInfo> getPlayerList();
        List<CampaignInfo> getRunningPlayerCampaigns(string playerid);
        string createNewCampaign(string campaignname, clsSektorKoordinaten fielddim);
        string createNewCampaign(string playerid, string campaignname, clsSektorKoordinaten fielddim, int anzUnitsPerPlayer);
        void safeCampaignState(CampaignState state);

        void addPlayer(PlayerInfo player);
    }
}
