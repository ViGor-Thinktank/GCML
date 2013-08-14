using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel.Commands
{
    class comCreateResource: clsCommandBaseClass 
    {
        private clsUnit m_objRecieveUnit
        {
            get
            {
                return base.m_objUnitToCommand;
            }
            set
            {
                base.m_objUnitToCommand = value;
            }        
        }

        public comCreateResource(clsUnit objRecieveUnit)
            : base("CreateResource")
        {
            this.m_objRecieveUnit = objRecieveUnit;
        }

        public override void Execute ()
		{
            base.markExecute();

            this.m_objRecieveUnit.addResourceValue(this.m_objRecieveUnit.intCreateValuePerRound);
            
        }

        public override string strInfo
        {
            get
            {
                return "cerate Resource Value" + this.m_objRecieveUnit.intCreateValuePerRound.ToString();
            }
        }

        public override clsFactoryBase getCommandFactory(clsUnit objUnit, Field FieldField)
        {
            return null;
        }
}
}
