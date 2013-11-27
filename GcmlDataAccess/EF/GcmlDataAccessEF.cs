using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;

namespace GcmlDataAccess
{
    //public class GcmlDataAccessEF : IGcmlDataAccess
    public class GcmlDataAccessEF
    {
        public GcmlDataAccessEF()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<GcmlDbContext>());
        }

        public CampaignController getCampaignController(string campaignId)
        {
            CampaignController result = null;
            using (var ctx = new GcmlDbContext())
            {
                var existing = ctx.CampaignStates.FirstOrDefault(c => c.CampaignId == campaignId);
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
            CampaignController ctrl = CampaignEngine.createNewCampaign(info);

            //using (var ctx = new GcmlDbContext())
            //{
            //    foreach (PlayerInfo nfo in ctrl.ListPlayers)
            //    {
            //        //ctx.Players.Attach(nfo);
            //        //nfo.Campaigns = new List<CampaignState>();
            //        //nfo.Campaigns.Add(state);
            //        //ctx.Players.AddOrUpdate(nfo);
            //        PlayerInfo p = getPlayer(nfo.playerName);
            //        p.Campaigns.Add(ctrl);
            //        ctx.Entry(nfo).State = EntityState.Modified;
            //        ctx.SaveChanges();

            //    }

            //    ctx.CampaignStates.Add(ctrl);
            //    ctx.SaveChanges();

            //    CampaignState s = ctx.CampaignStates.FirstOrDefault(c => c.CampaignId == ctrl.CampaignId);
            //}

            return ctrl.CampaignEngine.CampaignId;

        }

        public bool safeCampaignState(CampaignController controller)
        {
            bool result = false;
            CampaignState state = controller.CampaignEngine.getState();

            using (var ctx = new GcmlDbContext())
            {
                //var existing = ctx.CampaignStates.Find(state.CampaignId);
                //if (existing == null)
                //{
                //    ctx.CampaignStates.Add(state);
                //}
                //else
                //{
                //    existing = state;
                //}

                ctx.CampaignStates.AddOrUpdate(state);
                ctx.SaveChanges();
                result = true;
            }
            return result;
        }

        public PlayerInfo getPlayer(string playername)
        {
            PlayerInfo result = null;
            using (var ctx = new GcmlDbContext())
            {
                PlayerInfo player = (from p in ctx.Players
                    where p.playerName == playername
                    select p).FirstOrDefault();

                if (player == null)
                {
                    player = new PlayerInfo() { playerName = playername };
                    ctx.Players.Add(player);
                    ctx.SaveChanges();
                }

                result = player;
            }

            return result;
        }


        public List<PlayerInfo> getPlayers()
        {
            List<PlayerInfo> result =  new List<PlayerInfo>();
            //using (var ctx = new GcmlDbContext())
            //{
            //    result = ctx.Players.ToList();
            //}

            return result;
        }

        public List<CampaignInfo> getCampaignsForPlayer(string playername)
        {
            List<CampaignInfo> result =  new List<CampaignInfo>();
            using(var ctx = new GcmlDbContext())
            {
                PlayerInfo player = ctx.Players.FirstOrDefault(p => p.playerName == playername);
                if (player != null)
                {
                    var states = from state in ctx.CampaignStates
                                 where state.ListPlayers.Contains(player)
                                 select new CampaignInfo()
                                           {
                                               campaignId = state.CampaignId,
                                               campaignName = state.CampaignName,
                                               ListPlayerInfo = state.ListPlayers
                                           };
                    result = states.ToList<CampaignInfo>();
                }
            }
            return result;
        }
     }
}
