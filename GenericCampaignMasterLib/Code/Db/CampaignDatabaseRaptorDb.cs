using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;
using RaptorDB;



namespace GenericCampaignMasterLib
{
    class CampaignDatabaseRaptorDb : ICampaignDatabase
    {
        private RaptorDB<string> m_db;
        
        private string m_strDbStorePath;
        public string StorePath
        {
            get { return m_strDbStorePath; }
            set { m_strDbStorePath = value; }
        }

        private string m_strCampaignKey;
        public string CampaignKey
        {
            get { return m_strCampaignKey; }
            set { m_strCampaignKey = value; }
        }

        public void init()
        {
            string dbFilePath = Path.Combine(m_strDbStorePath, m_strCampaignKey);
            m_db = new RaptorDB<string>(dbFilePath, false);
        }

        #region ICampaignDatabase Member

        public string saveGameState(CampaignState state)
        {
            string strState = state.ToString();
            string timeKey = DateTime.Now.ToString("s");
            m_db.Set(timeKey, strState);
            return timeKey;
        }

        public CampaignState getLastGameState()
        {
            return deserializeState(m_db.FetchRecordString((int)m_db.Count() -1));
        }

        public CampaignState getCampaignStateByKey(string key)
        {
            string strState = "";
            m_db.Get(key, out strState);
            CampaignState lastState = deserializeState(strState);
            return lastState;
        }

        public CampaignState getCampaignStateByDate(DateTime time)
        {
            throw new NotImplementedException();
        }

        public List<CampaignState> getAllCampaignStates()
        {
            throw new NotImplementedException();
        }

        public List<DateTime> getStateList()
        {
            throw new NotImplementedException();
        }

        private CampaignState deserializeState(string strEngine)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CampaignState result = serializer.Deserialize<CampaignState>(strEngine);
            return result;
        }


        #endregion
    }
}
