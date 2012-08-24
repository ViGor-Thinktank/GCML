using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public interface ICampaignDatabase
    {
        string CampaignKey { get; set; }
        string saveGameState(CampaignState state);          // Rückgabewert ist der Schlüssel des States in der DB
        CampaignState getLastGameState();
        CampaignState getCampaignStateByKey(string key);
        CampaignState getCampaignStateByDate(DateTime time);
        List<CampaignState> getAllCampaignStates();
        List<DateTime> getStateList();
        void close();
    }
}
