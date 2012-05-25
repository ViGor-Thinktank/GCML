using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    [Serializable()]
    public class Player
    {
        public List<IUnit> ListUnits
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
