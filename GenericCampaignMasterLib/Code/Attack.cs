using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class Attack : ICommand
    {
        public int Attacker
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int Target
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
