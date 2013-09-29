using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;
using RaptorDB;

namespace GcmlDataAccess
{
    public class PlayerDbRaptor : IPlayerDatabase
    {
        RaptorDB<string> playerDb;

        public PlayerDbRaptor()
        {
            string dbpath = Path.Combine(Properties.Settings.Default.DbStorepath, "PLAYERDB");
            playerDb = RaptorDB<string>.Open(dbpath, false);
        }

        ~PlayerDbRaptor()
        {
            playerDb.Shutdown();
        }


        public PlayerInfo getPlayer(string id)
        {
            PlayerInfo result = null;
            string pnamestr;
            if (playerDb.Get(id, out pnamestr))
                result = new PlayerInfo() { playerId = id, playerName = pnamestr };

            return result;
        }

        public Dictionary<string, PlayerInfo> getAllPlayers()
        {
            throw new NotImplementedException();
        }

        public PlayerInfo getPlayerByName(string playername)
        {
            if(String.IsNullOrEmpty(playername))
                return null;

            PlayerInfo result = null;
            System.Text.UTF8Encoding  enc = new UTF8Encoding();

            foreach (var kv in playerDb.EnumerateStorageFile())
            {
                byte[] key = kv.Key;
                string keyStr = enc.GetString(key);

                byte[] val = kv.Data;
                string valStr = enc.GetString(val);

                if (valStr == playername)
                {
                    result = new PlayerInfo() { playerId = keyStr, playerName = valStr };
                    break;
                }
            }

            // Playername nicht gefunden - neuen anlegen
            if (result == null)
            {
                string id = Guid.NewGuid().ToString();
                playerDb.Set(id, playername);
                result = new PlayerInfo() { playerId = id, playerName = playername };
            }

            return result;
        }
    }
}
