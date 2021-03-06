﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel.Commands
{
    public class comDestroyUnit : clsCommandBaseClass
    {
        public string UnitId { get; private set; }    // ID für erzeugte Unit == ResourceID, damit der Controller die neue Unit registrieren kann.

        private Player m_Owner = null;
        private Field m_FieldField = null;
        public comDestroyUnit(clsUnit Unit, Player owner, Field FieldField)
            : base("DestroyUnit")
        {   
            this.m_objUnitToCommand = Unit;
            this.m_Owner = owner;
            this.m_FieldField = FieldField;
        }

        public override void Execute() 
        {
            foreach (clsUnit aktU in this.m_Owner.ListUnits)
            {
                if (aktU.Id == m_objUnitToCommand.Id)
                {
                    this.m_Owner.ListUnits.Remove(aktU);
                    Sektor sek = this.m_FieldField.getSektorForUnit(aktU);
                    sek.removeUnit(aktU);
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
