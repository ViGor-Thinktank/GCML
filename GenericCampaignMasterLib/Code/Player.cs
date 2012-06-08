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

        public Player(int playerId, CampaignEngine myEngine)
        {
            this._id = playerId;
            this.m_objMyEngine = myEngine;
        }

        private CampaignEngine m_objMyEngine;

        private List<IUnit> m_lisEinheiten; 
        public List<IUnit> ListUnits
        {
            get
            {
                if (m_lisEinheiten == null) { m_lisEinheiten = new List<IUnit>(); }
                return m_lisEinheiten;
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

        //tmp
        public List<ICommand> lisCommands; 
            
    
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

        public void createNewUnit(Type UnitType)
        {
            m_objMyEngine.addUnit(this.Id, UnitType);
        }

        public void getGameState()
        {
            m_lisEinheiten = m_objMyEngine.getActiveUnitsForPlayer(this);
            lisCommands = m_objMyEngine.getCommandsForUnit(ListUnits[0]);
        }
    }
}
