using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.IO;
using RaptorDB;

/// TEST ASDF
namespace GenericCampaignMasterLib
{
    public sealed class GcmlDataManager
    {
        private string STOREPATH = "c:\\temp\\";     // jaja ich weiss
        private string PLAYER_DB = "GCML_PLAYERS";
        private string CAMPAIGN_DB = "GCML_CAMPAIGNS";

        private RaptorDB<string> m_playerDb;
        private RaptorDB<string> m_campaignDb;
        private Dictionary<string, ICampaignDatabase> m_dictRunningCampaigns = new Dictionary<string, ICampaignDatabase>();         // Dictionary mit allen gefundenen (Db-Dateien im Verzeichnis) Kampagnen
        private Dictionary<string, CampaignController> m_dictLoadedController = new Dictionary<string, CampaignController>();       // aus der Db geladener Controller wird im Speicher vorgehalten. Bei GetController aktuellen State in Db speichern.

        private static GcmlDataManager instance = null;

        private GcmlDataManager() { }

        public static GcmlDataManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GcmlDataManager();
                    instance.init();
                }

                return instance;
            }
        }

        private void init()
        {
            string playerdbfile = Path.Combine(STOREPATH, PLAYER_DB);
            string campaigndbfile = Path.Combine(STOREPATH, CAMPAIGN_DB);
            
            m_playerDb = RaptorDB<string>.Open(playerdbfile, false);
            m_campaignDb = RaptorDB<string>.Open(campaigndbfile, true);

            if (m_campaignDb.Count() > 0)
            {
                for (int i = 0; i < m_campaignDb.Count(); i++)
                {
                    string campaignStr = m_campaignDb.FetchRecordString(i);
                    string campaignKey = campaignStr.Split('#')[0];
                    string dbfilepath = campaignStr.Split('#')[1];

                    CampaignDatabaseRaptorDb db = new CampaignDatabaseRaptorDb();
                    db.CampaignKey = campaignKey;
                    db.StorePath = dbfilepath;
                    db.init();

                    m_dictRunningCampaigns.Add(campaignKey, (ICampaignDatabase)db);
                }
            }
        }

        public List<string> getRunningCampaignIds()
        {
            return m_dictRunningCampaigns.Keys.ToList<string>();
        }


        public CampaignController getController(string campaignId)
        {
            CampaignController controller;
            if ((!String.IsNullOrEmpty(campaignId)) &&
                (m_dictLoadedController.ContainsKey(campaignId)))
            {
                controller = m_dictLoadedController[campaignId];
            }
            else
            {
                ICampaignDatabase db = m_dictRunningCampaigns[campaignId];
                CampaignState state = db.getLastGameState();
                CampaignEngine engine = state.Restore();
                controller = new CampaignController(engine);
                controller.CampaignKey = campaignId;
                controller.CampaignDataBase = db;
                m_dictLoadedController[campaignId] = controller;
            }

            return controller;
        }

        public string getPlayerId(string playername)
        {
            string playerId = "";
            var players = from p in getPlayerList().Values
                          where p.Playername == playername
                          select p;

            if (players.Count() > 0)
            {
                playerId = players.First().Id;
            }
            else
            {
                playerId = Guid.NewGuid().ToString();
                Player pnew = new Player();
                pnew.Playername = playername;
                pnew.Id = playerId;

                m_playerDb.Set(playerId, pnew.ToString());
            }

            return playerId;
        }

        public Player getPlayer(string playerId)
        {
            string strPlayer;
            m_playerDb.Get(playerId, out strPlayer);
            Player player = Player.FromString(strPlayer);

            return player;
        }

        public Dictionary<string, Player> getPlayerList()
        {
            Dictionary <string, Player> result = new Dictionary<string,Player>();
            for (int i = 0; i < m_playerDb.Count(); i++)
            {
                string pstr = m_playerDb.FetchRecordString(i);
                Player p = Player.FromString(pstr);
                result.Add(p.Id, p);
            }

            return result;
        }


        public List<string> getRunningPlayerCampaigns(string playerid)
        {
            List<string> campaignIds = m_dictRunningCampaigns.Keys.ToList<string>();
            var campaigns = from id in campaignIds
                            from pl in getController(id).getPlayerList()
                            where pl.Id == playerid
                            select id;
            return campaigns.ToList<string>();
        }

        public string createNewCampaign(string playerid, string fielddimension)
        {
            Player player = getPlayer(playerid);
            if (player == null)
                return "";

            // Datenbank
            CampaignDatabaseRaptorDb database = (CampaignDatabaseRaptorDb)getCampaignDbOrNew("");
            
            // Spielfeld
            Field field = new Field(3, 3);

            // Engine
            CampaignEngine engine = new CampaignEngine(field);
            engine.FieldField.Id = 123;
            engine.addPlayer(player);
            engine.addUnit(player, new clsUnit(new Random().Next(1000, 9999).ToString(), 0), field.getSektorList()[0]);

            CampaignController controller = new CampaignController();
            controller.CampaignDataBase = database;
            controller.campaignEngine = engine;
            controller.CampaignKey = database.CampaignKey;
            controller.saveCurrentGameState();

            m_dictLoadedController[controller.CampaignKey] = controller;
            return database.CampaignKey;
        }

        public string createNewCampaign(string playerid, string campaignname, clsSektorKoordinaten fielddim, int anzUnitsPerPlayer)
        {
            Player player = getPlayer(playerid);
            if (player == null)
                return "";

            // Datenbank
            CampaignDatabaseRaptorDb database = (CampaignDatabaseRaptorDb)getCampaignDbOrNew("");

            // Spielfeld
            Field field = new Field(fielddim);

            // Engine
            CampaignEngine engine = new CampaignEngine(field);
            engine.CampaignName = campaignname;
            engine.FieldField.Id = 123;
            engine.addPlayer(player);

            //for(int i = anzUnitsPerPlayer; i > 0; i--)
            //{
            //    engine.addUnit(player, new clsUnit(new Random().Next(1000, 9999).ToString(), 0), field.getSektorList()[0]);
            //}

            CampaignController controller = new CampaignController();
            controller.CampaignDataBase = database;
            controller.campaignEngine = engine;
            controller.CampaignKey = database.CampaignKey;
            controller.saveCurrentGameState();

            m_dictLoadedController[controller.CampaignKey] = controller;
            return database.CampaignKey;
        }


        private CampaignDatabaseRaptorDb getCampaignDbOrNew(string campaignId)
        {
            CampaignDatabaseRaptorDb result = null;
            if (!m_dictRunningCampaigns.ContainsKey(campaignId)  || String.IsNullOrEmpty(campaignId))
           { 
                result = new CampaignDatabaseRaptorDb();
                result.CampaignKey = Guid.NewGuid().ToString();
                result.StorePath = STOREPATH;
                result.init();

                m_dictRunningCampaigns.Add(result.CampaignKey, result);
                m_campaignDb.Set(result.CampaignKey, result.CampaignKey + "#" + STOREPATH);
                m_campaignDb.SaveIndex();
            }
            else
            {
                result = (CampaignDatabaseRaptorDb)m_dictRunningCampaigns[campaignId];
            }
            
            return result;
        }

        ~GcmlDataManager()
        {
            //foreach (ICampaignDatabase cmp in m_dictRunningCampaigns.Values)
            //    cmp.close();
            //m_campaignDb.Shutdown();
            //m_playerDb.Shutdown();
        }


    }
}
