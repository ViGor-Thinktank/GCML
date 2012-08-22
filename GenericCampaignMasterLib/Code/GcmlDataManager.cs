using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GenericCampaignMasterLib
{
    public sealed class GcmlDataManager
    {
        private Dictionary<string, Player> m_playerDic = new Dictionary<string, Player>();                                          // Spieler in eigener Db vorhalten (pro Spieler N Kampagnen´mögl)
        private Dictionary<string, ICampaignDatabase> m_dictRunningCampaigns = new Dictionary<string, ICampaignDatabase>();         // Dictionary mit allen gefundenen (Db-Dateien im Verzeichnis) Kampagnen
        private Dictionary<string, CampaignController> m_dictLoadedController = new Dictionary<string, CampaignController>();       // aus der Db geladener Controller wird im Speicher vorgehalten. Bei GetController aktuellen State in Db speichern.
        private string strStorepath = "d:\\temp\\";     // jaja ich weiss

        private static readonly GcmlDataManager instance = new GcmlDataManager();

        private GcmlDataManager() { }
        public static GcmlDataManager Instance
        {
            get
            {
                return instance;
            }
        }

        private void init()
        {


        }

        public CampaignController getController(string campaignId)
        {
            CampaignController controller;
            if (m_dictLoadedController.ContainsKey(campaignId))
            {
                controller = m_dictLoadedController[campaignId];
            }
            else
            {
                ICampaignDatabase db = getCampaign(campaignId);
                CampaignState state = db.getLastGameState();
                CampaignEngine engine = state.Restore();
                controller = new CampaignController(engine);
                controller.CampaignKey = campaignId;
                m_dictLoadedController[campaignId] = controller;
            }

            return controller;
        }

        public string getPlayerId(string playername)
        {
            string playerId = "";
            var players = from p in m_playerDic.Values
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
                m_playerDic.Add(pnew.Id, pnew);
            }

            return playerId;
        }

        public Player getPlayer(string playerId)
        {
            if (m_playerDic.ContainsKey(playerId))
                return m_playerDic[playerId];
            else
                return null;
        }

        public Dictionary<string, Player> getPlayerList()
        {
            return m_playerDic;
        }

        public ICampaignDatabase getCampaign(string id)
        {
            return m_dictRunningCampaigns[id];
        }

        public Dictionary<string, ICampaignDatabase> getRunningCampaigns()
        {
            return m_dictRunningCampaigns;
        }

        public string createNewCampaign(string playerid, string fielddimension)
        {
            Player player = getPlayer(playerid);
            if (player == null)
                return "";

            // Datenbank
            CampaignDatabaseRaptorDb database = new CampaignDatabaseRaptorDb();
            database.CampaignKey = Guid.NewGuid().ToString();
            database.StorePath = strStorepath;
            database.init();

            // Spielfeld
            Field field = new Field(3, 3);

            // Engine
            CampaignEngine engine = new CampaignEngine(field);
            engine.FieldField.Id = 123;
            engine.addPlayer(player);
            engine.addUnit(player, new DummyUnit(new Random().Next(1000, 9999).ToString()), field.getSektorList()[0]);

            CampaignController controller = new CampaignController();
            controller.CampaignDataBase = database;
            controller.campaignEngine = engine;
            controller.CampaignKey = database.CampaignKey;

            database.saveGameState(engine.getState());
            m_dictRunningCampaigns.Add(database.CampaignKey, database);

            return database.CampaignKey;
        }
    }
}
