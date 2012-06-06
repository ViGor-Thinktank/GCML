using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib.Code
{
    public class clsMoveFactory
    {
        Field m_FieldField = null;
        Unit.IUnit m_Unit;
        List<Field.clsSektorKoordinaten> m_MoveVektors;
        Sektor m_originSektor;

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



        public clsMoveFactory(Unit.IUnit u, Field FieldField)
        {
            
            //set Members
            m_Unit = u;
            m_FieldField = FieldField;

            //Init Stuff
            m_originSektor = FieldField.getSektorForUnit(u);
            m_MoveVektors = FieldField.getMoveVektors();
            
            
            
        }

        private void createMoveCommandsForSektor(Sektor aktSek, int intFieldsMoved)
        {
            Status("Enter createMoveCommandsForSektor " + aktSek.strUniqueID + " " + intFieldsMoved.ToString());
            foreach (Field.clsSektorKoordinaten aktKoord in m_MoveVektors)
            {
                Sektor newSek = m_FieldField.move(aktSek, aktKoord);

                // Wenn über die Collectiongrenze rausgelaufen wird -> wieder am Anfang beginnen
                if (newSek == null || newSek.strUniqueID == aktSek.strUniqueID)
                {
                    //newSek = FieldField.get("1|1");
                    continue;
                }

                Status(aktSek.strUniqueID + ": make Command " + newSek.strUniqueID);
                    
                Move readyCmd = new Move();
                readyCmd.Unit = m_Unit;
                readyCmd.OriginSektor = m_originSektor;
                readyCmd.TargetSektor = m_FieldField.get(newSek.objSektorKoord);
                raiseOnNewMoveCommand(readyCmd);

                intFieldsMoved += readyCmd.TargetSektor.intMoveCost;
                if (intFieldsMoved  < m_Unit.intMovement)
                {
                    createMoveCommandsForSektor(newSek, intFieldsMoved);
                }
            }
                
            intFieldsMoved++;
        }


        internal void go()
        {
            //go
            this.createMoveCommandsForSektor(m_FieldField.get(m_originSektor.objSektorKoord), 0);
        }
    }
}
