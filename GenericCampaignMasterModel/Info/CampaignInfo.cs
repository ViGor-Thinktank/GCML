using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel
{
    public class CampaignInfo
    {
        public string campaignId;
        public string campaignName;
        public IEnumerable<PlayerInfo> players;
        public GenericCampaignMasterModel.CampaignState objCampaignData;
    }
}
