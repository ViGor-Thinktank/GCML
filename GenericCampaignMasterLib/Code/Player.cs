using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace GenericCampaignMasterLib
{
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

        public override string ToString()
        {
            return new JavaScriptSerializer().Serialize(this);
        }

        public static Player FromString(string strData)
        {
            return new JavaScriptSerializer().Deserialize<Player>(strData);
        }

        public List<clsUnit> ListUnits = new List<clsUnit>();

        public Dictionary<string, Sektor> dicVisibleSectors;


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



        internal clsUnit getUnitByID(string strUnitID)
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



    }
}
