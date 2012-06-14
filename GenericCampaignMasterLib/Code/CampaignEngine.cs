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
        private Dictionary<int, Player> m_ListPlayers = null;
        public Dictionary<int, Player> ListPlayers
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

        // Todo: Methode soll nur die Units zurückliefern die aktivierbar sind
		// Auf die Liste aller Units des Players kann über die List-Property zugegriffen werden.
        public List<IUnit> getActiveUnitsForPlayer(Player p)
        {
            return p.ListUnits;
        }

        private clsMoveFactory m_objMovFactory;
        private List<ICommand>m_lisReadyCommands;

        public List<ICommand> getCommandsForUnit(IUnit u)
		{
            List<ICommand> listRawCommands = u.getCommands();           // Unfertige Commands von der Unit - Enthalten keine Position-/Zielsektoren
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
        public List<Move> getDefaultMoveCommandsForUnit(IUnit u)
        {
            List<Move> listMoves = new List<Move>();
            List<ICommand> cmds = getCommandsForUnit(u);
			foreach (ICommand c in cmds)
			{
                if (c.GetType() == typeof(Move))
                    listMoves.Add((Move)c);
			}

            return listMoves;
        }

        public List<Sektor> getViewableSectorsForUnit(IUnit u)
        {
            /*List<Sektor> sektorList = FieldField.objSektorCollection.getSektorList();
            return sektorList;
*/
            return null;
        }

        public Sektor getSektorContainingUnit(IUnit u)
        {
            return FieldField.getSektorForUnit(u);
            
        }

        public List<UnitInfo> getUnitInfo()
        {
            List<UnitInfo> result = new List<UnitInfo>();
            foreach(Player p in Players.Values)
            {
                foreach(IUnit u in p.ListUnits)
                {
                    Sektor s = getSektorContainingUnit(u);
                    
                    UnitInfo uInfo = new UnitInfo();
                    uInfo.sektorId = s.objSektorKoord.uniqueIDstr();
                    uInfo.unitId = u.Id.ToString();
                    uInfo.unitType = u.UnitType.ToString ();
					uInfo.playerId = p.Id.ToString();
                    result.Add(uInfo);
                }
                
            }

            return result;
        }

        public Player getUnitOwner(IUnit unit)
        {
            var owner = (from p in m_ListPlayers.Values
                         where p.ListUnits.Contains(unit)
                         select p).First();

            return owner as Player;
        }


        #region " Unitfactory "

        internal IUnit addUnit(int intPlayerID, Type UnitType)
        {
            IUnit newUnit = null;

            if (UnitType == typeof(DummyUnit))
            {
                newUnit = new DummyUnit(m_ListPlayers[intPlayerID].ListUnits.Count);
            }

            return addUnit(intPlayerID, newUnit, this.FieldField.nullSektorKoord);
        }

        public IUnit addUnit(int intPlayerID, IUnit newUnit, clsSektorKoordinaten objSektorKoord)
        {
            this.Players[intPlayerID].ListUnits.Add(newUnit);
            this.FieldField.get(objSektorKoord).ListUnits.Add(newUnit);

            return newUnit;
        }

 
        #endregion

        private Dictionary<int, Player> Players 
        { 
            get 
            {
                if (m_ListPlayers == null) { m_ListPlayers = new Dictionary<int, Player>(); }
                return m_ListPlayers;
            }
        }

        public Player addPlayer(string strPlayerName)
        {
            Player newPlayer = new Player(Players.Count);
            newPlayer.Playername = strPlayerName;

            return this.addPlayer(newPlayer);
        }

        public Player addPlayer(Player objNewPlayer)
        {
            if (Players.ContainsKey(objNewPlayer.Id))
            {
                throw new Exception_Engine_Player("PlayerID ist bereits vergeben!");
            }
            
            Players.Add(objNewPlayer.Id, objNewPlayer);
            
            return objNewPlayer;
        }

        public void setPlayerList(List<Player> list)
        {
            foreach (Player aktPlayer in list)
            {
                addPlayer(aktPlayer);
            }
        }



        public Dictionary<string, Sektor> getVisibleSektorsForPlayer(Player p)
        {
            clsViewableSectorFactory facViewSek = new clsViewableSectorFactory(this.FieldField);

            foreach (IUnit aktUnit in p.ListUnits)
            {
                facViewSek.getVisibleSektorsFromUnitSektor(this.getSektorContainingUnit(aktUnit), aktUnit.intSichtweite);
    
            }

            
            return facViewSek.ListVisibleSektors;
        }
    }
}
