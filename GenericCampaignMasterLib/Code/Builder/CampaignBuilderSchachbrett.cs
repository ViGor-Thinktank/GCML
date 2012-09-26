using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GenericCampaignMasterLib
{
	public class CampaignBuilderSchach : CampaignBuilder
	{
		public CampaignBuilderSchach () {}
		
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
            controller.CampaignDataBase = database;
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
            
            CampaignEngine engine = new CampaignEngine(new Field(5, 5));
            engine.FieldField.Id = 123;

			CampaignController controller = new CampaignController();
			controller.CampaignDataBase = database;
			controller.campaignEngine = engine;
			controller.CampaignKey = campaignkey;
			return controller;
		}
		
	}

}

