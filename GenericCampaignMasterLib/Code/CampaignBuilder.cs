using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public abstract class CampaignBuilder
    {
        public CampaignController restoreFromDb(string campaignKey)
        {
            return new CampaignController();
        }


        public CampaignController buildNew()
        {

            return new CampaignController();
        }

    }
}
