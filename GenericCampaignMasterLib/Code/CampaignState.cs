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
    public class CampaignState : Dictionary<string, string>
    {

        private JavaScriptSerializer m_serializer = new JavaScriptSerializer();

        public List<Sektor> getListSektors()
        {
			return (List<Sektor>) m_serializer.Deserialize<List<Sektor>>(this["sektors"]);
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

            
           return (List<Player>)m_serializer.Deserialize<List<Player>>(this["players"]);
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

        public CampaignState Save(CampaignEngine engine)
        {
            this["campaignname"] = engine.CampaignName;
            this["players"] = m_serializer.Serialize (engine.ListPlayers);
            this["sektors"] = m_serializer.Serialize (engine.FieldField.dicSektors.Values);
            this["fielddimension"] = m_serializer.Serialize(engine.FieldField.FieldDimension);
            this["fieldtype"] = engine.FieldField.GetType().ToString();
			
			List<UnitInfo> lstUnitInfo = engine.getUnitInfo();
			this["unitinfo"] = m_serializer.Serialize(lstUnitInfo);


            this["unittypesinfo"] = m_serializer.Serialize(clsUnit.objUnitTypeFountain);

            List<ResourceInfo> lstResInfo = engine.ResourceHandler.getResourceInfo();
            this["resourceinfo"] = m_serializer.Serialize(lstResInfo);
            
            return this;
        }
        
        public CampaignEngine Restore()
        {
            List<Player> lstPlayers = getListPlayers();
            List<Sektor> lstSektors = getListSektors();

            Sektor bla = lstSektors[0];

            // Feld erstellen;
            Type fieldType = Type.GetType(getFieldtype());
            clsSektorKoordinaten objSekKoord = getListDimensions();
            Field field = (Field)Activator.CreateInstance(fieldType, new object[] { objSekKoord });
            field.setSektorList(lstSektors);

			// Engine erstellen
            CampaignEngine engine = new CampaignEngine((Field)field);
            engine.CampaignName = this.ContainsKey("campaignname") ? this["campaignname"] : "OLDCAMPAIGN";
            engine.setPlayerList(lstPlayers);

            clsUnit.objUnitTypeFountain.dicUnitTypeData = this.getDicUnitTypeInfo();
 
            // Units platzieren
			foreach(UnitInfo uInfo in getListUnitInfo())
			{
				Type unitType = Type.GetType(uInfo.unitType);
				//UnitTypeBase newUnitType = (UnitTypeBase)Activator.CreateInstance(unitType);
				
                Player aktP = (from p in lstPlayers
						where p.Id == uInfo.playerId 
						select p).First();

                clsUnit unit = aktP.getUnitByID(uInfo.unitId);

                //clsUnit unit = new clsUnit(uInfo.unitId);
                field.dicSektors[uInfo.sektorId].addUnit(unit);

				
			}//*/

            // Ressourcehandler erzeugen und Ressourcen wiederherstellen
            ResourceHandler resHandler = new ResourceHandler();
            engine.ResourceHandler = resHandler;
            foreach (ResourceInfo resInfo in getListResourceInfo())
            {
                string strResType = resInfo.resourceableType;
                Type resType = Type.GetType(strResType);
                Object typeObj = Activator.CreateInstance(resType);

                Player resourceOwner = (from p in lstPlayers
                                       where p.Id == resInfo.ownerId
                                       select p).First() as Player;

                resHandler.addRessourcableObject(resourceOwner, (IResourceable)typeObj);
            }

            

            return engine;
        }

        public override string ToString()
        {
            return m_serializer.Serialize(this);
        }

    }
}
