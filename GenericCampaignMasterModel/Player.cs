using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel
{
    [Serializable()]
    public class Faction : IEquatable<Faction>
    {
        public Faction()
            { }

        public Faction(string strFactionName, List<clsUnitType> listUnitspawn = null)
        { 
            this.strFactionName = strFactionName;
            this.listUnitspawn = listUnitspawn;
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set { this._id = value; }
        }

      public string strFactionName { get; set; }

      public bool Equals(Faction other)
      {
          if ((this.Id == other.Id) &&
               (this.strFactionName == other.strFactionName))
              return true;
          else
              return false;
      }

      public List<clsUnitType> listUnitspawn = null;
    }

    [Serializable()]
    public class Player : IEquatable<Player>
    {
      

        private string _id;
        public string Id 
        { 
            get { return _id; }
            set { this._id = value;  }
        }

        public string Playername { get; set; }

        public Player() { }
        public Player(string playerId)
        {
            this._id = playerId;            
        }

        public Faction objPlayerFaction = null;
      
        public List<clsUnitGroup> ListUnits = new List<clsUnitGroup>();

        public Sektor unitspawnSektor;

        public Dictionary<string, Sektor> dicVisibleSectors;
        public List<Sektor> accessibleSectors { get; set; } // Sektoren in denen produzierte Einheiten plaziert werden können.

        #region IEquatable<Player> Member
        
        // TODO: Wenn Ressourcen und ListUnits implementiert sind,
        // Equals Erweitern!
        public bool Equals(Player other)
        {
            return ((this.Id == other.Id) && (this.Playername == other.Playername));
        }

        #endregion

        public clsUnitGroup getUnitByID(string strUnitID)
        {
            return (from u in this.ListUnits
                           where u.Id == strUnitID
                           select u).First();
        }

        public void Done()
        {
            this.m_blnDone = true;
        }

        public bool m_blnDone { get; set; }

        public PlayerInfo getPlayerInfo()
        {
            PlayerInfo nfo = new PlayerInfo();
            nfo.playerId = this.Id;
            nfo.playerName = this.Playername;
            return nfo;
        }

        public static Player FromString(string strPlayer)
        {
            throw new NotImplementedException();
        }
    }
}
