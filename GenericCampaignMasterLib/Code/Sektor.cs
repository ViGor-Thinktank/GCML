using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    [Serializable()]
    public class Sektor : List<IUnit>, IEquatable<Sektor>
    {
        private int _id;
        public int Id { get { return _id; } }

        public Sektor(int sektorId)
        {
            this._id = sektorId;
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
