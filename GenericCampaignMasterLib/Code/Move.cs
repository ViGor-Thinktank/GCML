using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class Move : ICommand
    {
        public IUnit Unit
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public Sektor TargetField
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
        #region ICommand Member

        public void Execute()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
