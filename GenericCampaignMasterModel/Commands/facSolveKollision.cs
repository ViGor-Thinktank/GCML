using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel.Commands
{
    public class facSolveKollision : clsSektorFactoryBase
    {
        public facSolveKollision(clsUnit u, Field FieldField)
            : base(u, FieldField)
        {
            
            //set Members
            m_Unit = u;
            
            //Init Stuff
            m_originSektor = FieldField.getSektorForUnit(u);
            
        }

        private void createSolveCommands()
        {
            comMove readyCmd = new comMove(m_Unit);
            readyCmd.OriginSektor = m_originSektor;
            Sektor targetSek = this.FieldField.dicSektors[m_Unit.MoveHistory_lastSektorID];
            readyCmd.TargetSektor = targetSek;
            this.raiseOnNewCommand(readyCmd);

            comDestroyUnit desCmd = new comDestroyUnit(m_Unit, this.actingPlayer);
            this.raiseOnNewCommand(desCmd);
        }
       
        public override void go()
        {
            this.createSolveCommands();       
        }

    }
}
