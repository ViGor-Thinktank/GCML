using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;
using RaptorDB;

namespace GcmlDataAccess
{
    public class CampaignDbRaptor : ICampaignDatabase
    {
        RaptorDB<string> campaignDb;

        public CampaignDbRaptor()
        {
            string dbpath = Path.Combine(Properties.Settings.Default.DbStorepath, "CAMPAIGNDB");
            campaignDb = RaptorDB<string>.Open(dbpath, false);
        }

        ~CampaignDbRaptor()
        {
            campaignDb.Shutdown();
        }

        public List<string> getAllCampaignKeys()
        {
            List<string> result = new List<string>();
            foreach (var v in campaignDb.EnumerateStorageFile())
            {
                byte[] key = v.Key;

                string str = Convert.ToString(key);
                result.Add(str);

            }

            return result;
        }

        public CampaignState getCampaignStateForCampaign(string campaignKey)
        {
            CampaignState result = null;
            string str;
            if (campaignDb.Get(campaignKey, out str))
                result = CampaignState.FromString(str);

            return result;
        }

        public List<CampaignInfo> getCampaignsForPlayer(string p)
        {
            throw new NotImplementedException();
        }

        public string createNewCampaign(string campaignname, clsSektorKoordinaten fielddim)
        {
            string newCampaignId = Guid.NewGuid().ToString();

            Field field = new Field(fielddim);
            CampaignEngine engine = new CampaignEngine(field);
            CampaignController controller = new CampaignController(engine);
            controller.CampaignKey = newCampaignId;
            controller.CampaignEngine = engine;

            CampaignState state = engine.getState();
            campaignDb.Set(newCampaignId, state.ToString());

            return newCampaignId;
        }

        public string CampaignKey
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string saveGameState(CampaignState state)
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

        public List<DateTime> getStateList()
        {
            throw new NotImplementedException();
        }

        public void close()
        {
            throw new NotImplementedException();
        }
    }
}
