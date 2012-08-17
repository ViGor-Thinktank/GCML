using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;

namespace GenericCampaignMasterLib
{
    public class clsSektorKoordinaten
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

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

        public clsSektorKoordinaten()
        {

        }

        public clsSektorKoordinaten(int x = 0, int y = 0, int z = 0)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.m_lstIntPosition = new List<int> { x, y, z };
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

        public static clsSektorKoordinaten operator +(clsSektorKoordinaten c1, clsSektorKoordinaten c2)
        {
            return new clsSektorKoordinaten(c1.X + c2.X, c1.Y + c2.Y, c1.Z + c2.Z);
        }

        public string ToJson()
        {
            return new JavaScriptSerializer().Serialize(this);
        }
    }
}
