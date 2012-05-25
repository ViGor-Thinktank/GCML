using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    [Serializable()]
    public class Player
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

        public Ressourcen Ressourcen
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }
}
