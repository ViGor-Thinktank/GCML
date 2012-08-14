using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class Move : ICommand
    {
        public BaseUnit Unit { get; set; }
        public Sektor TargetSektor { get; set; }
		public Sektor OriginSektor { get; set; }

        private bool m_blnExecuted = false;

        public bool blnExecuted { get { return m_blnExecuted; } }
        
        

        #region ICommand Member

        public void Execute ()
		{
            Register();
            m_blnExecuted = true;
			OriginSektor.removeUnit(this.Unit);
			TargetSektor.addUnit(this.Unit);
        }

        public void Register()
        {
            this.Unit.aktCommand = this;
        }

        #endregion


        public string strInfo
        {
            get
            {
                return this.OriginSektor.strUniqueID + " -> " + TargetSektor.strUniqueID;
            }
        }

        
    }
}
