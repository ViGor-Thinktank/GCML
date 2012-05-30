using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    [Serializable()]
    public class Player : IEquatable<Player>
    {
        private int _id;
        public int Id { get { return _id; } }
        public string Playername { get; set; }

        public Player(int playerId)
        {
            this._id = playerId;
        }

        public List<IUnit> ListUnits
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public Ressourcen Ressourcen
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        
    
        #region IEquatable<Player> Member
        
        // TODO: Wenn Ressourcen und ListUnits implementiert sind,
        // Equals Erweitern!
        public bool Equals(Player other)
        {
            if ((this.Id == other.Id) &&
                 (this.Playername == other.Playername))
                return true;
            else
                return false;
        }

        #endregion
    }
}
