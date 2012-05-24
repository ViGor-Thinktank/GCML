using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class Move : ICommand
    {
        public int Unit
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int TargetField
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
