using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel.Commands
{
    public class comConvertUnit : clsCommandBaseClass
    {
        private Player m_Owner = null;
        private Field m_FieldField = null;
        
        public comConvertUnit(clsUnit Unit, Player owner, Field FieldField)
            : base("ConvertUnit")
        {   
            this.m_objUnitToCommand = Unit;
            this.m_Owner = owner;
            this.m_FieldField = FieldField;
        }

        public override void Execute()
        {
         
            base.markExecute();
        }

        public new void Register()
        {

        }

        public override string strInfo
        {
            get
            {
                return "Destroy " + m_objUnitToCommand.strBez;
            }
        }

        public override clsFactoryBase getCommandFactory(clsUnit objUnit, Field FieldField)
        {
            return null;
        }

    }
}
