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
            GcmlDataAccessSQL da = new GcmlDataAccessSQL();

            CampaignInfo cinfo = new CampaignInfo()
            {
                campaignId = "",
                campaignName = "test",
                FieldDimension = new clsSektorKoordinaten(6, 6, 0),
                ListPlayerInfo = new List<PlayerInfo>(){ new PlayerInfo(){playerName = "asdf"} }
            };


            string campaignid = da.createNewCampaign(cinfo);


            //GcmlDataAccessEF data = new GcmlDataAccessEF();

            //PlayerInfo pinfo = data.getPlayer("test123");




            //data.createNewCampaign(cinfo);

            //GcmlDataAccess.GcmlDataAccessEF accessSqlServer = new GcmlDataAccess.GcmlDataAccessEF();
            //accessSqlServer.safeCampaignState(state);
            //state.ListPlayers.Add(new PlayerInfo() { playerId = "1234", playerName = "asdf" });

            //accessSqlServer.safeCampaignState(state);

        }
    }
}
