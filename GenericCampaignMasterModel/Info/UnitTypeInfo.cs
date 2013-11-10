using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel
{
    public class UnitTypeInfo
    {
        public int  ID { get; set; }
        public string Bezeichnung  { get; set; }
        public int Sichtweite { get; set; }
        public int Movement { get; set; }
        public string ClientData { get; set; }
        public string Description { get; set; }
        public int MaxResourceValue { get; set; }
        public int CreateValuePerRound { get; set; }
    }
}
