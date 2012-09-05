using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class SektorEventArgs
    {
        public clsUnit actingUnit;
        public Sektor eventSektor;

        public SektorEventArgs(clsUnit unit, Sektor sektor)
        {
            actingUnit = unit;
            eventSektor = sektor;
        }
    }
}
