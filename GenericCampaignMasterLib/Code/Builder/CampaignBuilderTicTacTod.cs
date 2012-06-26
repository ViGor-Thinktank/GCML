using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GenericCampaignMasterLib
{
	public class CampaignBuilderTicTacTod : CampaignBuilder
	{
		public CampaignBuilderTicTacTod () {}
		
		public override CampaignController restoreFromDb(string campaignKey, string stateKey)
        {
            CampaignDatabaseRaptorDb database = new CampaignDatabaseRaptorDb();
            database.CampaignKey = campaignKey;
            database.StorePath = Environment.CurrentDirectory;
            database.init();

            CampaignState state = database.getCampaignStateByKey(stateKey);
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

			//Field_Schachbrett field = new Field_Schachbrett(new List<int>(){ 3, 3});
            Field_Schlauch field = new Field_Schlauch(new List<int> { 3 });
			CampaignEngine engine = new CampaignEngine(field);
            engine.FieldField.Id = 123;

            Player player1 = new Player("1");
            Player player2 = new Player("2");

            DummyUnit unit1 = new DummyUnit(666);
            DummyUnit unit2 = new DummyUnit(667);

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

