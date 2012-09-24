using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
  
    public class clsUnit : IResourceable
    {
        public static clsUnitTypeCollection objUnitTypeFountain = new clsUnitTypeCollection();

        public clsUnit() { }

        public clsUnit(string unitId, int intUnitTypeID)
        {
            m_strId = unitId;
            m_objUnitType = objUnitTypeFountain.getUnitType(intUnitTypeID);
            m_strBezeichnung = UnitType.strBez + " " + unitId.ToString();
        }

        public clsUnit(string unitId, clsUnitType UnitType)
        {
            m_strId = unitId;
            m_objUnitType = UnitType;
            m_strBezeichnung = UnitType.strBez + " " + unitId.ToString();
        }

        public int intMovement { get { return m_objUnitType.intMovement; } }
        public int intSichtweite { get { return m_objUnitType.intSichtweite; } }

        public string strOwnerID = "";

        private string m_strId = "-1";
        public string Id 
        { 
            get { return m_strId; }
            set { m_strId = value; }
        }

        private string m_strBezeichnung = "";
        public string Bezeichnung
        {
            get { return m_strBezeichnung; }
            set { m_strBezeichnung = value; }
        }

        private clsUnitType m_objUnitType;
        public clsUnitType UnitType
        {
            get { return m_objUnitType; }
            set { m_objUnitType = value; }
        }

        public List<ICommand> getTypeCommands()
        {
            return m_objUnitType.getTypeCommands(this);
        }

        private Move m_aktCommand = null;

        public Move aktCommand
        {
            get { return m_aktCommand; }
            set { m_aktCommand = value; }
        }
        
        public string strAktCommandInfo
        {
            get
            {
                if (m_aktCommand != null)
                    return m_aktCommand.strInfo;
                else
                    return "";
            }
        }

        private List<ICommand> m_cmdCache;

        public List<ICommand> cmdCache
        {
            get { return m_cmdCache; }
            set { m_cmdCache = value; }
        }

        public void CampaignController_onTick()
        {
            if (this.aktCommand != null && !this.aktCommand.blnExecuted)
            {
                this.aktCommand.Execute();
            }
        }

        public List<ICommand> getResourceCommands(Player player)
        {
            List<ICommand> placeUnitCommands = new List<ICommand>();
            foreach (Sektor sektor in player.dicVisibleSectors.Values)
            {
                PlaceUnit cmd = new PlaceUnit();
                cmd.TargetSektor = sektor;
                cmd.UnitToPlace = this;
                placeUnitCommands.Add(cmd);
            }

            return placeUnitCommands.ToList<ICommand>();
        }
    }
}
