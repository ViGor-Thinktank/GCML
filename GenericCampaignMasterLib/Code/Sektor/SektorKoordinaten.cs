using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public abstract class clsSektorKoordinaten
    {
        public abstract bool Equals(clsSektorKoordinaten other);

        public abstract string uniqueIDstr();

    }
}
