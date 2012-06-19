using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GenericCampaignMasterLib
{
	public class CampaignBuilderTicTacTod : CampaignBuilder
	{
		public CampaignBuilderTicTacTod ()
		{
			
		}
		
		public override CampaignController restoreFromDb(string campaignKey)
        {
            return new CampaignController();
        }

        public override CampaignController buildNew()
        {
			CampaignDatabaseRaptorDb database = new CampaignDatabaseRaptorDb("");
			string campaignkey = database.initDatabase();


			//Field_Schachbrett field = new Field_Schachbrett(new List<int>(){ 3, 3});
            Field_Schlauch field = new Field_Schlauch(new List<int> { 3 });
			CampaignEngine engine = new CampaignEngine(field);
            engine.FieldField.Id = 123;

            Player player1 = new Player(1);
            Player player2 = new Player(2);

            DummyUnit unit1 = new DummyUnit(666);
            DummyUnit unit2 = new DummyUnit(667);

            player1.ListUnits.Add(unit1);
            player2.ListUnits.Add(unit2);

            engine.addPlayer(player1);
            engine.addPlayer(player2);

            //sektor1.addUnit(unit1);
            //sektor2.addUnit(unit2);

			CampaignController controller = new CampaignController();
			controller.CampaignDataBase = database;
			controller.campaignEngine = engine;
			controller.CampaignKey = campaignkey;
			return controller;
		}
		
	}

}

