using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterModel;

namespace GenericCampaignMasterModel.Commands
{
    public abstract class clsFactoryBase
    {
        public Player actingPlayer = null;

        public abstract void go();

        public delegate void delNewStatus(string strStatus);
        public event delNewStatus onNewStatus;
        protected void Status(string strStatus)
        {
            if (onNewStatus != null)
                onNewStatus(strStatus);
        }

        public delegate void delNewCommand(ICommand readyCmd);
        public event delNewCommand onNewCommand;
        protected void raiseOnNewCommand(ICommand readyCmd)
        {
            if (onNewCommand != null)
                onNewCommand(readyCmd);
        }

    }

    public abstract class clsSektorFactoryBase : clsFactoryBase
    {
        public clsSektorFactoryBase(clsUnit Unit, Field FieldField)
        {
            this.m_Unit = Unit;
            this.m_FieldField = FieldField;
            this.m_DirectionVektors = m_FieldField.getDirectionVectors();
        }
        protected clsUnit m_Unit;
        protected List<clsSektorKoordinaten> m_DirectionVektors;
        protected Sektor m_originSektor;
        private Field m_FieldField = null;
        protected Field FieldField { get { return m_FieldField; } }

    }
}
