using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public interface ICampaignDatabase
    {
        string initDatabase();                              // Initialisiert einer neue DB und liefert einen eindeutigen Schlüssel der Kampagne zurück
        void initDatabase(string campaignIdentifier);       // Lädt eine Db anhand der CampaignId                           
        string saveGameState(CampaignState state);          // Rückgabewert ist der Schlüssel des States in der DB
        CampaignState getLastGameState();
        CampaignState getCampaignStateByKey(string key);
        CampaignState getCampaignStateByDate(DateTime time);
        List<CampaignState> getAllCampaignStates();
        List<DateTime> getStateList();
    }
}
