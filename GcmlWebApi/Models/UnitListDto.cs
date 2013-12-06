using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using GenericCampaignMasterModel;

namespace GcmlWebApi
{
    public class UnitListDto
    {
        public List<UnitInfo> unitList { get; set; }
    }
}