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
			
			Field_Schachbrett field = new Field_Schachbrett(new List<int>(){ 3, 3});
			CampaignEngine engine = new CampaignEngine(field);
			
			CampaignController controller = new CampaignController();
			controller.CampaignDataBase = database;
			controller.campaignEngine = engine;
			controller.CampaignKey = campaignkey;
			
			return controller;
		}
		
	}

}

