using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;
using RaptorDB;
using NLog;

namespace GcmlDataAccess
{
    public class CampaignDbRaptor : ICampaignDatabase
    {
        RaptorDB<string> campaignDb;
        NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

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
            {
                try
                {
                    result = CampaignState.FromString(str);
                }
                catch (Exception e)
                {
                    log.ErrorException("Fehler beim Deserialisieren von State " + campaignKey, e);
                    campaignDb.RemoveKey(campaignKey);
                    log.Warn("State " + campaignKey + " gelöscht. Daten: " + str);
                }
            }

            return result;
        }

        public List<CampaignInfo> getCampaignsForPlayer(string playerId)
        {
            List<CampaignInfo> result = new List<CampaignInfo>();
            foreach (var c in campaignDb.EnumerateStorageFile())
            {
                string id = System.Text.Encoding.Default.GetString(c.Key);
                CampaignState state = getCampaignStateForCampaign(id);

                if((state != null) && (state.getListPlayers().Find(p => p.Id == playerId) != null))
                {
                    var info = new CampaignInfo(){ campaignId = state.CampaignId, campaignName = state.CampaignName };
                    result.Add(info);
                 }
            }
            
            return result;
        }

        public string createNewCampaign(string campaignname, clsSektorKoordinaten fielddim)
        {
            string newCampaignId = Guid.NewGuid().ToString();

            Field field = new Field(fielddim);
            CampaignEngine engine = new CampaignEngine(field);
            engine.CampaignName = campaignname;
            engine.CampaignId = newCampaignId;
            
            CampaignController controller = new CampaignController(engine);
            controller.CampaignKey = newCampaignId;

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
            campaignDb.Set(state.CampaignId, state.ToString());
            return state.CampaignId;
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
