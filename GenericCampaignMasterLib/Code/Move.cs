using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class Move : ICommand
    {
        public int SektorRange { get; set; }
		public Field CField { get; set; }
        public IUnit CUnit { get; set; }
        public Sektor TargetSektor { get; set; }
        
        #region ICommand Member

        public void Execute()
        {
            CField.putUnit(CUnit, TargetSektor);
        }

        #endregion
    }
}
