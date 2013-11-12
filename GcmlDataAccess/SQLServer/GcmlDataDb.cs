using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;

namespace GcmlDataAccess
{
    public class GcmlDataDb
    {
        public CampaignState getCampaignState(string campaignId)
        {
            return CampaignState.NewInstance();
        }

        //public CampaignState newCampaign(CampaignInfo info)
        //{
        //    return CampaignState.NewInstance();
        //}

        public bool safeCampaignState(CampaignState state)
        {
            bool result = false;
            using (var gcmlcontext = new GcmlDbContext())
            {
                gcmlcontext.CampaignStates.Add(state);
                gcmlcontext.SaveChanges();
                result = true;
            }
            return result;
        }

        public List<PlayerInfo> getPlayers()
        {
            List<PlayerInfo> result =  new List<PlayerInfo>();

            return result;
        }

        public PlayerInfo getPlayerInfo(string playerId)
        {
            return new PlayerInfo();
        }

        public List<CampaignInfo> getCampaignsForPlayer(string playerId)
        {
            return new List<CampaignInfo>();
        }

        public bool safePlayer(PlayerInfo info)
        {
            return true;
        }
    }
}
