using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    [Serializable()]
    public class CampaignEngine
    {
        public List<Player> CPlayers { get; set; }
        public Field CField { get; set; }

		// Todo: Methode soll nur die Units zurückliefern die aktivierbar sind
		// Auf die Liste aller Units des Players kann über die List-Property zugegriffen werden.
        public List<IUnit> getActiveUnitsForPlayer(Player p)
        {
			return p.ListUnits;
        }

        public List<ICommand> getCommandsForUnit(IUnit u)
        {
            return u.getCommands();
        }

        public List<Sektor> getViewableSectorsForUnit(IUnit u)
        {
            List<Sektor> sektorList = CField.SektorList;
            return sektorList;
        }
    }
}
