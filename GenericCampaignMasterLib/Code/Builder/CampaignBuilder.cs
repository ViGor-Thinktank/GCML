using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public abstract class CampaignBuilder
    {		
		public virtual CampaignController restoreFromDb(string campaignKey)
        {
            return new CampaignController();
        }

        public virtual CampaignController buildNew()
        {
            return new CampaignController();
        }

    }
}
