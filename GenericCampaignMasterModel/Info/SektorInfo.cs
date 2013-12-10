using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace GenericCampaignMasterModel
{
    public class SektorInfo
    {
        [Key]
        public string sektorId { get; set; }
        public clsSektorKoordinaten sektorKoordinaten { get; set; }
        public List<string> containedUnitIds;
    }
}
