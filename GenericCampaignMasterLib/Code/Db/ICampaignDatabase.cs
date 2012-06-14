using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    interface ICampaignDatabase
    {
        public void initDatabase();
        public void saveGameState(CampaignState state);
        public CampaignState getLastGameState();
        public CampaignState getCampaignStateByKey(string key);
        public CampaignState getCampaignStateByDate(DateTime time);
        public List<CampaignState> getAllCampaignStates();
    }
}
