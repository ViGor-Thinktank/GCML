using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using GenericCampaignMasterModel;

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
        #endregion

        private List<Sektor> unitCollisionStack = new List<Sektor>();
        private List<clsUnit> unitActedStack = new List<clsUnit>();
        private List<Player> lstFinishedPlayers = new List<Player>();
        private Dictionary<string, ICommand> m_dictCommandCache = new Dictionary<string, ICommand>();

        public event Field.delStatus onStatus;
        public event delTick onTick;
        
        public delegate void delTick();
        

        public void Global_onStatus(string strText)
        {
            if (onStatus != null)
                onStatus(strText);
        }

        public void Tick()
        {
            if (this.onTick != null)
                this.onTick();
        }

        public CampaignController()
        {


        }
        public CampaignController(string strKey)
        {
            this.restoreGameState(strKey);
        }

        public CampaignController(CampaignEngine engine)
        {
            initEngine(engine);
        }

#region " Init, Save, Load "

        private void initEngine(CampaignEngine engine)
        {
            m_campaignEngine = engine;
            foreach (Sektor sektor in engine.FieldField.getSektorList())
            {
                sektor.onUnitEnteredSektor += onUnitMove;
                sektor.onUnitLeftSektor += onUnitMove;
            }

            // Unit CommandHandler registrieren
            this.onTick = null;
            foreach (Player p in m_campaignEngine.ListPlayers)
            {
                foreach (clsUnit u in p.ListUnits)
                {
                    this.onTick += new delTick(u.CampaignController_onTick);
                }
            }

            // ResourceHandler registrieren
            this.onTick += new delTick(m_campaignEngine.ResourceHandler.CampaignController_onTick);
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
            CampaignEngine engine = CampaignEngine.restoreFromState(state);
            initEngine(engine);
        }

#endregion

       

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

            // onUnitMove wird pro Move 2x aufgerufen (Verlassen und Betreten)
            if (!unitActedStack.Contains(args.actingUnit))       
                unitActedStack.Add(args.actingUnit);

            // Todo AccessibleSectors für alle Player berechnen
            // Vorerst jeder Sektor accessible der keine Unit enthält
            foreach (Player p in campaignEngine.ListPlayers)
                p.accessibleSectors = campaignEngine.getAccessibleSektorsForPlayer(p);


        }

        private bool checkSektorForUnitCollision(Sektor sektor)
        {
            bool resultCollision = false;
            if (sektor.ListUnits.Count > 1)
            {
                List<Player> unitOwnersInSektor = new List<Player>();

                foreach (clsUnit unit in sektor.ListUnits)
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
        private void unitCollisionStack_Add(Sektor sektor)
        {
            Global_onStatus("Collision: " + sektor.strUniqueID);
            unitCollisionStack.Add(sektor);
        }

        public void createNewUnit(string strPlayerID, int intUnitTypeID)
        {
            clsUnit newUnit = this.m_campaignEngine.addUnit(strPlayerID, intUnitTypeID);

            this.onTick += new delTick(newUnit.CampaignController_onTick);
        }

        public void createNewUnitAndRegister(Player owner, clsUnitType newUnitType, Sektor targetSektor)
        {
            clsUnit newUnit = new clsUnit(newUnitType);
            newUnit.strOwnerID = owner.Id;
            owner.ListUnits.Add(newUnit);
            targetSektor.addUnit(newUnit);

            this.onTick += new delTick(newUnit.CampaignController_onTick);
        }


        public Player getPlayer(string pID)
        {
            return this.m_campaignEngine.getPlayer(pID);
        }

        public Player addPlayer(string p)
        {
            return this.m_campaignEngine.addPlayer(p);
        }

        public List<Player> getPlayerList()
        {
            return m_campaignEngine.ListPlayers;
        }

        public clsUnit getUnit(string strUnitId)
		{
            return m_campaignEngine.getUnit(strUnitId);
		}

        public UnitInfo getUnitInfo(string unitId)
        {
            clsUnit unit = getUnit(unitId);
            UnitInfo info = m_campaignEngine.getUnitInfo(unit);
            return info;
        }

        public List<ICommand> getCommandsForUnit(clsUnit unit)
        {
            List <ICommand> lstCmds = this.m_campaignEngine.getCommandsForUnit(unit);
            foreach(ICommand cmd in lstCmds)
                m_dictCommandCache.Add(cmd.CommandId, cmd);

            return lstCmds;
        }

        public ICommand getCommand(string commandId)
        {
            ICommand result = null;
            if (m_dictCommandCache.ContainsKey(commandId))
                result = m_dictCommandCache[commandId];


            return result;
        }


        public List<clsUnit> getActiveUnitsForPlayer(Player player)
        {
            List<clsUnit> unitsForPlayer = player.ListUnits;
            var lstUnitsCanAct = from u in player.ListUnits
                                 where !unitActedStack.Contains(u)
                                 select u;

            return new List<clsUnit>(lstUnitsCanAct);
        }

        public Sektor getSektorForUnit(clsUnit unit)
        {
             return campaignEngine.FieldField.getSektorForUnit(unit);
        }

        public Sektor getSektor(string sektorId)
        {
            Sektor result = new Sektor();
            var query = from s in m_campaignEngine.FieldField.dicSektors.Values
                        where s.Id == sektorId
                        select s;
            if (query.Count() > 0)
                result = query.First<Sektor>();

            return result;
        }

        public Player getCampaignStateForPlayer(string pID, string strState = "")
        {
            Player askingPlayer = this.m_campaignEngine.getPlayer(pID);

            this.m_campaignEngine.fillVisibleSektors(ref askingPlayer);

            return askingPlayer;


        }

        

        public CampaignInfo getCampaignInfo()
        {
            CampaignInfo nfo = new CampaignInfo();
            nfo.campaignId = this.CampaignKey;
            nfo.campaignName = this.campaignEngine.CampaignName;
            
            nfo.players = new Dictionary<string, string>();
            foreach (Player p in this.campaignEngine.ListPlayers)
            {
                nfo.players.Add(p.Id, p.Playername);
            }

            
            return nfo;
        }



        public void endRound(Player p)
        {
            if (!lstFinishedPlayers.Contains(p))
                lstFinishedPlayers.Add(p);

            if (lstFinishedPlayers.Count() == m_campaignEngine.ListPlayers.Count())
            {
                Tick();
                lstFinishedPlayers.Clear();
                m_dictCommandCache.Clear();

            }

            // Neue Units die aus Ressourcen angelegt wurden registrieren
            while (m_campaignEngine.ResourceHandler.CreatedUnitIds.Count > 0)
            {
                string newUnitId = m_campaignEngine.ResourceHandler.CreatedUnitIds.Pop();
                clsUnit unit = m_campaignEngine.getUnit(newUnitId);
                this.onTick += new delTick(unit.CampaignController_onTick);
            }
            
        }

        #region Ressourcen Handling

        public List<ResourceInfo> getRessourcesForPlayer(Player player)
        {
            List<ResourceInfo> result;
            List <ResourceInfo> resInfo = m_campaignEngine.ResourceHandler.getResourceInfo();

            result = (from p in resInfo
                      where p.ownerId == player.Id
                      select p as ResourceInfo).ToList<ResourceInfo>();

            return result;
        }


        public void addResource(ResourceInfo resInfo)
        {
            string strResType = resInfo.resourceableType;
            Type resType = Type.GetType(strResType);
            Object typeObj = Activator.CreateInstance(resType);

            Player resourceOwner = (from p in campaignEngine.ListPlayers
                                    where p.Id == resInfo.ownerId
                                    select p).First() as Player;


            campaignEngine.ResourceHandler.addRessourcableObject(resourceOwner, (IResourceable)typeObj);
        }

        // Kapselt die Aufrufe von getResourceCommands im ResourceHandler um die gelieferten Commands 
        // im CommandCache zu speichern
        public List<ICommand> getResourceCommands(string resourceId)
        {
            List<ICommand> lstCmds = this.campaignEngine.ResourceHandler.getResourceCommands(resourceId);   
            foreach (ICommand cmd in lstCmds)
                m_dictCommandCache.Add(cmd.CommandId, cmd);

            return lstCmds;
        }
        #endregion


        public List<clsUnitType> getCampaignInfo_UnitTypes()
        {
            clsUnitTypeCollection objUnitTypeCollection = new clsUnitTypeCollection();
            objUnitTypeCollection.Add(new clsUnitType(objUnitTypeCollection.dicUnitTypeData.Count, "Star Destroyer"));
            return objUnitTypeCollection.dicUnitTypeData.Values.ToList<clsUnitType>();
        }

        public clsUnitType getCampaignInfo_UnitTypeByID(int intUnitID)
        {
            clsUnitTypeCollection objUnitTypeCollection = new clsUnitTypeCollection();
            objUnitTypeCollection.Add(new clsUnitType(objUnitTypeCollection.dicUnitTypeData.Count, "Star Destroyer"));
            return objUnitTypeCollection.dicUnitTypeData[intUnitID.ToString()];
        }
    }

}
