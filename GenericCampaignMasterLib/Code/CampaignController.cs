﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using GenericCampaignMasterModel;
using GenericCampaignMasterModel.Commands;

namespace GenericCampaignMasterLib
{
    public class CampaignController
    {
        public CampaignController()
        {


        }
        public CampaignController(string strKey)
        {
            this.GameState_restoreByKey(strKey);
        }
        public CampaignController(CampaignEngine engine)
        {
            initEngine(engine);
        }

# region Properties

        private CampaignEngine m_campaignEngine;
        public CampaignEngine CampaignEngine
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

        public Field FieldField
        {
            get
            {
                if (CampaignEngine != null)
                    return CampaignEngine.FieldField;
                else
                    return null;
            }
        }

        private List<Sektor> unitCollisionStack = new List<Sektor>();
        private List<clsUnitGroup> unitActedStack = new List<clsUnitGroup>();
        private List<Player> lstFinishedPlayers = new List<Player>();
        private Dictionary<string, ICommand> m_dictCommandCache = new Dictionary<string, ICommand>();

        public event Field.delStatus onStatus;
        public event delTick onTick;
        public event delTick onHasTicked;
        
#endregion

        public void Global_onStatus(string strText)
        {
            if (onStatus != null)
                onStatus(strText);
        }

        public delegate void delTick();

        public void Tick()
        {
            if (this.onTick != null)
                this.onTick();
            if (this.onHasTicked != null)
                this.onHasTicked();
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
            foreach (Player p in m_campaignEngine.lisPlayers)
            {
                foreach (clsUnitGroup u in p.ListUnits)
                {
                    this.onTick += new delTick(u.CampaignController_onTick);
                }
            }

            // ResourceHandler registrieren
            this.onTick += new delTick(m_campaignEngine.ResourceHandler.CampaignController_onTick);
        }

        public string GameState_saveCurrent()
        {
            CampaignState state = m_campaignEngine.getState();
            string key = m_campaignDataBase.saveGameState(state);
            return key;
        }

        public void GameState_restoreByKey(string key)
        {
            CampaignState state = m_campaignDataBase.getCampaignStateByKey(key);
            CampaignEngine engine = CampaignEngine.restoreFromState(state);
            initEngine(engine);
        }

#endregion

       
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
            foreach (Player p in CampaignEngine.lisPlayers)
                p.accessibleSectors = CampaignEngine.getAccessibleSektorsForPlayer(p);


        }
        private bool checkSektorForUnitCollision(Sektor sektor)
        {
            bool resultCollision = false;
            if (sektor.ListUnits.Count > 1)
            {
                List<Player> unitOwnersInSektor = new List<Player>();

                foreach (clsUnitGroup unit in sektor.ListUnits)
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

        public Faction Faction_add(string strFactionName, List<clsUnitType> listUnitspawn)
        {
            return this.m_campaignEngine.addGetFaction(strFactionName, listUnitspawn);
        }
      
        public Player Player_getByID(string pID)
        {
            return this.m_campaignEngine.getPlayerByID(pID);
        }

        public Player Player_getByName(string strName)
        {
            return this.m_campaignEngine.getPlayerByName(strName);
        }

        public Player Player_add(string strPlayerName, Faction fac)
        {
            return this.m_campaignEngine.addPlayer(strPlayerName, fac);
        }

        public List<Player> Player_getPlayerList()
        {
            return m_campaignEngine.lisPlayers;
        }

        public List<clsUnitGroup> Player_getActiveUnitsForPlayer(Player player)
        {
            List<clsUnitGroup> unitsForPlayer = player.ListUnits;
            var lstUnitsCanAct = from u in player.ListUnits
                                 where !unitActedStack.Contains(u)
                                 select u;

            return new List<clsUnitGroup>(lstUnitsCanAct);
        }

        public void Player_endRound(Player p)
        {
            if (!lstFinishedPlayers.Contains(p))
                lstFinishedPlayers.Add(p);

            if (lstFinishedPlayers.Count() == m_campaignEngine.lisPlayers.Count())
            {
                Tick();
                lstFinishedPlayers.Clear();
                m_dictCommandCache.Clear();

            }

            // Neue Units die aus Ressourcen angelegt wurden registrieren
            while (m_campaignEngine.ResourceHandler.CreatedUnitIds.Count > 0)
            {
                string newUnitId = m_campaignEngine.ResourceHandler.CreatedUnitIds.Pop();
                clsUnitGroup unit = m_campaignEngine.getUnit(newUnitId);
                this.onTick += new delTick(unit.CampaignController_onTick);
            }

        }

        //, string strSpawnSektorKoord = "" --> zu devzwecken 
        public clsUnitGroup Unit_createNew(string strPlayerID, int intUnitTypeID, string strSpawnSektorKoord = "")
        {
            clsUnitGroup newUnit = this.m_campaignEngine.addUnit(strPlayerID, intUnitTypeID, strSpawnSektorKoord);

            this.onTick += new delTick(newUnit.CampaignController_onTick);

            return newUnit;
        }

        public clsUnitGroup Unit_getByID(string strUnitId)
		{
            return m_campaignEngine.getUnit(strUnitId);
		}

        public UnitInfo Unit_getInfoByID(string unitId)
        {
            clsUnitGroup unit = Unit_getByID(unitId);
            UnitInfo info = m_campaignEngine.getUnitInfo(unit);
            return info;
        }

        public clsCommandCollection Unit_getCommandsForUnit(clsUnitGroup unit)
        {
            clsCommandCollection objCommands;

            if (!unitCollisionStack.Contains(this.Unit_getSektorForUnit(unit)))
            {
                objCommands = this.m_campaignEngine.getCommandsForUnit(unit);
            }
            else
            {
                //Q&&D
                objCommands = new clsCommandCollection();

                objCommands.aktUnit = unit;

                objCommands.listRawCommands = new List<ICommand> { new comSolveKollision() }; 
                objCommands.listReadyCommands = new List<ICommand>();                      // Liste mit ausführbaren Commands - Enthalten zB explizite Position / Zielsektoren

            }


            foreach (ICommand cmd in objCommands.listReadyCommands)
            {
                m_dictCommandCache.Add(cmd.CommandId, cmd);
                cmd.onControllerEvent += new delControllerEvent(Command_onControllerEvent);
            }
            return objCommands;
        }

        public Sektor Unit_getSektorForUnit(clsUnitGroup unit)
        {
            return CampaignEngine.FieldField.getSektorForUnit(unit);
        }


        //FA 16.07.13 beschissene Q&D Lösung, Trennung Lib und Model zur Diskussion stellen
        public void Command_onControllerEvent(clsEventData objEventData)
        {
            if (objEventData.objCommand.GetType() == typeof(comPlaceUnit))
            {
                comPlaceUnit cmd = (comPlaceUnit)objEventData.objCommand;
                this.Unit_createNew(cmd.strNewOwner(), cmd.intNewUnitTypeID(), cmd.TargetSektor.objSektorKoord.uniqueIDstr());
            }
        }

        public ICommand Command_getByID(string commandId)
        {
            ICommand result = null;
            if (m_dictCommandCache.ContainsKey(commandId))
                result = m_dictCommandCache[commandId];
            return result;
        }


        public Sektor Sektor_getByID(string sektorId)
        {
            return m_campaignEngine.FieldField.dicSektors[sektorId];/*
            Sektor result = new Sektor();
            var query = from s in m_campaignEngine.FieldField.dicSektors.Values
                        where s.Id == sektorId
                        select s;
            if (query.Count() > 0)
                result = query.First<Sektor>();

            return result;*/
        }
        public List<Sektor> Sektor_getUnitCollisions()
        {
            return unitCollisionStack;
        }

        public Player Campaign_getStateForPlayer(string pID, string strState = "")
        {
            Player askingPlayer = this.m_campaignEngine.getPlayerByID(pID);

            if (askingPlayer !=null)
                this.m_campaignEngine.fillVisibleSektors(ref askingPlayer);

            return askingPlayer;


        }

        public CampaignInfo Campaign_getInfo()
        {
            CampaignInfo nfo = new CampaignInfo();
            nfo.campaignId = this.CampaignKey;
            nfo.campaignName = this.CampaignEngine.CampaignName;
            
            nfo.players = new Dictionary<string, string>();
            foreach (Player p in this.CampaignEngine.lisPlayers)
            {
                nfo.players.Add(p.Id, p.Playername);
            }

            
            return nfo;
        }

        #region Ressourcen Handling

            public List<ResourceInfo> Ressource_getRessourcesForPlayer(Player player)
        {
            List<ResourceInfo> result;
            List <ResourceInfo> resInfo = m_campaignEngine.ResourceHandler.getResourceInfo();

            result = (from p in resInfo
                      where p.ownerId == player.Id
                      select p as ResourceInfo).ToList<ResourceInfo>();

            return result;
        }
            public void Ressource_add(ResourceInfo resInfo)
        {
            string strResType = resInfo.resourceableType;
            Type resType = Type.GetType(strResType);
            Object typeObj = Activator.CreateInstance(resType);

            Player resourceOwner = (from p in CampaignEngine.lisPlayers
                                    where p.Id == resInfo.ownerId
                                    select p).First() as Player;


            CampaignEngine.ResourceHandler.addRessourcableObject(resourceOwner, (IResourceable)typeObj);
        }
            // Kapselt die Aufrufe von getResourceCommands im ResourceHandler um die gelieferten Commands 
        // im CommandCache zu speichern
            public List<ICommand> Ressource_getCommandsByID(string resourceId)
        {
            List<ICommand> lstCmds = this.CampaignEngine.ResourceHandler.getResourceCommands(resourceId);   
            foreach (ICommand cmd in lstCmds)
                m_dictCommandCache.Add(cmd.CommandId, cmd);

            return lstCmds;
        }

        #endregion


        public int UnitType_addNew(clsUnitType newUnit)
        {
            return clsUnitGroup.objUnitTypeFountain.addNewType(newUnit);
        }

        public clsUnitType UnitType_getTypeByName(string strUnitName)
        {
            return clsUnitGroup.objUnitTypeFountain.getTypeByName(strUnitName);
        }

        public clsUnitType UnitType_getTypeByID(int intUnitID)
        {
            if (clsUnitGroup.objUnitTypeFountain.dicUnitTypeData.ContainsKey(intUnitID.ToString()))
                return clsUnitGroup.objUnitTypeFountain.dicUnitTypeData[intUnitID.ToString()];
            else
                return null;
        }
    }

}
