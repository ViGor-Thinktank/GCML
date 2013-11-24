using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Web.Script.Serialization;
using System.ComponentModel.DataAnnotations;

namespace GenericCampaignMasterModel
{
    public class CampaignState : Dictionary<string, string>
    {
        [Key]
        public string CampaignId { get; set; }
        public string CampaignName { get; set; }
        public List<PlayerInfo> ListPlayers { get; set; }
        public List<SektorInfo> ListSektorInfo { get; set; }
        public clsSektorKoordinaten FieldDimension { get; set; }
        public string FieldType { get; set; }
        public List<UnitInfo> ListUnitInfo { get; set; }
        public List<UnitTypeInfo> ListUnitTypes { get; set; }
        public List<ResourceInfo> ListResourceInfo { get; set; }

        public Dictionary<string, Sektor> DicSektors 
        {
            get
            {
                throw new Exception();
                Dictionary<string, Sektor> result = new Dictionary<string, Sektor>();
                return result;
            }

            set
            {

            }
        }
        //public clsUnitTypeCollection ListUnitTypes { get; set; }


        private JavaScriptSerializer m_serializer = new JavaScriptSerializer();

        #region Get-Methoden
        public List<Sektor> getListSektors()
        {
            return ListSektorInfo.Select(si => new Sektor(si)).ToList<Sektor>();
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
            return ListPlayers.Select(pi => new Player(pi)).ToList<Player>();
        }

        public List<UnitInfo> getListUnitInfo()
        {
            return ListUnitInfo;
        }

        public Dictionary<string, clsUnitType> getDicUnitTypeInfo()
        {
            string strUnitInfo = this.Keys.Contains("unittypesinfo") ? this["unittypesinfo"] : "";

            Dictionary<string, clsUnitType> dicUnitTypeInfo = (Dictionary<string, clsUnitType>)m_serializer.Deserialize<Dictionary<string, clsUnitType>>(strUnitInfo);
            return dicUnitTypeInfo;
        }

        public clsSektorKoordinaten getListDimensions()
        {
            return FieldDimension;
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
        #endregion

        public void Save()
        {
            this["campaignname"] = this.CampaignName;
            this["campaignid"] = this.CampaignId;
            this["ListPlayerInfo"] = m_serializer.Serialize (this.ListPlayers);
            this["sektors"] = m_serializer.Serialize (this.ListSektorInfo);
            this["fielddimension"] = m_serializer.Serialize(this.FieldDimension);
            this["fieldtype"] = this.FieldType;
			this["unitinfo"] = m_serializer.Serialize(this.ListUnitInfo);
            this["unittypesinfo"] = m_serializer.Serialize(this.ListUnitTypes);
            this["resourceinfo"] = m_serializer.Serialize(this.ListResourceInfo);
        }

        public override string ToString()
        {
            Save();
            return m_serializer.Serialize(this as Dictionary<string, string>);
        }

        public static CampaignState FromString(string stateStr)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, string> stateData = serializer.Deserialize<Dictionary<string, string>>(stateStr);

            CampaignState result = CampaignState.NewInstance();
            result.CampaignName = stateData["campaignname"];
            result.CampaignId = stateData["campaignid"];
            result.ListPlayers = serializer.Deserialize<List<PlayerInfo>>(stateData["ListPlayerInfo"]);
            result.ListSektorInfo = serializer.Deserialize<List<SektorInfo>>(stateData["sektors"]);
            result.FieldDimension = serializer.Deserialize<clsSektorKoordinaten>(stateData["fielddimension"]);
            result.FieldType = stateData["fieldtype"];
            result.ListUnitInfo = serializer.Deserialize<List<UnitInfo>>(stateData["unitinfo"]);
            result.ListUnitTypes = serializer.Deserialize<List<UnitTypeInfo>>(stateData["unittypesinfo"]);
            result.ListResourceInfo = serializer.Deserialize<List<ResourceInfo>>(stateData["resourceinfo"]);
            
            return result;
        }


        public static CampaignState NewInstance()
        {
            CampaignState result = new CampaignState();
            result.CampaignId = string.Empty;
            result.CampaignName = string.Empty;
            result.ListPlayers = new List<PlayerInfo>();
            result.ListSektorInfo = new List<SektorInfo>();
            //result.DicSektors = new Dictionary<string, Sektor>();
            result.FieldDimension = new clsSektorKoordinaten();
            result.FieldType = string.Empty;
            result.ListUnitInfo = new List<UnitInfo>();
            result.ListUnitTypes = new List<UnitTypeInfo>();
            result.ListResourceInfo = new List<ResourceInfo>();

            return result;
        }


        /// <summary>
        /// Konstruktor ist private - Klasse kann nicht instanziiert werden, sondern Instanz
        /// muss mit NewCampaignState() geholt werden. Stellt sicher, dass immer alle Felder initialisiert
        /// sind (Exceptions beim Serialisieren/Deserialisieren).
        /// </summary>
        private CampaignState(){}
    }
}
