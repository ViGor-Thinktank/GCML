using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    class CampaignDatabaseRaptorDb : ICampaignDatabase
    {
        #region ICampaignDatabase Member

        public void initDatabase()
        {
            throw new NotImplementedException();
        }

        public void saveGameState(CampaignState state)
        {
            throw new NotImplementedException();
        }

        public CampaignState getLastGameState()
        {
            throw new NotImplementedException();
        }

        public CampaignState getCampaignStateByKey(string key)
        {
            throw new NotImplementedException();
        }

        public CampaignState getCampaignStateByDate(DateTime time)
        {
            throw new NotImplementedException();
        }

        public List<CampaignState> getAllCampaignStates()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
