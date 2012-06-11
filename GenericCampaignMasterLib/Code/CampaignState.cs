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
    [Serializable()]
    public class CampaignState
    {
        Dictionary <string, object> state = null;

        public CampaignState Save(CampaignEngine engine)
        {
            state = new Dictionary<string, object>();
            state ["players"] = engine.dicPlayers.Values.ToList();
            state ["sektors"] = engine.FieldField.getSektorList();
            state ["fielddimension"] = engine.FieldField.ListDimensions;
            state ["fieldtype"] = engine.FieldField.GetType();
            return this;
        }
        
        public CampaignEngine Restore()
        {
            List<Player> lstPlayers = (List <Player>)state ["players"];
            List<Sektor> lstSektors = (List <Sektor>)state ["sektors"];

            List<int> lstDim = (List <int>)state ["fielddimension"];
            string fieldTypeName = (string)state ["fieldtype"];
            Type fieldType = Type.GetType(fieldTypeName);
            Field f = (Field) Activator.CreateInstance(fieldType, new object[]{ lstDim });

            CampaignEngine engine = new CampaignEngine((Field)f);
            engine.setPlayerList(lstPlayers);

            return engine;
        }

    }
}
