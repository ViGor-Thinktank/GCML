using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel
{
    public class SektorEventArgs
    {
        public clsUnitGroup actingUnit;
        public Sektor eventSektor;

        public SektorEventArgs(clsUnitGroup unit, Sektor sektor)
        {
            actingUnit = unit;
            eventSektor = sektor;
        }
    }
}
