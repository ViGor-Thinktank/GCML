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
        private int m_intId;
		private List<IUnit> m_ListUnits;
		
        public int Id { get { return m_intId; } }
		public List<IUnit> ListUnits { get { return m_ListUnits; }}
        public event EventHandler UnitEnteredSektor;
        public event EventHandler UnitLeftSektor; 

        public Sektor (int sektorId)
		{
			m_intId = sektorId;
			m_ListUnits = new List<IUnit> ();
        }

		public void addUnit (IUnit unit)
		{
			m_ListUnits.Add (unit);

            if(UnitEnteredSektor != null)
                UnitEnteredSektor(this, new EventArgs());
		}
		
		public void removeUnit (IUnit unit)
		{
			m_ListUnits.Remove (unit);

            if(UnitLeftSektor != null)
                UnitLeftSektor(this, new EventArgs());
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
    }
}
