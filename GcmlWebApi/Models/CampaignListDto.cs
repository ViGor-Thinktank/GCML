using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GenericCampaignMasterModel;

namespace GcmlWebApi
{
    public class CampaignListDto
    {
        public List<CampaignInfo> campaignList { get; set; }
    }
}