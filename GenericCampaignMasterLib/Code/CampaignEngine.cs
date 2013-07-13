using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GenericCampaignMasterModel;
using GenericCampaignMasterModel.Commands;

namespace GenericCampaignMasterLib
{
    public class CampaignEngine
    {
        public string CampaignName { get; set; }
        
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
            state.CampaignName = this.CampaignName;
            state.ListPlayers = this.ListPlayers;
            state.DicSektors = this.FieldField.dicSektors;
            state.FieldDimension = this.FieldField.FieldDimension;
            state.FieldType = this.FieldField.GetType().AssemblyQualifiedName;
            state.ListUnitInfo = this.getUnitInfo();
            state.ListUnitTypes = clsUnit.objUnitTypeFountain;
            state.ListResourceInfo = this.ResourceHandler.getResourceInfo();
            state.Save();
            return state;
        }

        public static CampaignEngine restoreFromState(CampaignState state)
        {
            List<Player> lstPlayers = state.getListPlayers();
            List<Sektor> lstSektors = state.getListSektors();

            // Feld erstellen;
            //Type fieldType = Type.GetType(state.getFieldtype());  // Todo: GetType funktioniert nicht obwohl GenericCampaignMasterModel.Field korrekt ist
            Type fieldType = typeof(GenericCampaignMasterModel.Field);      
           
            
            clsSektorKoordinaten objSekKoord = state.getListDimensions();
            Field field = (Field)Activator.CreateInstance(fieldType, new object[] { objSekKoord });
            field.setSektorList(lstSektors);

            // Engine erstellen
            CampaignEngine engine = new CampaignEngine((Field)field);
            engine.CampaignName = state.ContainsKey("campaignname") ? state["campaignname"] : "OLDCAMPAIGN";
            engine.setPlayerList(lstPlayers);

            clsUnit.objUnitTypeFountain.dicUnitTypeData = state.getDicUnitTypeInfo();
            
            // Units platzieren
            foreach (UnitInfo uInfo in state.getListUnitInfo())
            {
                Type unitType = Type.GetType(uInfo.unitType);
                //UnitTypeBase newUnitType = (UnitTypeBase)Activator.CreateInstance(unitType);

                Player aktP = (from p in lstPlayers
                               where p.Id == uInfo.playerId
                               select p).First();

                clsUnit unit = aktP.getUnitByID(uInfo.unitId);

                //clsUnit unit = new clsUnit(uInfo.unitId);
                field.dicSektors[uInfo.sektorId].addUnit(unit);
            }

            // Ressourcehandler erzeugen und Ressourcen wiederherstellen
            ResourceHandler resHandler = new ResourceHandler();
            engine.ResourceHandler = resHandler;
            foreach (ResourceInfo resInfo in state.getListResourceInfo())
            {
                string strResType = resInfo.resourceableType;
                Type resType = Type.GetType(strResType);
                Object typeObj = Activator.CreateInstance(resType);

                Player resourceOwner = (from p in lstPlayers
                                        where p.Id == resInfo.ownerId
                                        select p).First() as Player;

                resHandler.addRessourcableObject(resourceOwner, (IResourceable)typeObj);
            }

            return engine;
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


        private ResourceHandler m_resHandler = new ResourceHandler();
        public ResourceHandler ResourceHandler
        {
            get { return m_resHandler; }
            set { m_resHandler = value; }
        }
        #endregion

    
       
        public clsCommandCollection getCommandsForUnit(clsUnit objAktUnit)
		{
            clsCommandCollection objCommandCol = new clsCommandCollection();

            objCommandCol.aktUnit = objAktUnit;

            objCommandCol.listRawCommands = objAktUnit.getTypeCommands();                // Unfertige Commands von der Unit - Enthalten zB keine Position-/Zielsektoren
            objCommandCol.listReadyCommands = new List<ICommand>();                      // Liste mit ausführbaren Commands - Enthalten zB explizite Position / Zielsektoren

            objCommandCol.onStatus += new Field.delStatus(General_onNewStatus);

            foreach (ICommand cmdRaw in objCommandCol.listRawCommands)
			{
                clsFactoryBase objCommandFactory = cmdRaw.getCommandFactory(objAktUnit, FieldField);
                if (objCommandFactory != null)
                {
                    objCommandCol.useFactory(objCommandFactory);
                }
                else
                {
                    cmdRaw.CommandId = Guid.NewGuid().ToString();
                    objCommandCol.listReadyCommands.Add(cmdRaw);
                }
            }
			
			return objCommandCol;
        }

        public event Field.delStatus onEngineStatus;

        void General_onNewStatus(string strStatus)
        {
            if (this.onEngineStatus != null)
                this.onEngineStatus(strStatus);
        }
   
     
        public Sektor getSektorContainingUnit(clsUnit u)
        {
            return FieldField.getSektorForUnit(u);
            
        }

        public List<UnitInfo> getUnitInfo()
        {
            List<UnitInfo> result = new List<UnitInfo>();
            foreach(Player p in ListPlayers)
            {
                foreach (clsUnit u in p.ListUnits)
                {
                    UnitInfo info = getUnitInfo(u);
                    result.Add(info);
                }
            }

            return result;
        }

        public UnitInfo getUnitInfo(clsUnit unit)
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


        public Player getUnitOwner(clsUnit unit)
        {

            var owner = (from p in m_ListPlayers
                         where p.Id == unit.strOwnerID 
                         select p).First();

            return owner as Player;
        }

        public clsUnit getUnit(string id)
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

        public clsUnit addUnit(Player owner, clsUnit newUnit, Sektor sektor)
        {
            newUnit.strOwnerID = owner.Id;
            owner.ListUnits.Add(newUnit);
            sektor.addUnit(newUnit);
            return newUnit;
        }


        internal clsUnit addUnit(string strPlayerID, int intUnitTypeID)
        {
             clsUnit newUnit = null;
            //Ergibt eineindeutige UnitIDs
            newUnit = new clsUnit(strPlayerID + getPlayerByID(strPlayerID).ListUnits.Count.ToString(), intUnitTypeID);
            newUnit.strOwnerID = strPlayerID;
            clsSektorKoordinaten insertSek = (getPlayerByID(strPlayerID).unitspawnSektor != null ? getPlayerByID(strPlayerID).unitspawnSektor.objSektorKoord : this.FieldField.nullSektorKoord);
            return addUnit(strPlayerID, newUnit, insertSek);
        }

        public Player getPlayerByName(string strName)
        {

            foreach (Player aktP in m_ListPlayers)
            {
                if (aktP.Playername == strName)
                    return aktP;
            }
            return null;

        }
        public Player getPlayerByID(string playerId)
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

        public clsUnit addUnit(string strPlayerID, clsUnit newUnit, clsSektorKoordinaten objSektorKoord)
        {

            newUnit.strOwnerID = strPlayerID;
            getPlayerByID(strPlayerID).ListUnits.Add(newUnit);
            this.FieldField.get(objSektorKoord).ListUnits.Add(newUnit);

            return newUnit;
        }

 
        #endregion

        #region Player

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

            objNewPlayer.accessibleSectors = getAccessibleSektorsForPlayer(objNewPlayer);
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

            foreach (clsUnit aktUnit in p.ListUnits)
            {
                facViewSek.getVisibleSektorsFromUnitSektor(this.getSektorContainingUnit(aktUnit), aktUnit.intSichtweite);
            }

            p.dicVisibleSectors = facViewSek.ListVisibleSektors;
        }

        public List<Sektor> getAccessibleSektorsForPlayer(Player p)
        {
            List<Sektor> result = new List<Sektor>();
            var qsek = from s in this.FieldField.getSektorList()
                       where s.ListUnits.Count() == 0
                       select s;

            if (qsek.Count() > 0)
                result = qsek.ToList<Sektor>();

            return result;
        }
#endregion
    }
}
