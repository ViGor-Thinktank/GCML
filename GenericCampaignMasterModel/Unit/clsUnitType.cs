using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterModel.Commands;
using GenericCampaignMasterModel;

namespace GenericCampaignMasterModel
{
    public class clsUnitType : IResourceable
    {

        //Eigenschaften
        private string m_strBez = "";
        protected int m_intMovement = 0;
        protected int m_intSichtweite = 0;
        private int m_ID = -1;

        [NonSerialized]
        public List<clsUnitType> m_listUnitSpawn = new List<clsUnitType>();

        public bool blnAlywaysVisible = false;

        //Q&D Dev Zwischenlösung
        public clsUnitType firstUnitSpawnType
        {
            get
            {
                if (m_listUnitSpawn.Count() > 0)
                    return m_listUnitSpawn[0];
                else
                    return null;
            }
        }

        public string strClientData = ""; //extra Daten die der Client benötigt && verwaltet, Beispiel Texturen Infos
        public string strDescription = "";

        public int ID { get { return m_ID; } set { m_ID = value; } }
        public string strBez { get { return m_strBez; } set { m_strBez = value; } }
        public int intSichtweite { get { return m_intSichtweite; } set { m_intSichtweite = value; } }
        public int intMovement { get { return m_intMovement; } set { m_intMovement = value; } }

        public bool blnCanStoreResourceValue { get { return intMaxResourceValue != -1; } }
        public bool blnCanSpawnUnits { get { return m_listUnitSpawn != null; } }


        //q&d lösung für einfache Uniterzeugung
        public clsUnitType(string strBez, int intSichtweite, int intMovement, string strTexture, string strDescription = "", List<clsUnitType> listUnitspawn = null, int intMaxResourceValue = -1, bool blnResourceProduzent = false, bool blnAlywaysVisible = false)
        {
            this.strBez = strBez;
            this.intSichtweite = intSichtweite;
            this.intMovement = intMovement;
            this.strClientData = "Texture:=" + strTexture;
            this.m_listUnitSpawn = listUnitspawn;
            this.intMaxResourceValue = intMaxResourceValue;
            this.strDescription = strDescription;
            this.blnAlywaysVisible = blnAlywaysVisible;
            this.intCreateValuePerRound = blnAlywaysVisible ? 10 : 0;
        }

        //Konstruktoren
        public clsUnitType() { ; }
        public clsUnitType(int newID, string strDefaultBez)
        {
            m_ID = newID;
            m_strBez = strDefaultBez;
        }

        public clsUnitType(UnitTypeInfo info)
        {
            this.ID = info.ID;
            this.strBez = info.Bezeichnung;
            this.intSichtweite = info.Sichtweite;
            this.intMovement = info.Movement;
            this.strClientData = info.ClientData;
            this.strDescription = info.Description;
            this.intMaxResourceValue = info.MaxResourceValue;
            this.intCreateValuePerRound = info.CreateValuePerRound;
        }

        public List<ICommand> getResourceCommands(Player owner)
        {
            List<ICommand> placeUnitCommands = new List<ICommand>();
            // Todo: Accessible-Fields Property für Player: Felder die Einheiten platziert werden können (Ruleset?)
            foreach (Sektor sektor in owner.accessibleSectors)
            {
                comPlaceUnit cmd = new comPlaceUnit(null, owner, this);
                cmd.CommandId = Guid.NewGuid().ToString();
                cmd.TargetSektor = sektor;
                placeUnitCommands.Add(cmd);
            }

            return placeUnitCommands.ToList<ICommand>();
        }

        public int intMaxResourceValue { get; set; }

        public int intCreateValuePerRound { get; set; }

        public string strIconName
        {
            get
            {
                return this.strBez + "_Icon";
            }
        }

        public UnitTypeInfo getInfo()
        {
            UnitTypeInfo result = new UnitTypeInfo();
            result.ID = ID;
            result.Bezeichnung = strBez;
            result.Sichtweite = intSichtweite;
            result.Movement = intMovement;
            result.ClientData = strClientData;
            result.Description = strDescription;
            result.MaxResourceValue = intMaxResourceValue;
            result.CreateValuePerRound = intCreateValuePerRound;
            return result;
        }

    }
}
