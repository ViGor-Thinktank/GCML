using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib.Code.Unit;
using GenericCampaignMasterLib.Code;

namespace GenericCampaignMasterLib
{
    [Serializable()]
    public class CampaignEngine
    {
     
        #region " Properties && Felder "
        private Dictionary<int, Player> m_Players = null;
        public Dictionary<int, Player> dicPlayers
        {
            get
            {
                return m_Players;
            }
        }

        private Field m_FieldField;

        public Field FieldField
        {
            get { return m_FieldField; }
            set { m_FieldField = value; }
        }

        #endregion

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

					Sektor aktSek = FieldField.get(originSektor.objSektorKoord);

                    //while (intFieldsMoved <= cmdMove.IntRange)
                    List<Field.clsSektorKoordinaten> MoveVektors = FieldField.getMoveVektors(u.intMovement);
                    foreach (Field.clsSektorKoordinaten aktKoord in MoveVektors)
                    {
                        Sektor newSek = FieldField.move(aktSek, aktKoord); 

						// Wenn über die Collectiongrenze rausgelaufen wird -> wieder am Anfang beginnen
                        if (newSek == null || newSek.strUniqueID == aktSek.strUniqueID)
						{                            
                            //newSek = FieldField.get("1|1");
							continue;
						}
						
                        Move readyCmd = new Move();
                        readyCmd.Unit = u;
                        readyCmd.OriginSektor = originSektor;
                        readyCmd.TargetSektor = FieldField.get(newSek.objSektorKoord);
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
            /*List<Sektor> sektorList = FieldField.objSektorCollection.getSektorList();
            return sektorList;
*/
            return null;
        }

        public Sektor getSektorContainingUnit(IUnit u)
        {
            return FieldField.getSektorForUnit(u);
            
        }
        #region " Uuitfaktory "
        
        public void addUnit(int intPlayerID, IUnit newUnit)
        {
            m_Players[intPlayerID].ListUnits.Add(newUnit);
        }


        #endregion
        public Player addPlayer(Player objNewPlayer)
        {
            if (m_Players == null) { m_Players = new Dictionary<int, Player>(); }

            if (m_Players.ContainsKey(objNewPlayer.Id))
            {
                throw new Exception_Engine_Player("PlayerID ist bereits vergeben!");
            }
            
            m_Players.Add(objNewPlayer.Id, objNewPlayer);
            
            return objNewPlayer;
        }

        public void setPlayerList(List<Player> list)
        {
            foreach (Player aktPlayer in list)
            {
                m_Players.Add(aktPlayer.Id, aktPlayer);
            }
        }


        //TMP Bastelkram
        private List<string> HandlerList = new List<string>();
        public void testHandler(object sender, EventArgs e)
        {
            HandlerList.Add(((Sektor)sender).Id);
        }

        
    }
}
