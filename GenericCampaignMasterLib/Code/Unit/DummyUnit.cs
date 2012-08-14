using System;
using System.Collections.Generic;

namespace GenericCampaignMasterLib
{
	
	/// <summary>
	/// Testimplementierung für das IUnit Interface. Kann sich 1 Feld fortbewegen.
	/// </summary>
	public class DummyUnit : BaseUnit
	{
        public DummyUnit()
            : base("-1")
        {

        }

        public DummyUnit(string unitId) : base(unitId)
		{
            
		}

        
		public new List<ICommand> getCommands()
		{
            List<ICommand> lisPossibleCommands = base.getTypeCommands();
            //ggf. Unit- oder Situationspezifisches hinzufügen
            return lisPossibleCommands;

		}
		
	}
}

