using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GenericCampaignMasterLib;

namespace GenericCampaignMasterLib
{
    [Serializable()]
    public class CampaignEngine
    {
        public event Field.delStatus onStatus;

        public CampaignEngine(Field newField)
        {
            this.m_FieldField = newField;
            this.m_FieldField.onFieldStatus += new Field.delStatus(Global_onStatus);
            this.onEngineStatus += new Field.delStatus(Global_onStatus);
        }

        public void Global_onStatus(string strText)
        {
            if (onStatus != null)
                onStatus(strText);
        }

        public CampaignState getState()
        {
            CampaignState state = new CampaignState();
            return state.Save(this);
        }

        #region " Properties && Felder "
        private List<Player> m_ListPlayers = new List<Player>();
        public List<Player> ListPlayers
        {
            get
            {
                return m_ListPlayers;
            }
        }

        private Field m_FieldField;

        public Field FieldField
        {
            get { return m_FieldField; }
            }

        #endregion

        private clsMoveFactory m_objMovFactory;
        private List<ICommand>m_lisReadyCommands;

        public List<ICommand> getCommandsForUnit(BaseUnit u)
		{
            List<ICommand> listRawCommands = u.getTypeCommands();           // Unfertige Commands von der Unit - Enthalten keine Position-/Zielsektoren
            m_lisReadyCommands = new List<ICommand>();                   // Liste mit vollständigen Commands - wird zurückgeliefert.
        	
			foreach (ICommand cmdRaw in listRawCommands)
			{
				if (cmdRaw.GetType() == typeof(Move))
				{
                    m_objMovFactory = new clsMoveFactory(u, FieldField);
                    m_objMovFactory.onNewMoveCommand += new clsMoveFactory.delNewMoveCommand(m_objMovFactory_onNewMoveCommand);
                    m_objMovFactory.onNewStatus += new clsMoveFactory.delNewStatus(m_objMovFactory_onNewStatus);
                    m_objMovFactory.go();
				}
			}
			
			return m_lisReadyCommands;
        }

        public event Field.delStatus onEngineStatus;

        void m_objMovFactory_onNewStatus(string strStatus)
        {
            if (this.onEngineStatus != null)
                this.onEngineStatus(strStatus);
        }

        void m_objMovFactory_onNewMoveCommand(Move readyCmd)
        {
            m_lisReadyCommands.Add(readyCmd);
        }

        
        /// <summary>
        /// Erstmal zum Testen: Einheit liefert i.d.R. nur ein Command für Move.
        /// Liefert nur eine Collection aus möglichen Moves.
        /// </summary>
        public List<ICommand> getDefaultMoveCommandsForUnit(BaseUnit u)
        {
            if (u.cmdCache == null)
                u.cmdCache = getCommandsForUnit(u);
            return u.cmdCache;
        }

        public List<Sektor> getViewableSectorsForUnit(IUnit u)
        {
            /*List<Sektor> sektorList = FieldField.objSektorCollection.getSektorList();
            return sektorList;
*/
            return null;
        }
        
        public Sektor getSektorContainingUnit(BaseUnit u)
        {
            return FieldField.getSektorForUnit(u);
            
        }

        public List<UnitInfo> getUnitInfo()
        {
            List<UnitInfo> result = new List<UnitInfo>();
            foreach(Player p in ListPlayers)
            {
                foreach (BaseUnit u in p.ListUnits)
                {
                    UnitInfo info = getUnitInfo(u);
                    result.Add(info);
                }
            }

            return result;
        }

        public UnitInfo getUnitInfo(BaseUnit unit)
        {
            Sektor s = getSektorContainingUnit(unit);
            Player owner = getUnitOwner(unit);

            string sektorId = (s == null) ? "" : s.strUniqueID;
            string ownerId = (owner == null) ? "" : owner.Id.ToString();

            UnitInfo uInfo = new UnitInfo();
            uInfo.sektorId = sektorId;
            uInfo.unitId = unit.Id.ToString();
            uInfo.playerId = ownerId;
            uInfo.unitType = unit.UnitType.GetType().ToString();

            return uInfo;

        }


        public Player getUnitOwner(BaseUnit  unit)
        {

            var owner = (from p in m_ListPlayers
                         where p.Id == unit.ownerID 
                         select p).First();

            return owner as Player;
        }

        public BaseUnit getUnit(string id)
		{
        	var units = from p in m_ListPlayers
						from u in p.ListUnits
						where u.Id.ToString() == id
						select u;
            
            //korrekter Ansatz: Wenn es mehr als einen Traffer bei einer eineindeutigen Suche gibt, ist das ein Fehler! 
            if (units.Count() == 0)
                throw new Exception("kein Treffer für id " + id);
            else if (units.Count() == 1)
                return units.First ();
            else
                throw new Exception("mehr als ein Treffer für id " + id);
                
		}

        #region " Unitfactory "

        public IUnit addUnit(Player owner, BaseUnit newUnit, Sektor sektor)
        {
            newUnit.ownerID = owner.Id;
            owner.ListUnits.Add(newUnit);
            sektor.addUnit(newUnit);
            return newUnit;
        }


        internal BaseUnit addUnit(string strPlayerID, Type UnitType)
        {
            BaseUnit newUnit = null;

            if (UnitType == typeof(DummyUnit))
            {
                //Ergibt nicht eineindeutige UnitIDs
                newUnit = new DummyUnit(strPlayerID + getPlayer(strPlayerID).ListUnits.Count.ToString());
                newUnit.ownerID = strPlayerID;
            }

            return addUnit(strPlayerID, newUnit, this.FieldField.nullSektorKoord);
        }

        public Player getPlayer(string playerId)
        {

            foreach (Player aktP in m_ListPlayers)
            {
                if (aktP.Id == playerId)
                    return aktP;
            }
            return null;

            /*/gibt falsches (immer den ersten von allen) Ergebnis zurück
            var qplayers = from p in m_ListPlayers
                           where p.Id == playerId
                           select p;

            if (qplayers.Count() > 0)
                return qplayers.First();
            else
                return null;*/
        }

        public BaseUnit addUnit(string strPlayerID, BaseUnit newUnit, clsSektorKoordinaten objSektorKoord)
        {

            newUnit.ownerID = strPlayerID;
            getPlayer(strPlayerID).ListUnits.Add(newUnit);
            this.FieldField.get(objSektorKoord).ListUnits.Add(newUnit);

            return newUnit;
        }

 
        #endregion

        public void flushPlayers()
        {
            this.m_ListPlayers.Clear();
        }

        public Player addPlayer(string strPlayerName)
        {
            Player newPlayer = new Player(ListPlayers.Count.ToString());
            newPlayer.Playername = strPlayerName;

            return this.addPlayer(newPlayer);
        }

        public Player addPlayer(Player objNewPlayer)
        {
            if (m_ListPlayers == null)
                m_ListPlayers = new List<Player>();

            m_ListPlayers.Add(objNewPlayer);
            
            return objNewPlayer;
        }

        public void setPlayerList(List<Player> list)
        {
            foreach (Player aktPlayer in list)
            {
                addPlayer(aktPlayer);
            }
        }

        public void fillVisibleSektors(ref Player p)
        {
            clsViewableSectorFactory facViewSek = new clsViewableSectorFactory(this.FieldField);

            foreach (BaseUnit aktUnit in p.ListUnits)
            {
                facViewSek.getVisibleSektorsFromUnitSektor(this.getSektorContainingUnit(aktUnit), aktUnit.Sichtweite);
    
            }

            p.dicVisibleSectors = facViewSek.ListVisibleSektors;
        }

        
    }
}
