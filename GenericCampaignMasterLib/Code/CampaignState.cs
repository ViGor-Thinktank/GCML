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
        public Dictionary<string, Sektor> ListSektors
        {
            get
            {
                return (Dictionary<string, Sektor>)this["sektors"];
            }
            set
            {
                this["sektors"] = value;
            }
        }

        public Dictionary<int, Player> ListPlayers
        {
            get
            {
                return (Dictionary<int, Player>)this["players"];
            }
            set
            {
                this["players"] = value;
            }
        }

        public List<int> ListDimensions
        {
            get
            {
                return null;
                
            }
            set
            {
                this["fielddimension"] = value;
            }
        }

        public string strFieldtype
        {
            get
            {
                return (string)this["fieldtype"];
            }
            set 
            {
                this["fieldtype"] = value;
            }
        }

        public CampaignState Save(CampaignEngine engine)
        {
            this.ListPlayers = engine.ListPlayers;
            this.ListSektors = engine.FieldField.ListSektors;
            this.ListDimensions = engine.FieldField.ListDimensions;
            this.strFieldtype = engine.FieldField.GetType().ToString();
            return this;
        }
        
        public CampaignEngine Restore()
        {
            List<Player> lstPlayers = this.ListPlayers.Values.ToList();
            List<Sektor> lstSektors = this.ListSektors.Values.ToList();

            List<int> lstDim = this.ListDimensions;


            Type fieldType = Type.GetType(this.strFieldtype);
            Field f = (Field) Activator.CreateInstance(fieldType, new object[]{ lstDim });

            CampaignEngine engine = new CampaignEngine((Field)f);
            engine.setPlayerList(lstPlayers);

            return engine;
        }

    }
}
