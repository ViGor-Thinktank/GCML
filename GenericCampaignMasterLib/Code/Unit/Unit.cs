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
		
		 List<ICommand> getCommands();		// Liefert alle Aktionen, die von der Unit ausgeführt werden können. Unabhängig vom Kontext.

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



        public List<ICommand> getCommands()
        {
            return m_objUnitType.getCommands(this);
        }




    }
}
