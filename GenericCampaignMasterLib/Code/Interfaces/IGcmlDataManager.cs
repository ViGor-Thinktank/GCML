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
         string getPlayerId(string playername);
         PlayerInfo getPlayer(string playerId);
         Dictionary<string, PlayerInfo> getPlayerList();
         List<CampaignInfo> getRunningPlayerCampaigns(string playerid);
         string createNewCampaign(string playerid, string fielddimension);
         string createNewCampaign(string playerid, string campaignname, clsSektorKoordinaten fielddim, int anzUnitsPerPlayer);
        
    }
}
