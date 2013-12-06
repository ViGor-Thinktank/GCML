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
        public string campaignId { get; set; }
        public string campaignName { get; set; }
        public List<PlayerInfo> ListPlayerInfo { get; set; }
        public clsSektorKoordinaten FieldDimension { get; set; }
        public SektorInfo[,] SektorField { get; set; }
        public CampaignState objCampaignData;
    }
}
