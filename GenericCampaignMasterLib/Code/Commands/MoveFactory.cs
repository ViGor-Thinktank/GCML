using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GenericCampaignMasterLib
{
    public class clsMoveFactory : clsSektorFactoryBase
    {
        private clsUnit m_Unit;
        
        private List<string> m_listKnownMovements = new List<string>();

        public delegate void delNewMoveCommand(Move readyCmd);
        public event delNewMoveCommand onNewMoveCommand;
        private void raiseOnNewMoveCommand(Move readyCmd)
        {
            if (onNewMoveCommand != null)
                onNewMoveCommand(readyCmd);
        }

        public clsMoveFactory(clsUnit u, Field FieldField) : base(FieldField)
        {
            
            //set Members
            m_Unit = u;
            

            //Init Stuff
            m_originSektor = FieldField.getSektorForUnit(u);
            m_listKnownMovements.Add(m_originSektor.strUniqueID);

            
        }

        private void createMoveCommandsForSektor(Sektor aktSek, int intFieldsMoved)
        {
            foreach (clsSektorKoordinaten aktVektor in m_DirectionVektors)
            {
                Sektor newSek = this.FieldField.move(aktSek, aktVektor);

                if (newSek != null && aktSek.strUniqueID != newSek.strUniqueID)
                {
                    
                    if (!m_listKnownMovements.Contains(newSek.strUniqueID))
                    {
                        m_listKnownMovements.Add(newSek.strUniqueID);

                        Move readyCmd = new Move();
                        readyCmd.Unit = m_Unit;
                        readyCmd.CommandId = Guid.NewGuid().ToString();
                        
                        readyCmd.OriginSektor = m_originSektor;
                        
                        Sektor targetSek = this.FieldField.get(newSek.objSektorKoord);
                        
                        readyCmd.TargetSektor = targetSek;
                        
                        raiseOnNewMoveCommand(readyCmd);
                    }

                    int intNewFieldsMoved = intFieldsMoved + newSek.intMoveCost;
                    
                    if (intNewFieldsMoved < m_Unit.intMovement)
                    {
                        createMoveCommandsForSektor(newSek, intNewFieldsMoved);
                    }
                }

            }
                
            
        }


        internal void go()
        {
            //go
            this.createMoveCommandsForSektor(this.FieldField.get(m_originSektor.objSektorKoord), 0);
        }
    }
}
