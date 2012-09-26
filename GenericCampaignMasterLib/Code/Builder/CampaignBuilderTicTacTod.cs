using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GenericCampaignMasterLib
{
	public class CampaignBuilderTicTacTod : CampaignBuilder
	{
        private CampaignDatabaseRaptorDb m_database;

		public CampaignBuilderTicTacTod () 
        {
            m_database = new CampaignDatabaseRaptorDb();
            m_database.StorePath = Environment.CurrentDirectory;
        }

        private void initDb(string campaignKey)
        {
            m_database.CampaignKey = campaignKey;
            m_database.init();
        }

        public override CampaignController getCurrentGame(string campaignKey)
        {
            initDb(campaignKey);
            CampaignState state = m_database.getLastGameState();
            CampaignEngine engine = state.Restore();
            CampaignController controller = new CampaignController(engine);
            controller.CampaignKey = campaignKey;
            return controller;
        }
		
		public override CampaignController restoreFromDb(string campaignKey, string stateKey)
        {
            initDb(campaignKey);
            CampaignState state = m_database.getCampaignStateByKey(stateKey);
            CampaignEngine engine = state.Restore();
            CampaignController controller = new CampaignController(engine);
            controller.CampaignKey = campaignKey;
            return controller;
        }

        public override CampaignController buildNew()
        {
            string campaignkey = Guid.NewGuid().ToString();
            string storepath = Environment.CurrentDirectory;
			
            CampaignDatabaseRaptorDb database = new CampaignDatabaseRaptorDb();
            database.CampaignKey = campaignkey;
            database.StorePath = storepath;
            database.init();

            Field field = new Field(5, 5);
            //Field_Schlauch field = new Field_Schlauch(new clsSektorKoordinaten(3));
			CampaignEngine engine = new CampaignEngine(field);
            engine.FieldField.Id = 123;

            Player player1 = new Player("1");
            Player player2 = new Player("2");

            clsUnit unit1 = new clsUnit("666", 0);
            clsUnit unit2 = new clsUnit("667", 0);

            engine.addPlayer(player1);
            engine.addPlayer(player2);
            
            engine.addUnit(player1, unit1, field.getSektorList()[0]);
            engine.addUnit(player2, unit2, field.getSektorList()[2]);

			CampaignController controller = new CampaignController();
			controller.CampaignDataBase = database;
			controller.campaignEngine = engine;
			controller.CampaignKey = campaignkey;
			return controller;
		}
		
	}

}

