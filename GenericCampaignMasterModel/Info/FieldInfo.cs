using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel
{
    public class FieldInfo
    {
        public CampaignInfo Campaign { get; set; }
        public clsSektorKoordinaten FieldKoord { get; set; }
        public IEnumerable<SektorInfo> ListSektors { get; set; }
        public SektorInfo[,] SektorField { get; set; }
    }
}
