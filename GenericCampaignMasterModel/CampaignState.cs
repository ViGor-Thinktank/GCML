using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Web.Script.Serialization;

namespace GenericCampaignMasterModel
{
    public class CampaignState : Dictionary<string, string>
    {
        public string CampaignName { get; set; }
        public string CampaignId { get; set; }
        public List<Player> ListPlayers { get; set; }
        public Dictionary<string, Sektor> DicSektors { get; set; }
        public clsSektorKoordinaten FieldDimension { get; set; }
        public string FieldType { get; set; }
        public List<UnitInfo> ListUnitInfo { get; set; }
        public clsUnitTypeCollection ListUnitTypes { get; set; }
        public List<ResourceInfo> ListResourceInfo { get; set; }

        private JavaScriptSerializer m_serializer = new JavaScriptSerializer();

        public List<Sektor> getListSektors()
        {
			return (List<Sektor>)m_serializer.Deserialize<List<Sektor>>(this["sektors"]);
        }

        public Player getPlayer(string strPlayerID)
        {

            List<Player> lisP = getListPlayers();
            
            var varPlayerList = from p in lisP
                         where p.Id == strPlayerID
                         select p;


            
            if (varPlayerList.Count() == 1)
                return varPlayerList.First();
            else if (varPlayerList.Count() > 1)
                throw new Exception("mehr als ein Treffer für PlayerID " + strPlayerID);
            else
                throw new Exception("kein Treffer für PlayerID " + strPlayerID);

        }

        public List<Player> getListPlayers()
        {
            List<Player> result = new List<Player>(); 
            List<Player> tmp = (List<Player>) m_serializer.Deserialize<List<Player>>(this["players"]);
            if (tmp != null)
                return tmp;
            return result;
        }
		
		public List<UnitInfo> getListUnitInfo()
		{	
			string strUnitInfo = this["unitinfo"];
			List<UnitInfo> lstUnitInfo = (List<UnitInfo>)m_serializer.Deserialize<List<UnitInfo>>(strUnitInfo);
			return lstUnitInfo;
		}

        public Dictionary<string, clsUnitType> getDicUnitTypeInfo()
        {
            string strUnitInfo = this.Keys.Contains("unittypesinfo") ? this["unittypesinfo"] : "";

            Dictionary<string, clsUnitType> dicUnitTypeInfo = (Dictionary<string, clsUnitType>)m_serializer.Deserialize<Dictionary<string, clsUnitType>>(strUnitInfo);
            return dicUnitTypeInfo;
        }

        public clsSektorKoordinaten getListDimensions()
        {
            return (clsSektorKoordinaten)m_serializer.Deserialize<clsSektorKoordinaten>(this["fielddimension"]);
        }

        public string getFieldtype()
        {
        
            return this["fieldtype"];
        }

        public List<ResourceInfo> getListResourceInfo()
        {
            List<ResourceInfo> lstResInfo;
            string strResInfo = this.Keys.Contains("resourceinfo") ? this["resourceinfo"] : "";

            if (String.IsNullOrEmpty(strResInfo))
                lstResInfo = new List<ResourceInfo>();
            else
                lstResInfo = (List<ResourceInfo>)m_serializer.Deserialize<List<ResourceInfo>>(strResInfo);
            
            return lstResInfo;
        }

        public void Save()
        {
            this["campaignname"] = this.CampaignName;
            this["campaignid"] = this.CampaignId;
            this["players"] = m_serializer.Serialize (this.ListPlayers);
            this["sektors"] = m_serializer.Serialize (this.DicSektors.Values);
            this["fielddimension"] = m_serializer.Serialize(this.FieldDimension);
            this["fieldtype"] = this.FieldType;
			this["unitinfo"] = m_serializer.Serialize(this.ListUnitInfo);
            this["unittypesinfo"] = m_serializer.Serialize(this.ListUnitTypes);
            this["resourceinfo"] = m_serializer.Serialize(this.ListResourceInfo);
        }

        public override string ToString()
        {
            Save();
            return m_serializer.Serialize(this);
        }

        public static CampaignState FromString(string stateStr)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CampaignState result = serializer.Deserialize<CampaignState>(stateStr);
            result.CampaignName = result["campaignname"];
            result.CampaignId = result["campaignid"];
            //result.ListPlayers = serializer.Deserialize<List<Player>>(result["players"]);
            //result.DicSektors = serializer.Deserialize<Dictionary<string, Sektor>>(result["sektors"]);
            //serializer.Deserialize<string>(result["fielddimension"]
            //serializer.Deserialize<string>(result["fieldtype"]
            //serializer.Deserialize<string>(result["unitinfo"]
            //serializer.Deserialize<string>(result["unittypesinfo"]
            //serializer.Deserialize<string>(result["resourceinfo"]
            return result;
        }

        
    }
}
