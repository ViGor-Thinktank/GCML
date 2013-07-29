using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel.Commands
{
    public class comDropResource : clsCommandBaseClass 
    {
        private clsUnitGroup m_objDeliverUnit
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
        private clsUnitGroup m_objRecieveUnit { get; set; }

        private int m_intValue { get { return this.m_objDeliverUnit.intResourceValue; } }

        public comDropResource(clsUnitGroup objDeliverUnit, clsUnitGroup objRecieveUnit) : base("DropResource")
        {
            this.m_objDeliverUnit = objDeliverUnit;
            this.m_objRecieveUnit = objRecieveUnit;
        }

        public override void Execute ()
		{
            base.markExecute();

            this.m_objDeliverUnit.intResourceValue = this.m_objRecieveUnit.addResourceValue(this.m_objDeliverUnit.intResourceValue);
        }

        public override string strInfo
        {
            get
            {
                return "Resource Value" + this.m_intValue.ToString();
            }
        }

        public override clsFactoryBase getCommandFactory(clsUnitGroup objUnit, Field FieldField)
        {
            return new facDropResourceFactory(objUnit, FieldField);
        }
    }
}
