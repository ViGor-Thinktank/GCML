﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel.Commands
{
    public class comSolveKollision : clsCommandBaseClass
    {
        public comSolveKollision()
            : base("SolveKollision")
        {
          
        }


        public override void Execute()
        {
            base.markExecute();

        }

        public override string strInfo
        {
            get
            {
                return "SolveKollision";
            }
        }

        public override clsFactoryBase getCommandFactory(clsUnit objUnit, Field FieldField)
        {
            return new facSolveKollision(objUnit, FieldField);

        }
    }

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

            readyCmd = new comMove(m_Unit);
            readyCmd.OriginSektor = m_originSektor;
            targetSek = m_originSektor;
            readyCmd.TargetSektor = targetSek;
            this.raiseOnNewCommand(readyCmd);
        }

        public override void go()
        {
            this.createSolveCommands();
        }

    }
        
}
