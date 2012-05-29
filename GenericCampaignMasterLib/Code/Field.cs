using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    [Serializable()]
    public class Field : IEquatable<Field>
    {
        public int Id { get; set; }
		public List<Sektor> ListSektors { get; set; }

        /*
		public void putUnit(IUnit unit, Sektor target)
        {
            if (!ListSektors.Contains(target))
                throw new Exception("Sektor nicht vorhanden");

            Sektor oldPos = getSektorContainingUnit(unit);
            if (oldPos != null)
                oldPos.Remove(unit);

            target.Add(unit);
        }*/
		
        public Sektor getSektorContainingUnit(IUnit unit)
        {
            // Erstmal so gelöst. Geht besser.      
            foreach(Sektor s in ListSektors)
            {
                if(s.ListUnits.Contains(unit))
                    return s;
            }

            return null;
        }	
        
        #region IEquatable<Field> Member

        // TODO: Nur Temporär um die Testklasse auszuprobieren.
        // Muss die Sektoren vergleichen.
        public bool Equals(Field other)
        {
            if (this.Id == other.Id)
                return true;
            else
                return false;
        }

        #endregion
    }
}
