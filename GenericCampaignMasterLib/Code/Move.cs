using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib.Code.Unit;

namespace GenericCampaignMasterLib
{
    public class Move : ICommand
    {
        public int IntRange { get; set; }
		public IUnit Unit { get; set; }
        public Sektor TargetSektor { get; set; }
		public Sektor OriginSektor { get; set; }
        
        #region ICommand Member

        public void Execute ()
		{
			OriginSektor.removeUnit(this.Unit);
			TargetSektor.addUnit(this.Unit);

            int i = 0;
        }

        #endregion
    }
}
