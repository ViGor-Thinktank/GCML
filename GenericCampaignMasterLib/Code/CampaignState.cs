using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Web.Script.Serialization;

namespace GenericCampaignMasterLib
{
    public struct UnitInfo
    {
        public string playerId;
        public string sektorId;
        public string unitId;
    }

    public class CampaignState : Dictionary<string, string>
    {

        private JavaScriptSerializer m_serializer = new JavaScriptSerializer();

        public List<Sektor> getListSektors()
        {
            return (List<Sektor>) m_serializer.Deserialize(this["sektors"], typeof(List<Sektor>));
        }

        public List<Player> getListPlayers()
        {

            string p = this["players"];
            return (List<Player>)m_serializer.Deserialize(this["players"], typeof(List<Player>));
        }

        public List<int> getListDimensions()
        {
            return (List<int>)m_serializer.Deserialize(this["fielddimension"], typeof(List<int>));
        }

        public string getFieldtype()
        {
        
            return this["fieldtype"];
        }

        public CampaignState Save(CampaignEngine engine)
        {
            this["players"] = m_serializer.Serialize (engine.ListPlayers.Values);
            this["sektors"] = m_serializer.Serialize (engine.FieldField.ListSektors.Values);
            this["fielddimension"] = m_serializer.Serialize(engine.FieldField.ListDimensions);
            this["fieldtype"] = engine.FieldField.GetType().ToString();
            return this;
        }
        
        public CampaignEngine Restore()
        {
            List<Player> lstPlayers = getListPlayers();
            List<Sektor> lstSektors = getListSektors();

            List<int> lstDim = getListDimensions();

            Type fieldType = Type.GetType(getFieldtype());
            Field f = (Field) Activator.CreateInstance(fieldType, new object[]{ lstDim });

            CampaignEngine engine = new CampaignEngine((Field)f);
            engine.setPlayerList(lstPlayers);

            return engine;
        }

        public override string ToString()
        {
            return base.ToString();
        }

    }
}
