using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class SektorEventArgs
    {
        public IUnit actingUnit;
        public Sektor eventSektor;

        public SektorEventArgs(IUnit unit, Sektor sektor)
        {
            actingUnit = unit;
            eventSektor = sektor;
        }
    }
}
