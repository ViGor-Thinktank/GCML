using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    [Serializable()]
    public class CampaignEngine
    {
        private int listUnitsActed;

        public List<Player> CPlayers { get; set; }
        public Field CField { get; set; }

        public List<IUnit> getUnitsForPlayer(Player p)
        {
            throw new System.NotImplementedException();
        }

        public List<ICommand> getCommandsForUnit(IUnit u)
        {
            throw new System.NotImplementedException();
        }

        public List<Sektor> getViewableSectors(IUnit u)
        {
            throw new System.NotImplementedException();
        }
    }
}
