using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;

namespace GcmlDataAccess
{
    
    public class GcmlDataAccessSqlServer : IGcmlDataAccess
    {
        public CampaignController getCampaignController(string campaignId)
        {
            CampaignController result = null;
            using (var ctx = new GcmlDbContext())
            {
                var existing = ctx.CampaignStates.Where(c => c.CampaignId == campaignId).FirstOrDefault();
                if (existing != null)
                {
                    CampaignEngine engine = CampaignEngine.restoreFromState(existing);
                    result = new CampaignController(engine);
                }
            }

            return result;
        }

        public string createNewCampaign(CampaignInfo info)
        {
            string newCampaignId = Guid.NewGuid().ToString();
            info.campaignId = newCampaignId;

            Field field = new Field(info.FieldDimension);
            CampaignEngine engine = new CampaignEngine(field);
            engine.CampaignName = info.campaignName;
            engine.CampaignId = info.campaignId;

            CampaignController controller = new CampaignController(engine);
            CampaignState state = engine.getState();

            using (var ctx = new GcmlDbContext())
            {
                ctx.CampaignStates.Add(state);
                ctx.SaveChanges();
            }

            return newCampaignId;

        }

        public bool safeCampaignState(CampaignState state)
        {
            bool result = false;
            using (var ctx = new GcmlDbContext())
            {
                var existing = ctx.CampaignStates.Find(state.CampaignId);
                if (existing == null)
                {
                    ctx.CampaignStates.Add(state);
                }
                else
                {
                    existing = state;
                }

                ctx.SaveChanges();
                result = true;
            }
            return result;
        }

        public List<PlayerInfo> getPlayers()
        {
            List<PlayerInfo> result =  new List<PlayerInfo>();
            using (var ctx = new GcmlDbContext())
            {
                result = ctx.Players.ToList<PlayerInfo>();
            }

            return result;
        }

        public PlayerInfo getPlayerInfo(string playerId)
        {
            PlayerInfo result = new PlayerInfo();
            using (var ctx = new GcmlDbContext())
            {
                result = ctx.Players.FirstOrDefault(p => p.playerId == playerId);
            }

            return result;
        }

        public List<CampaignInfo> getCampaignsForPlayer(string playerId)
        {
            List<CampaignInfo> result =  new List<CampaignInfo>();
            using(var ctx = new GcmlDbContext())
            {
                PlayerInfo player = ctx.Players.FirstOrDefault(p => p.playerId == playerId);
                if (player != null)
                {
                    var states = from state in ctx.CampaignStates
                                 where state.ListPlayers.Contains(player)
                                 select new CampaignInfo()
                                           {
                                               campaignId = state.CampaignId,
                                               campaignName = state.CampaignName,
                                               players = state.ListPlayers
                                           };
                    result = states.ToList<CampaignInfo>();
                }
            }
            return result;
        }

        public bool safePlayer(PlayerInfo info)
        {
            bool result = false;
            using (var ctx = new GcmlDbContext())
            {
                var existing = ctx.Players.Find(info);
                if (existing == null)
                    ctx.Players.Add(info);
                else
                    existing.playerName = info.playerName;
                ctx.SaveChanges();
                result = true;
            }

            return result;
        }
    }
}
