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
    public class CampaignState
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

        //public Dictionary<string, Sektor> DicSektors 
        //{
        //    get
        //    {
        //        throw new Exception();
        //        Dictionary<string, Sektor> result = new Dictionary<string, Sektor>();
        //        return result;
        //    }

        //    set
        //    {

        //    }
        //}
        //public clsUnitTypeCollection ListUnitTypes { get; set; }


        private JavaScriptSerializer m_serializer = new JavaScriptSerializer();

        #region Get-Methoden
        //public List<Sektor> getListSektors()
        //{
        //    return ListSektorInfo.Select(si => new Sektor(si)).ToList<Sektor>();
        //}

        //public Player getPlayer(string strPlayerID)
        //{

        //    List<Player> lisP = getListPlayers();
            
        //    var varPlayerList = from p in lisP
        //                 where p.Id == strPlayerID
        //                 select p;


            
        //    if (varPlayerList.Count() == 1)
        //        return varPlayerList.First();
        //    else if (varPlayerList.Count() > 1)
        //        throw new Exception("mehr als ein Treffer für PlayerID " + strPlayerID);
        //    else
        //        throw new Exception("kein Treffer für PlayerID " + strPlayerID);

        //}

        //public List<Player> getListPlayers()
        //{
        //    return ListPlayers.Select(pi => new Player(pi)).ToList<Player>();
        //}

        //public List<UnitInfo> getListUnitInfo()
        //{
        //    return ListUnitInfo;
        //}

        public Dictionary<string, clsUnitType> getDicUnitTypeInfo()
        {
           Dictionary<string, clsUnitType> result = new Dictionary<string, clsUnitType>();
            foreach (var ui in ListUnitTypes)
                result.Add(ui.ID.ToString(), new clsUnitType(ui));
            return result;
        }

        //public clsSektorKoordinaten getListDimensions()
        //{
        //    return FieldDimension;
        //}

        //public string getFieldtype()
        //{
        
        //    return this["fieldtype"];
        //}

        //public List<ResourceInfo> getListResourceInfo()
        //{
        //    List<ResourceInfo> lstResInfo;
        //    string strResInfo = this.Keys.Contains("resourceinfo") ? this["resourceinfo"] : "";

        //    if (String.IsNullOrEmpty(strResInfo))
        //        lstResInfo = new List<ResourceInfo>();
        //    else
        //        lstResInfo = (List<ResourceInfo>)m_serializer.Deserialize<List<ResourceInfo>>(strResInfo);
            
        //    return lstResInfo;
        //}
        #endregion



        public override string ToString()
        {
            return m_serializer.Serialize(this);
        }

        public static CampaignState FromString(string stateStr)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            CampaignState stateData = serializer.Deserialize<CampaignState>(stateStr);

            //CampaignState result = CampaignState();
            //result.CampaignName = stateData["campaignname"];
            //result.CampaignId = stateData["campaignid"];
            //result.ListPlayers = serializer.Deserialize<List<PlayerInfo>>(stateData["ListPlayerInfo"]);
            //result.ListSektorInfo = serializer.Deserialize<List<SektorInfo>>(stateData["sektors"]);
            //result.FieldDimension = serializer.Deserialize<clsSektorKoordinaten>(stateData["fielddimension"]);
            //result.FieldType = stateData["fieldtype"];
            //result.ListUnitInfo = serializer.Deserialize<List<UnitInfo>>(stateData["unitinfo"]);
            //result.ListUnitTypes = serializer.Deserialize<List<UnitTypeInfo>>(stateData["unittypesinfo"]);
            //result.ListResourceInfo = serializer.Deserialize<List<ResourceInfo>>(stateData["resourceinfo"]);

            return stateData;
        }

        public CampaignState()
        {
            CampaignId = string.Empty;
            CampaignName = string.Empty;
            ListPlayers = new List<PlayerInfo>();
            ListSektorInfo = new List<SektorInfo>();
            //result.DicSektors = new Dictionary<string, Sektor>();
            FieldDimension = new clsSektorKoordinaten();
            FieldType = string.Empty;
            ListUnitInfo = new List<UnitInfo>();
            ListUnitTypes = new List<UnitTypeInfo>();
            ListResourceInfo = new List<ResourceInfo>();
        }
    }
}
