using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    [Serializable()]
    public interface IUnit
    {
        public int Id { get; set; }             // Dummy
        public string Bezeichnung { get; set; } // Dummy
    }
}
