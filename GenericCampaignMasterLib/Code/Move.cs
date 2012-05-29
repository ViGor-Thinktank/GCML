using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class Move : ICommand
    {
		public int Fields { get; set; }
        public IUnit Unit { get; set; }
        public Sektor TargetField { get; set; }
        
        #region ICommand Member

        public void Execute()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
