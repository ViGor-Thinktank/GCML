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
        public CampaignInfo()
        {
            campaignId = String.Empty;
            campaignName = String.Empty;
            ListPlayerInfo = new List<PlayerInfo>();
            FieldDimension = new clsSektorKoordinaten();
            SektorField = new SektorInfo[0, 0];
            ListUnits = new List<UnitInfo>();
            objCampaignData = new CampaignState();
        }

        public string campaignId { get; set; }
        public string campaignName { get; set; }
        public List<PlayerInfo> ListPlayerInfo { get; set; }
        public clsSektorKoordinaten FieldDimension { get; set; }
        public SektorInfo[,] SektorField { get; set; }
        public List<UnitInfo> ListUnits { get; set; }
        public CampaignState objCampaignData;
    }
}
