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
    public sealed class GcmlDataManager
    {
        private Dictionary<string, Player> m_playerDic = new Dictionary<string, Player>();
        private Dictionary<string, ICampaignDatabase> m_dictRunningCampaigns = new Dictionary<string, ICampaignDatabase>();
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

        public CampaignController getController(string campaignId)
        {
            ICampaignDatabase db = getCampaign(campaignId);
            CampaignState state = db.getLastGameState();
            CampaignEngine engine = state.Restore();
            CampaignController controller = new CampaignController(engine);
            controller.CampaignKey = campaignId;
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
            if(m_playerDic.ContainsKey(playerId))
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
            Field field = new Field(5, 5);

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

    public class CampaignMasterService : ICampaignMasterService
    {
        private JavaScriptSerializer m_serializer = new JavaScriptSerializer();
       
        #region ICampaignMasterService Member

        public string getPlayerId(string playername)
        {
            return GcmlDataManager.Instance.getPlayerId(playername);   
        }

        public List<string> getPlayerCampaigns(string playerid)
        {
            Dictionary<string, ICampaignDatabase> campaigns = GcmlDataManager.Instance.getRunningCampaigns();

            var plCampaigns =   from cmp in campaigns.Values
                                from p in cmp.getPlayerList()
                                where p.Id == playerid
                                select cmp.CampaignKey;
            return plCampaigns.ToList<string>();
        }

        public clsSektorKoordinaten getFieldKoord(string campaignid)
        {
            CampaignController controller = GcmlDataManager.Instance.getController(campaignid);
            clsSektorKoordinaten fieldKoord = controller.campaignEngine.FieldField.FieldDimension;
            return fieldKoord;
        }

        public SektorInfo getSektor(string campaignid, clsSektorKoordinaten sektorkoord)
        {
            CampaignController controller = GcmlDataManager.Instance.getController(campaignid);
            Sektor sektor = controller.campaignEngine.FieldField.get(sektorkoord);
            return sektor.getInfo();
        }

        public List<string> getSektorList(string campaignid)
        {
            throw new NotImplementedException();
        }

        public BaseUnit getUnit(string campaignid, string unitid)
        {
            throw new NotImplementedException();
        }

        public List<SektorInfo> getUnitCollisions(string campaignid)
        {
            List<string> lstStrSektorClollisions = new List<string>();
            CampaignController controller = GcmlDataManager.Instance.getController(campaignid);
            List<Sektor> lstsek = controller.getUnitCollisions();

            List <SektorInfo> collInfo = (from s in lstsek
                                         select s.getInfo()).ToList<SektorInfo>();
            return collInfo;
        }

        public List<CommandInfo> getCommandsForUnit(string campaignid, string unitid)
        {
            throw new NotImplementedException();
        }

        public string createNewCampaign(string playerid, string fielddimension)
        {
            return GcmlDataManager.Instance.createNewCampaign(playerid, fielddimension);
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
    }

}
