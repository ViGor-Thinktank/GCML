using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterModel;

namespace GenericCampaignMasterLib
{
    public interface IPlayerDatabase
    {
        PlayerInfo getPlayer(string id);
        List<PlayerInfo> getAllPlayers();
        PlayerInfo getPlayerByName(string playername);
        void close();

        string addPlayer(PlayerInfo player);
    }
}
