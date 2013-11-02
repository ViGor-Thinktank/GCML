﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Resources;

namespace GenericCampaignMasterModel
{
    public class CampaignInfo
    {
        public string campaignId { get; set; }
        public string campaignName { get; set; }
        public IEnumerable<PlayerInfo> players { get; set; }
        public GenericCampaignMasterModel.CampaignState objCampaignData;
    }
}
