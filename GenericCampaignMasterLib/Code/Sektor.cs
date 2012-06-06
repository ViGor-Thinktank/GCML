using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib.Code.Unit;

namespace GenericCampaignMasterLib
{
    [Serializable()]
    public class Sektor : IEquatable<Sektor>
    {
        private string m_strID;
		private List<IUnit> m_ListUnits;

        public string Id { get { return m_strID; } }
		public List<IUnit> ListUnits 
        { 
            get 
            {
                if (m_ListUnits == null) { m_ListUnits = new List<IUnit>(); }
                return m_ListUnits; 
            }
        }
        public event EventHandler onUnitEnteredSektor;
        public event EventHandler onUnitLeftSektor;

        public string strUniqueID { get { return (this.objSektorKoord != null) ? this.objSektorKoord.uniqueIDstr():"" ; } }

        public Field.clsSektorKoordinaten objSektorKoord;

        public Sektor(string sektorId)
		{
			m_strID = sektorId;
			m_ListUnits = new List<IUnit> ();
        }

		public void addUnit (IUnit unit)
		{
			m_ListUnits.Add (unit);

            if(onUnitEnteredSektor != null)
                onUnitEnteredSektor(this, new EventArgs());
		}
		
		public void removeUnit (IUnit unit)
		{
			m_ListUnits.Remove(unit);

            if(onUnitLeftSektor != null)
                onUnitLeftSektor(this, new EventArgs());
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
        private int m_intMoveCost = 1;
        public int intMoveCost { get { return m_intMoveCost; } }
    }
}
