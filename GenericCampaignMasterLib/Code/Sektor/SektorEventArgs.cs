using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class SektorEventArgs
    {
        public Unit.clsUnit actingUnit;
        public Sektor eventSektor;

        public SektorEventArgs(Unit.clsUnit unit, Sektor sektor)
        {
            actingUnit = unit;
            eventSektor = sektor;
        }
    }
}
