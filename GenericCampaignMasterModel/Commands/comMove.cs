using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel.Commands
{
    public class comMove : clsCommandBaseClass
    {
        
        public comMove(clsUnitGroup Unit) : base("Move")
        {
            this.m_objUnitToCommand = Unit;
        }

        public Sektor OriginSektor { get; set; }

        public override void Execute()
		{
            base.markExecute();
			OriginSektor.removeUnit(this.m_objUnitToCommand);
			TargetSektor.addUnit(this.m_objUnitToCommand);
        }


        public override clsFactoryBase getCommandFactory(clsUnitGroup objUnit, Field FieldField)
        {
            return new facMoveFactory(objUnit, FieldField);

        }
        public override string strInfo
        {
            get
            {
                return this.OriginSektor.strUniqueID + " -> " + TargetSektor.strUniqueID;
            }
        }

        
    }
}
