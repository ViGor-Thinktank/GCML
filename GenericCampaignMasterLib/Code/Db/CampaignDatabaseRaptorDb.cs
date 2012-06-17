using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using RaptorDB;

namespace GenericCampaignMasterLib
{
    class CampaignDatabaseRaptorDb : ICampaignDatabase
    {
        private string m_strDbStorePath;
        private RaptorDB<string> m_db;

        public CampaignDatabaseRaptorDb(string storePath)
        {
            m_strDbStorePath = storePath;
        }

        #region ICampaignDatabase Member

        public string initDatabase()
        {
			
            string campaignKey = Guid.NewGuid().ToString();
			string dbFilePath = string.IsNullOrEmpty(m_strDbStorePath) ? Path.Combine (Environment.CurrentDirectory, campaignKey) : Path.Combine(m_strDbStorePath, campaignKey);
            m_db = new RaptorDB<string>(dbFilePath, false);
            return campaignKey;
        }

        public void initDatabase(string campaignIdentifier)
        {
            string campaignKey = new Guid().ToString();
            m_db = new RaptorDB<string>(Path.Combine(m_strDbStorePath, campaignKey), false);
        }

        public string saveGameState(CampaignState state)
        {
            string strState = state.ToString();
            string timeKey = DateTime.Now.ToString();
            m_db.Set(timeKey, strState);
            return strState;
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
