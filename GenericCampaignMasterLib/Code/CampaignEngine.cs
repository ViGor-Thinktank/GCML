using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib.Code.Unit;

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
            List<ICommand> listRawCommands = u.getCommands();           // Unfertige Commands von der Unit - Enthalten keine Position-/Zielsektoren
            List<ICommand> listReadyCommands = new List<ICommand>();    // Liste mit vollständigen Commands - wird zurückgeliefert.
        	
			foreach (ICommand cmdRaw in listRawCommands)
			{
				if (cmdRaw.GetType() == typeof(Move))
				{
					Move cmdMove = (Move)cmdRaw;
					Sektor originSektor = getSektorContainingUnit(u);

					// Target Sektor ermitteln zu Testzwecken: Unit kann durch die Liste navigieren, wenn Ende erreicht wieder von vorne beginnen
					int intFieldsMoved = 0;
					int intPos = FieldField.ListSektors.IndexOf(originSektor);
					while (intFieldsMoved <= cmdMove.IntRange)
					{
						// Wenn über die Collectiongrenze rausgelaufen wird -> wieder am Anfang beginnen
						if (intFieldsMoved == FieldField.ListSektors.Count)
						{
							intPos = 0;
							continue;
						}
						
                        Move readyCmd = new Move();
                        readyCmd.Unit = u;
                        readyCmd.OriginSektor = originSektor;
                        readyCmd.TargetSektor = FieldField.ListSektors [intPos++];
                        readyCmd.IntRange = cmdMove.IntRange;
                        listReadyCommands.Add(readyCmd);

						intFieldsMoved++;
					}
				}
			}
			
			return listReadyCommands;
        }

        /// <summary>
        /// Erstmal zum Testen: Einheit liefert i.d.R. nur ein Command für Move.
        /// Liefert nur eine Collection aus möglichen Moves.
        /// </summary>
        public List<Move> getDefaultMoveCommandsForUnit(IUnit u)
        {
            List<Move> listMoves = new List<Move>();
            List<ICommand> cmds = getCommandsForUnit(u);
			foreach (ICommand c in cmds)
			{
                if (c.GetType() == typeof(Move))
                    listMoves.Add((Move)c);
			}

            return listMoves;
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
