using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using GenericCampaignMasterLib;

namespace GenericCampaignMasterLib
{
    public class CampaignController
    {
        # region Properties
        private CampaignEngine m_campaignEngine;
        public CampaignEngine campaignEngine
        {
            get { return m_campaignEngine; }
            set { initEngine(value); }

        }

        private ICampaignDatabase m_campaignDataBase;
        public ICampaignDatabase CampaignDataBase
        {
            get { return m_campaignDataBase; }
            set { m_campaignDataBase = value; }

        }

		public string CampaignKey { get; set;}
		
        private List<Sektor> unitCollisionStack = new List<Sektor>();
        private List<IUnit> unitActedStack = new List<IUnit>();
        #endregion


        public event Field.delStatus onStatus;
        
		
        public void Global_onStatus(string strText)
        {
            if (onStatus != null)
                onStatus(strText);
        }

        public CampaignController()
        {


        }

        public CampaignController(CampaignEngine engine)
        {
            initEngine(engine);
        }

        private void initEngine(CampaignEngine engine)
        {
            m_campaignEngine = engine;
            foreach (Sektor sektor in engine.FieldField.getSektorList())
            {
                sektor.onUnitEnteredSektor += onUnitMove;
                sektor.onUnitLeftSektor += onUnitMove;
            }
        }

        public string saveCurrentGameState()
        {
            CampaignState state = m_campaignEngine.getState();
            string key = m_campaignDataBase.saveGameState(state);
            return key;
        }

        public void restoreGameState(string key)
        {
            CampaignState state = m_campaignDataBase.getCampaignStateByKey(key);
            CampaignEngine engine = state.Restore();
            initEngine(engine);
        }

        public List<Sektor> getUnitCollisions()
        {
            return unitCollisionStack;
        }
       
        private void onUnitMove(object sender, SektorEventArgs args)
        {
            Sektor sektor = sender as Sektor;
            if (checkSektorForUnitCollision(sektor))
            {
                unitCollisionStack_Add(sektor);
            }
            else if (unitCollisionStack.Contains(sektor))
            {
                unitCollisionStack.Remove(sektor);
            }

            if (!unitActedStack.Contains(args.actingUnit))       // onUnitMove wird pro Move 2x aufgerufen (Verlassen und Betreten)
                unitActedStack.Add(args.actingUnit);
        }

        private void unitCollisionStack_Add(Sektor sektor)
        {
            Global_onStatus("Collision: " + sektor.strUniqueID);
            unitCollisionStack.Add(sektor);
        }

        public Player getPlayer(string pID)
        {
            return this.m_campaignEngine.getPlayer(pID);
        }

        public void createNewUnit(string strPlayerID, Type type)
        {
            this.m_campaignEngine.addUnit(strPlayerID, type);
        }


        # region Public Clientfunktionen

        public Player addPlayer(string p)
        {
            return this.m_campaignEngine.addPlayer(p);
        }

        

        public List<Player> getPlayerList()
        {
            return m_campaignEngine.ListPlayers;
        }
			
		public IUnit getUnit (string unitId)
		{
			return m_campaignEngine.getUnit (unitId);
		}

        public List<ICommand> getCommandsForUnit(IUnit unit)
        {
            return this.m_campaignEngine.getCommandsForUnit(unit);
        }

        public List<IUnit> getActiveUnitsForPlayer(Player player)
        {
            List<IUnit> unitsForPlayer = player.ListUnits;
            var lstUnitsCanAct = from u in player.ListUnits
                                 where !unitActedStack.Contains(u)
                                 select u;

            return new List<IUnit>(lstUnitsCanAct);
        }

        public Sektor getSektorForUnit(IUnit unit)
        {
             return campaignEngine.FieldField.getSektorForUnit(unit);
        }

        public Sektor getSektor(string sektorId)
        {
            Sektor result = new Sektor();
            var query = from s in m_campaignEngine.FieldField.ListSektors.Values
                        where s.Id == sektorId
                        select s;
            if (query.Count() > 0)
                result = query.First<Sektor>();

            return result;
        }

        # endregion

        # region Prüffunktionen

        private bool checkSektorForUnitCollision(Sektor sektor)
        {
            bool resultCollision = false;
            if (sektor.ListUnits.Count > 1)
            {
                List<Player> unitOwnersInSektor = new List<Player>();
                foreach (IUnit unit in sektor.ListUnits)
                {
                    Player owner = m_campaignEngine.getUnitOwner(unit);
                    if (!unitOwnersInSektor.Contains(owner))
                        unitOwnersInSektor.Add(owner);
                }

                if (unitOwnersInSektor.Count() > 1)
                    resultCollision = true;
            }
            else
            {
                ;
            }

            return resultCollision;
        }



        private void checkForRoundEnd()
        {


        }

        private void newRound()
        {

        }

        #endregion


        public CampaignState_Player getCampaignStateForPlayer(string pID)
        {

            //Player askingPlayer = this.m_campaignEngine.ListPlayers[pID];
            CampaignState_Player objPlayerInfo = new CampaignState_Player();
            objPlayerInfo.p = this.m_campaignEngine.getPlayer(pID);
            this.m_campaignEngine.fillVisibleSektors(ref objPlayerInfo.p);

            return objPlayerInfo;


            //return newState;
        }
    }

}
