using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;
using GcmlDataAccess;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CampaignController ctrl = CampaignBuilder.Instance.buildNew();
            CampaignState state = ctrl.CampaignEngine.getState();

            //using (GcmlDbContext ctx = new GcmlDbContext())
            //{
            //    //ctx.CampaignStates.Add(state);
            //    //

            //    //ctx.
            //    //ctx.SaveChanges();

            //}

            GcmlDataAccess.GcmlDataAccessSqlServer accessSqlServer = new GcmlDataAccess.GcmlDataAccessSqlServer();
            accessSqlServer.safeCampaignState(state);
            state.ListPlayers.Add(new PlayerInfo() { playerId = "1234", playerName = "asdf" });

            accessSqlServer.safeCampaignState(state);

        }
    }
}
