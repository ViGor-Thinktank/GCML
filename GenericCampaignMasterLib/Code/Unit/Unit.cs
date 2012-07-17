using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public interface IUnit
    {
        int intSichtweite { get; }             
         int Id { get; }             // Dummy
        string Bezeichnung { get; set; } // Dummy
		UnitTypeBase UnitType { get; }
		
		 List<ICommand> getCommands();		// Liefert alle Aktionen, die von der Unit ausgeführt werden können. Unabhängig vom Kontext.

         int intMovement { get; }
    }
	
	
    public class BaseUnit : IUnit
    {

        public BaseUnit(int unitId, UnitTypeBase UnitType, string strBezeichnung)
        {
            m_intId = unitId;
            m_objUnitType = UnitType;
        }
        
        public BaseUnit(int unitId, UnitTypeBase UnitType)
        {
            m_intId = unitId;
            m_objUnitType = UnitType;
            m_strBezeichnung = UnitType.strDefaultBez + " " + unitId.ToString();
        }

        public int intMovement { get { return m_objUnitType.intMovement; } }
        public int intSichtweite { get { return m_objUnitType.intSichtweite; } }


        private int m_intId = -1;
        public int Id { get { return m_intId; } }

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
