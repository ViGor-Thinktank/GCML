using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;

namespace GcmlDataAccess
{
    
    // Todo: Umbenennen - ist die eigentliche DataAccess-Klasse.
    public class GcmlDataDb : IGcmlDataDb
    {
        public CampaignState getCampaignState(string campaignId)
        {
            return CampaignState.NewInstance();
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
                result = ctx.Players.Where(p => p.playerId == playerId).FirstOrDefault();
            }

            return result;
        }

        public List<CampaignInfo> getCampaignsForPlayer(string playerId)
        {
            List<CampaignInfo> result =  new List<CampaignInfo>();
            using(var ctx = new GcmlDbContext())
            {
                PlayerInfo player = ctx.Players.Where(p => p.playerId == playerId).FirstOrDefault();
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
