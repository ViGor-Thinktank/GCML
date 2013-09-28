using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;


namespace GcmlDataAccess
{
    public class PlayerDbRaptor : IPlayerDatabase
    {
        public Player getPlayer(string id)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, Player> getAllPlayers()
        {
            throw new NotImplementedException();
        }

        public Player getPlayerByName(string playername)
        {
            throw new NotImplementedException();
        }
    }
}
