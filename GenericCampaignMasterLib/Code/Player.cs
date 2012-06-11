﻿using System;
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

        private List<IUnit> m_lisEinheiten;

        public List<IUnit> ListUnits
        {
            get
            {
                if (m_lisEinheiten == null) { m_lisEinheiten = new List<IUnit>(); }
                return m_lisEinheiten;
            }

        }

        private Dictionary<string, Sektor> m_dicVisibleSectors;

        public Dictionary<string, Sektor> dicVisibleSectors
        {
            get
            {
                if (m_dicVisibleSectors == null) { m_dicVisibleSectors = new Dictionary<string, Sektor>(); }
                return m_dicVisibleSectors;
            }

        }

        public Ressourcen Ressourcen
        {
            get
            {
                //throw new System.NotImplementedException();
                return new Ressourcen();
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
