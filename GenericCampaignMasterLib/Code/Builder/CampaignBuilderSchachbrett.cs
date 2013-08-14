using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterModel;


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
            CampaignEngine engine = CampaignEngine.restoreFromState(state);
            CampaignController controller = new CampaignController(engine);
            controller.CampaignKey = campaignKey;
            controller.CampaignDataBase = database;
			return controller;
        }

        private int m_size = 5;

        public CampaignController buildNew(int intSize)
        {
            m_size = intSize;
            return init();
        }

        public override CampaignController buildNew()
        {
            return init();
		}

        private CampaignController init()
        {
            string campaignkey = Guid.NewGuid().ToString();
            string storepath = Environment.CurrentDirectory;

            CampaignDatabaseRaptorDb database = new CampaignDatabaseRaptorDb();
            database.CampaignKey = campaignkey;
            database.StorePath = storepath;
            database.init();

            CampaignEngine engine = new CampaignEngine(new Field(m_size, m_size));
            engine.FieldField.Id = 123;

            CampaignController controller = new CampaignController();
            controller.CampaignDataBase = database;
            controller.CampaignEngine = engine;
            controller.CampaignKey = campaignkey;
            return controller;
        }
		
	}

}

