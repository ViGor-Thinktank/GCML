using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace GenericCampaignMasterModel
{
    public class UnitInfo
    {
        [Key]
        public string sektorId { get; set; }
        public string playerId { get; set; }
        public string unitId { get; set; }
    }
}
