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
        // Neue Methoden. Ansatz: Eine ICampaignDatabase für alle Campaigns.
        List<string> getAllCampaignKeys();
        CampaignState getCampaignStateForCampaign(string campaignKey);
        List<CampaignInfo> getCampaignsForPlayer(string p);
        string createNewCampaign(Player p, string campaignname, clsSektorKoordinaten fielddim);

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
