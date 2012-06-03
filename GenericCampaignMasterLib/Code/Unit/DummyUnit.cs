using System;
using System.Collections.Generic;

namespace GenericCampaignMasterLib.Code.Unit
{
	
	/// <summary>
	/// Testimplementierung für das IUnit Interface. Kann sich 1 Feld fortbewegen.
	/// </summary>
	public class DummyUnit : BaseUnit
	{
        public DummyUnit(int unitId) : base(unitId, new UnitTypeDummy())
		{
            intMovementRate = 1;
		}

        
		public new List<ICommand> getCommands()
		{
            List<ICommand> lisPossibleCommands = base.getCommands();
            //ggf. Unit- oder Situationspezifisches hinzufügen
            return lisPossibleCommands;

		}
		
	}
}

