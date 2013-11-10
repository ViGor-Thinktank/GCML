using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;
using Ninject;


namespace GcmlDataAccess
{
    public class CampaignBuilder
    {
        IGcmlDataManager datamanager;
        public IGcmlDataManager DataManager 
        {
            get { return datamanager; }
        }

        private static CampaignBuilder instance = null;
        public static CampaignBuilder Instance
        {
            get
            {
                if (instance == null)
                    instance = new CampaignBuilder();

                return instance;
            }
        }
        
        private CampaignBuilder() 
        {
            var kernel = new StandardKernel();
            kernel.Bind<ICampaignDatabase>().To<CampaignDbRaptor>();
            kernel.Bind<IPlayerDatabase>().To<PlayerDbRaptor>();
            kernel.Bind<IGcmlDataManager>().To<GcmlDataManager>();

            datamanager = kernel.Get<IGcmlDataManager>();
        }

        //public CampaignController getCurrentGame(string campaignKey)
        //{
        //    return datamanager.getController(campaignKey);
        //}

        //public CampaignController restoreFromDb(string campaignKey, string stateKey)
        //{
        //    return datamanager.getController(campaignKey);
        //}

        public CampaignController buildNew()
        {
            clsSektorKoordinaten koord = new clsSektorKoordinaten(7, 7);
            string newCampaignId = datamanager.createNewCampaign("testkampagne", koord);
            CampaignController ctr = datamanager.getController(newCampaignId);

            return ctr;
        }

    }
}
