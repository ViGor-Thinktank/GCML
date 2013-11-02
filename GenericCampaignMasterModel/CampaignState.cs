﻿using System;
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
        public List<SektorInfo> ListSektorInfo { get; set; }
        public clsSektorKoordinaten FieldDimension { get; set; }
        public string FieldType { get; set; }
        public List<UnitInfo> ListUnitInfo { get; set; }
        public clsUnitTypeCollection ListUnitTypes { get; set; }
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


        private JavaScriptSerializer m_serializer = new JavaScriptSerializer();

        #region Get-Methoden
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
        #endregion

        public void Save()
        {
            this["campaignname"] = this.CampaignName;
            this["campaignid"] = this.CampaignId;
            this["players"] = m_serializer.Serialize (this.ListPlayers);
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
            result.ListPlayers = serializer.Deserialize<List<Player>>(stateData["players"]);
            result.ListSektorInfo = serializer.Deserialize<List<SektorInfo>>(stateData["sektors"]);
            //result.FieldDimension = serializer.Deserialize<clsSektorKoordinaten>(result["fielddimension"]);
            
            //serializer.Deserialize<string>(result["fieldtype"]
            //serializer.Deserialize<string>(result["unitinfo"]
            //serializer.Deserialize<string>(result["unittypesinfo"]
            //serializer.Deserialize<string>(result["resourceinfo"]
            return result;
        }


        public static CampaignState NewInstance()
        {
            CampaignState result = new CampaignState();
            result.CampaignId = string.Empty;
            result.CampaignName = string.Empty;
            result.ListPlayers = new List<Player>();
            result.ListSektorInfo = new List<SektorInfo>();
            //result.DicSektors = new Dictionary<string, Sektor>();
            result.FieldDimension = new clsSektorKoordinaten();
            result.FieldType = string.Empty;
            result.ListUnitInfo = new List<UnitInfo>();
            result.ListUnitTypes = new clsUnitTypeCollection();
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
