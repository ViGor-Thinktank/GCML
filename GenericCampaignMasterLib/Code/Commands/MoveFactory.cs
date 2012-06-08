using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class clsMoveFactory
    {
        private Field m_FieldField = null;
        private IUnit m_Unit;
        private List<Field.clsSektorKoordinaten> m_MoveVektors;
        private Sektor m_originSektor;

        private List<string> m_listKnownMovements = new List<string>();

        public delegate void delNewMoveCommand(Move readyCmd);
        public event delNewMoveCommand onNewMoveCommand;
        private void raiseOnNewMoveCommand(Move readyCmd)
        {
            if (onNewMoveCommand != null)
                onNewMoveCommand(readyCmd);
        }

        public delegate void delNewStatus(string strStatus);
        public event delNewStatus onNewStatus;
        private void Status(string strStatus)
        {
            if (onNewStatus != null)
                onNewStatus(strStatus);
        }



        public clsMoveFactory(IUnit u, Field FieldField)
        {
            
            //set Members
            m_Unit = u;
            m_FieldField = FieldField;

            //Init Stuff
            m_originSektor = FieldField.getSektorForUnit(u);
            m_MoveVektors = FieldField.getDirectionVectors();
            m_listKnownMovements.Add(m_originSektor.strUniqueID);

            
        }

        private void createMoveCommandsForSektor(Sektor aktSek, int intFieldsMoved)
        {
            foreach (Field.clsSektorKoordinaten aktKoord in m_MoveVektors)
            {
                Sektor newSek = m_FieldField.move(aktSek, aktKoord);

                if (newSek != null && aktSek.strUniqueID != newSek.strUniqueID)
                {
                    string strStatus = aktSek.strUniqueID + "->" + newSek.strUniqueID;
                    
                    if (!m_listKnownMovements.Contains(newSek.strUniqueID))
                    {
                        m_listKnownMovements.Add(newSek.strUniqueID);

                        Move readyCmd = new Move();
                        readyCmd.Unit = m_Unit;
                        readyCmd.strCreate = strStatus;
                        readyCmd.OriginSektor = m_originSektor;
                        Sektor targetSek = m_FieldField.get(newSek.objSektorKoord);
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
            this.createMoveCommandsForSektor(m_FieldField.get(m_originSektor.objSektorKoord), 0);
        }
    }
}
