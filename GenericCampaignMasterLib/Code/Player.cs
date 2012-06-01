using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib.Code.Unit;

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

        private List<IUnit> m_Units = null;

        public List<IUnit> ListUnits
        {
            get
            {
                if (m_Units == null) { m_Units = new List<IUnit>(); }
                return m_Units;
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
