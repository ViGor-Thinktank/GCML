using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel.Commands
{
    public abstract class clsSektorFactoryBase : clsFactoryBase
    {
        public clsSektorFactoryBase(clsUnitGroup Unit, Field FieldField)
        {
            this.m_Unit = Unit;
            this.m_FieldField = FieldField;
            this.m_DirectionVektors = m_FieldField.getDirectionVectors();
        }
        protected clsUnitGroup m_Unit;
        protected List<clsSektorKoordinaten> m_DirectionVektors;
        protected Sektor m_originSektor;
        private Field m_FieldField = null;
        protected Field FieldField { get { return m_FieldField ;} } 
        
    }
}
