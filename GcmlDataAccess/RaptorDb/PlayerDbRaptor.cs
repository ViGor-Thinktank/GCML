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
    public class PlayerDbRaptor
    {
        RaptorDB<string> playerDb;

        public PlayerDbRaptor()
        {
            string dbpath = Path.Combine(Properties.Settings.Default.DbStorepath, "PLAYERDB");
            playerDb = RaptorDB<string>.Open(dbpath, false);
        }

        public PlayerInfo getPlayer(string id)
        {
            PlayerInfo result = null;
            string pnamestr;
            if (playerDb.Get(id, out pnamestr))
                result = new PlayerInfo() { playerId = id, playerName = pnamestr };

            return result;
        }

        public List<PlayerInfo> getAllPlayers()
        {
            List<PlayerInfo> result = new List<PlayerInfo>();
            System.Text.UTF8Encoding  enc = new UTF8Encoding();
            System.Text.UnicodeEncoding enc2 = new UnicodeEncoding();

            foreach (var kv in playerDb.EnumerateStorageFile())
            {
                byte[] key = kv.Key;
                string keyStr = enc.GetString(key);

                byte[] val = kv.Data;
                string valStr = enc2.GetString(val);

                var np = new PlayerInfo() { playerId = keyStr, playerName = valStr };
                result.Add(np);
            }

            return result;
        }


        public PlayerInfo getPlayerByName(string playername)
        {
            if(String.IsNullOrEmpty(playername))
                return null;

            PlayerInfo result = getAllPlayers().Where(p => p.playerName== playername).FirstOrDefault();

            // Playername nicht gefunden - neuen anlegen
            if (result == null)
            {
                var pinfo = new PlayerInfo() { playerName = playername };
                string newid = addNewPlayer(pinfo);
                result = getPlayer(newid);
            }

            return result;
        }

        public string addPlayer(PlayerInfo pinfo)
        {
            return addNewPlayer(pinfo);
        }

        public void close()
        {
            playerDb.Shutdown();
        }

        private string addNewPlayer(PlayerInfo pinfo)
        {
            if(String.IsNullOrEmpty(pinfo.playerName))
                throw new Exception("Kein Name für Spieler");
            string pid = Guid.NewGuid().ToString();
            playerDb.Set(pid, pinfo.playerName);

            return pid;
        }
    }
}
