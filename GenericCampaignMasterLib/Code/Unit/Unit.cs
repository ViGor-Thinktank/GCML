using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public interface IUnit
    {
        int Sichtweite { get; }             
        string Id { get; }             // Dummy
        string Bezeichnung { get; } // Dummy
		
		 List<ICommand> getTypeCommands();		// Liefert alle Aktionen, die von der Unit ausgeführt werden können. Unabhängig vom Kontext.

         int Movement { get; }
    }

  
    public class BaseUnit : IUnit
    {

        public BaseUnit(string unitId, UnitTypeBase UnitType, string strBezeichnung)
        {
            m_strId = unitId;
            m_objUnitType = UnitType;
        }

        public BaseUnit()
        {
            m_objUnitType = new UnitTypeDummy();
        }


        public BaseUnit(string unitId)
        {
            m_strId = unitId;
            m_objUnitType = new UnitTypeDummy();
            m_strBezeichnung = UnitType.strDefaultBez + " " + unitId.ToString();
        }

        public BaseUnit(string unitId, UnitTypeBase UnitType)
        {
            m_strId = unitId;
            m_objUnitType = UnitType;
            m_strBezeichnung = UnitType.strDefaultBez + " " + unitId.ToString();
        }

        public int Movement { get { return m_objUnitType.intMovement; } }
        public int Sichtweite { get { return m_objUnitType.intSichtweite; } }

        public string ownerID = "";

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

        private UnitTypeBase m_objUnitType;
        public UnitTypeBase UnitType
        {

            get { return m_objUnitType; }

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
    }
}
