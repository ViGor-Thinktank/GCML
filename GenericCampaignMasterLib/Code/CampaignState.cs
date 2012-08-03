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
		public string unitType;
    }

    
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

            return null;
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

        public clsSektorKoordinaten getListDimensions()
        {
            return (clsSektorKoordinaten)m_serializer.Deserialize<clsSektorKoordinaten>(this["fielddimension"]);
        }

        public string getFieldtype()
        {
        
            return this["fieldtype"];
        }

        public CampaignState Save(CampaignEngine engine)
        {
            this["players"] = m_serializer.Serialize (engine.ListPlayers);
            this["sektors"] = m_serializer.Serialize (engine.FieldField.dicSektors.Values);
            this["fielddimension"] = m_serializer.Serialize(engine.FieldField.FieldDimension);
            this["fieldtype"] = engine.FieldField.GetType().ToString();
			
			List<UnitInfo> lstUnitInfo = engine.getUnitInfo();
			this["unitinfo"] = m_serializer.Serialize(lstUnitInfo);
			
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
            engine.setPlayerList(lstPlayers);

            // Units platzieren
			foreach(UnitInfo uInfo in getListUnitInfo())
			{
				Type unitType = Type.GetType(uInfo.unitType);
				//UnitTypeBase newUnitType = (UnitTypeBase) Activator.CreateInstance(unitType);

                UnitTypeBase newUnitType = (UnitTypeBase)Activator.CreateInstance(unitType);
				
                BaseUnit unit = new BaseUnit(Int32.Parse(uInfo.unitId));
                string sektorId = uInfo.sektorId;
                clsSektorKoordinaten sektorKoord = field.dicSektors[sektorId].objSektorKoord;

				// TODO Koordinaten
				engine.addUnit(uInfo.playerId, unit, sektorKoord);
			}
			
            return engine;
        }

        public override string ToString()
        {
            return m_serializer.Serialize(this);
        }

    }
}
