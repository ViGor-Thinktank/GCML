using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

namespace GenericCampaignMasterLib
{
    public class CampaignState : Dictionary<string, object>
    {
        public CampaignState Save(CampaignEngine engine)
        {
            this ["players"] = engine.dicPlayers.Values.ToList();
            this ["sektors"] = engine.FieldField.getSektorList();
            this ["fielddimension"] = engine.FieldField.ListDimensions;
            this ["fieldtype"] = engine.FieldField.GetType().ToString();
            return this;
        }
        
        public CampaignEngine Restore()
        {
            List<Player> lstPlayers = (List <Player>)this ["players"];
            List<Sektor> lstSektors = (List <Sektor>)this ["sektors"];

            List<int> lstDim = (List <int>)this ["fielddimension"];
            string fieldTypeName = (string)this ["fieldtype"];
            Type fieldType = Type.GetType(fieldTypeName);
            Field f = (Field) Activator.CreateInstance(fieldType, new object[]{ lstDim });

            CampaignEngine engine = new CampaignEngine((Field)f);
            engine.setPlayerList(lstPlayers);

            return engine;
        }

    }
}
