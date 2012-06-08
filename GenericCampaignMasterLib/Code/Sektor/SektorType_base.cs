using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class clsSektorType_base
    {
        public clsSektorType_base() { }
        public clsSektorType_base(int intMoveCost) { m_intMoveCost = intMoveCost; }
        
        private int m_intMoveCost = 1;

        public int intMoveCost
        {
            get { return m_intMoveCost; }
        }
    }
}
