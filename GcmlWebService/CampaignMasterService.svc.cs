using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using GenericCampaignMasterLib;
using System.Web.Script.Serialization;

namespace GcmlWebService
{
    // HINWEIS: Mit dem Befehl "Umbenennen" im Menü "Umgestalten" können Sie den Klassennamen "Service1" sowohl im Code als auch in der SVC- und der Konfigurationsdatei ändern.
    public class CampaignMasterService : ICampaignMasterService
    {
        private JavaScriptSerializer m_serializer = new JavaScriptSerializer();
        private Dictionary<string, Player> m_playerDic = new Dictionary<string, Player>();
        private Dictionary<string, ICampaignDatabase> m_dictRunningCampaigns = new Dictionary<string, ICampaignDatabase>();
        private string strStorepath = Environment.CurrentDirectory;

        #region ICampaignMasterService Member

        public string getPlayerId(string playername)
        {
            string playerId = "";

            var players = from cmp in m_dictRunningCampaigns.Values
                          from p in cmp.getPlayerList()
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

        public List<string> getPlayerCampaigns(string playerid)
        {
            var campaigns = from cmp in m_dictRunningCampaigns.Values
                            from p in cmp.getPlayerList()
                            where p.Id == playerid
                            select cmp.CampaignKey;
            return campaigns.ToList<string>();
        }

        public clsSektorKoordinaten getFieldKoord(string campaignid)
        {
            CampaignController controller = getController(campaignid);
            clsSektorKoordinaten fieldKoord = controller.campaignEngine.FieldField.FieldDimension;

            return fieldKoord;
        }

        public Sektor getSektor(string campaignid, string sektorkoord)
        { 
            //string resultSektor = "";
            clsSektorKoordinaten koord = (clsSektorKoordinaten)m_serializer.Deserialize<clsSektorKoordinaten>(sektorkoord);
            CampaignController controller = getController(campaignid);
            Sektor sektor = controller.campaignEngine.FieldField.get(koord);

            return sektor;
        }

        public List<string> getSektorList(string campaignid)
        {
            throw new NotImplementedException();
        }

        public BaseUnit getUnit(string campaignid, string unitid)
        {
            throw new NotImplementedException();
        }

        public List<Sektor> getUnitCollisions(string campaignid)
        {
            List<string> lstStrSektorClollisions = new List<string>();
            CampaignController controller = getController(campaignid);
            return controller.getUnitCollisions();
        }

        public List<CommandInfo> getCommandsForUnit(string campaignid, string unitid)
        {
            throw new NotImplementedException();
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
            Field field = new Field( 5, 5 );
            
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

        public void addPlayerToCampaign(string playerid, string campaignid)
        {
            throw new NotImplementedException();
        }

        public void executeCommand(string campaignid, CommandInfo command)
        {
            throw new NotImplementedException();
        }

        public void addUnitToField(string campaignid, string unit, string targetsektor)
        {
            throw new NotImplementedException();
        }

        #endregion


        

        private CampaignController getController(string campaignId)
        {
            ICampaignDatabase db = getCampaign(campaignId);
            CampaignState state = db.getLastGameState();
            CampaignEngine engine = state.Restore();
            CampaignController controller = new CampaignController(engine);
            controller.CampaignKey = campaignId;
            return controller;
        }

        private Player getPlayer(string playerId)
        {
            Player player = null;
            var players = from cmp in m_dictRunningCampaigns.Values
                          from p in cmp.getPlayerList()
                          where p.Id == playerId
                          select p;

            if (players.Count() > 0)
                player = players.First();
            else
                player = m_playerDic[playerId];

            return player;
        }


        private Dictionary<string, Player> getPlayerList()
        {
            return new Dictionary<string, Player>();
        }

        private ICampaignDatabase getCampaign(string id)
        {
            return null; 

        }
        private Dictionary<string, ICampaignDatabase> getRunningCampaigns()
        {
            return new Dictionary<string, ICampaignDatabase>();

        }



    }

}
