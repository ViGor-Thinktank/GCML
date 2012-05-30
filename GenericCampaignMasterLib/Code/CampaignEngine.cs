using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    [Serializable()]
    public class CampaignEngine
    {
        public List<Player> ListPlayers { get; set; }
        public Field FieldField { get; set; }

		// Todo: Methode soll nur die Units zurückliefern die aktivierbar sind
		// Auf die Liste aller Units des Players kann über die List-Property zugegriffen werden.
        public List<IUnit> getActiveUnitsForPlayer(Player p)
        {
			return p.ListUnits;
        }

        public List<ICommand> getCommandsForUnit(IUnit u)
		{
			List<ICommand> listCommands = u.getCommands();
			foreach (ICommand cmd in listCommands)
			{
				if (cmd.GetType() == typeof(Move))
				{
					Move cmdMove = (Move)cmd;
					cmdMove.OriginSektor = getSektorContainingUnit(u);
					
					// Target Sektor ermitteln zu Testzwecken: Unit kann durch die Liste navigieren, wenn Ende erreicht wieder von vorne beginnen
					int intFieldsMoved = 0;
					int intPos = FieldField.ListSektors.IndexOf(cmdMove.OriginSektor);
					Sektor targetSektor = null;
					while (intFieldsMoved <= cmdMove.IntRange)
					{
						// Wenn über die Collectiongrenze rausgelaufen wird -> wieder am Anfang beginnen
						if (intFieldsMoved == FieldField.ListSektors.Count)
						{
							intPos = 0;
							continue;
						}
						
						targetSektor = FieldField.ListSektors [intPos++];
						
						intFieldsMoved++;
					}
					
					cmdMove.TargetSektor = targetSektor;
				}
			}
			
			return listCommands;
        }

        public List<Sektor> getViewableSectorsForUnit(IUnit u)
        {
            List<Sektor> sektorList = FieldField.ListSektors;
            return sektorList;
        }

        public Sektor getSektorContainingUnit(IUnit u)
        {
            // Erstmal so gelöst. Geht besser.      
            foreach (Sektor s in FieldField.ListSektors)
            {
                if (s.ListUnits.Contains(u))
                    return s;
            }

            return null;
        }	

    }
}
