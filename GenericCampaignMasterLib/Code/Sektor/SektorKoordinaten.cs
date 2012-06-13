using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class clsSektorKoordinaten
    {
        private List<int> m_lstIntPosition;
        public List<int> Position
        {
            get
            {
                return m_lstIntPosition;
            }

            set
            {
                m_lstIntPosition = value;
            }
        }

        public virtual bool Equals(clsSektorKoordinaten other)
        {
            if (this.Position == other.Position)
                return true;
            else
                return false;
        }

        public virtual string uniqueIDstr()
        {
            string result = "|";
            foreach (int i in m_lstIntPosition)
                result += i.ToString() + "|";
            
            return result;
        }


    }
}
