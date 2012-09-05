using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class Move : ICommand
    {
        public clsUnit Unit { get; set; }
        public Sektor TargetSektor { get; set; }
		public Sektor OriginSektor { get; set; }

        private bool m_blnExecuted = false;

        public bool blnExecuted { get { return m_blnExecuted; } }

        #region ICommand Member
        public string CommandId { get; set; }

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

        public CommandInfo getInfo()
        {
            CommandInfo nfo = new CommandInfo();
            nfo.commandId = this.CommandId;
            nfo.actingUnitId = this.Unit.Id;
            nfo.commandType = this.GetType().ToString();
            nfo.strInfo = this.strInfo;
            nfo.isActive = (this.Unit.aktCommand == this) ? true : false;
            return nfo;
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
