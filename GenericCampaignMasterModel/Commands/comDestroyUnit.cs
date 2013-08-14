using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel.Commands
{
    public class comDestroyUnit : clsCommandBaseClass
    {
        public string UnitId { get; private set; }    // ID für erzeugte Unit == ResourceID, damit der Controller die neue Unit registrieren kann.

        private Player m_Owner = null;
        
        public comDestroyUnit(clsUnit Unit, Player owner)
            : base("DestroyUnit")
        {   
            this.m_objUnitToCommand = Unit;
            this.m_Owner = owner;
        }

        public override void Execute() 
        {
            foreach (clsUnit aktU in this.m_Owner.ListUnits)
            {
                if (aktU.Id == m_objUnitToCommand.Id)
                {
                    this.m_Owner.ListUnits.Remove(aktU);
                    break;
                }
            }
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
