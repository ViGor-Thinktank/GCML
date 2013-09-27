using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterModel;

namespace GenericCampaignMasterLib
{
    // Das Interface beschreibt den Zugriff auf eine Datenbank
    // mit allen Kampagnen - nicht wie bisher einer Kampagne.
    public interface ICampaignDatabase
    {
        // Neue Methoden
        List<string> getAllCampaignKeys();
        CampaignState getCampaignStateForCampaign(string campaignKey);

        // Alte Methoden. Ursprünglicher Ansatz: Pro Campaign eine DB mit ICampaignDatabase.
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
