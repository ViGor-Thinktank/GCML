using System;
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
        
}
