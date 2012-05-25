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

        public Sektor Sektor
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
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
