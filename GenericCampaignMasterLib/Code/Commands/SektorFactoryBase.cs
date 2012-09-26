using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public abstract class clsSektorFactoryBase
    {
        public clsSektorFactoryBase(Field newFieldField)
        {
            this.m_FieldField = newFieldField;
            this.m_DirectionVektors = m_FieldField.getDirectionVectors();
        }

        protected List<clsSektorKoordinaten> m_DirectionVektors;
        protected Sektor m_originSektor;
        private Field m_FieldField = null;
        protected Field FieldField { get { return m_FieldField ;} } 
        

        public delegate void delNewStatus(string strStatus);
        public event delNewStatus onNewStatus;
        private void Status(string strStatus)
        {
            if (onNewStatus != null)
                onNewStatus(strStatus);
        }

    }
}
