using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class CampaignEngine
    {
        private int listUnitsActed;
    
        public void initNewGame()
        {
            throw new System.NotImplementedException();
        }

        public List<Unit> getUnitsForPlayer(Player p)
        {
            throw new System.NotImplementedException();
        }

        public List<ICommand> getCommandsForUnit(Unit u)
        {
            throw new System.NotImplementedException();
        }

        public CampaignState SaveState()
        {
            throw new System.NotImplementedException();
        }
		
        public void RestoreState(CampaignState state)
        {
            throw new System.NotImplementedException();
        }

        public List<Sektor> getViewableSectors(Unit u)
        {
            throw new System.NotImplementedException();
        }
    }
}
