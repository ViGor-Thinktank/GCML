using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Resources;

namespace GenericCampaignMasterModel
{
    public class CampaignInfo
    {
        public string campaignId;

        [Display(Name="Name")]
        public string campaignName;

        public IEnumerable<PlayerInfo> players;
        public GenericCampaignMasterModel.CampaignState objCampaignData;
    }
}
