using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    [Serializable()]
    public class Sektor : IEquatable<Sektor>
    {
        public Sektor()
        {
            m_objSektorType = new clsSektorType_base();
        }


        public Sektor(string sektorId) 
        {
            m_strID = sektorId;
            m_objSektorType = new clsSektorType_base();
        }
        public Sektor(string sektorId, clsSektorType_base objSektorType) 
        {
            m_strID = sektorId;
            m_objSektorType = objSektorType; 
        }
        

        private string m_strID;
        private List<clsUnit> m_ListUnits = new List<clsUnit>();

        private clsSektorType_base m_objSektorType;

        public string Id 
        { 
            get 
        { 
                return m_strID; 
            }
            set
            {
                m_strID = value;
            }
        }

        public List<clsUnit> ListUnits 
        { 
            get 
            {
                return m_ListUnits; 
            }
            set
            {
                m_ListUnits = ListUnits;
            }
        }

        public delegate void UnitEnteringHandler(object sender, SektorEventArgs args);
        public delegate void UnitLeavingHandler(object sender, SektorEventArgs args);

        public event UnitEnteringHandler onUnitEnteredSektor;
        public event UnitLeavingHandler onUnitLeftSektor;

        public string strUniqueID { get { return (this.objSektorKoord != null) ? this.objSektorKoord.uniqueIDstr():"" ; } }

        public clsSektorKoordinaten objSektorKoord { get; set; }



        public void addUnit(clsUnit unit)
		{
			m_ListUnits.Add (unit);

            if(onUnitEnteredSektor != null)
                onUnitEnteredSektor(this, new SektorEventArgs(unit, this));
		}

        public void removeUnit(clsUnit unit)
		{

            clsUnit aktUnit = null;

            var objUnitList = from u in this.m_ListUnits
                                where u.Id == unit.Id
                                select u;

            
            if (objUnitList.Count() == 1)
                aktUnit = objUnitList.First();
            else if (objUnitList.Count() > 1)
                throw new Exception("mehr als ein Treffer für UnitID " + unit.Id);
            else
                throw new Exception("kein Treffer für UnitID " + unit.Id);

            m_ListUnits.Remove(aktUnit);
            
            if(onUnitLeftSektor != null)
                onUnitLeftSektor(this, new SektorEventArgs(unit, this));
		}

        #region IEquatable<Sektor> Member
        
		public bool Equals(Sektor other)
        {
            if (other.Id == this.Id)
                return true;
            else
                return false;
        }

        #endregion
        
        public int intMoveCost { get { return m_objSektorType.intMoveCost; } }


        public SektorInfo getInfo()
        {
            SektorInfo sektornfo = new SektorInfo();
            sektornfo.sektorId = this.strUniqueID;
            sektornfo.sektorKoordinaten = this.objSektorKoord;
            sektornfo.containedUnitIds = (from u in this.ListUnits
                                          select u.Id).ToList<string>();
            return sektornfo;
        }
    }
}
