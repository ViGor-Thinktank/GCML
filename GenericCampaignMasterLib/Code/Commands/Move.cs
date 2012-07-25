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

        #region ICommand Member

        public void Execute ()
		{
			OriginSektor.removeUnit(this.Unit);
			TargetSektor.addUnit(this.Unit);
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
