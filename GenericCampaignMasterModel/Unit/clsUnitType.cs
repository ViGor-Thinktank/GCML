using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterModel.Commands;

namespace GenericCampaignMasterModel
{
    public class clsUnitType : IResourceable
    {

        //Eigenschaften
        private string m_strBez = "";
        protected  int m_intMovement = 0;
        protected int m_intSichtweite = 0;
        private int m_ID = -1;
        private List<clsUnitType> m_listUnitSpawn = null;

        private int m_intResourceValue = -1;

        public string strClientData = ""; //extra Daten die der Client benötigt && verwaltet, Beispiel Texturen Infos
        public string strDescription = "";

        public int ID { get { return m_ID; } set { m_ID = value; } }
        public string strBez { get { return m_strBez; } set { m_strBez = value; } }
        public int intSichtweite { get { return m_intSichtweite; } set { m_intSichtweite = value; } }
        public int intMovement  { get { return m_intMovement; } set { m_intMovement = value; } }
        public int intResourceValue { get { return m_intResourceValue; } set { m_intResourceValue = value; } }

        public bool blnCanStoreResourceValue { get { return m_intResourceValue != -1; } }
        public bool blnCanSpawnUnits { get { return m_listUnitSpawn != null; } }


        //q&d lösung für einfache Uniterzeugung
        public clsUnitType(string strBez, int intSichtweite, int intMovement, string strTexture, string strDescription = "", List<clsUnitType> listUnitspawn = null, int intResourceValue = -1)
        {
            this.strBez = strBez;
            this.intSichtweite = intSichtweite;
            this.intMovement = intMovement;
            this.strClientData = "Texture:=" + strTexture ;
            this.m_listUnitSpawn = listUnitspawn;
            this.intResourceValue = intResourceValue;
            this.strDescription = strDescription;
        }

        //Konstruktoren
        public clsUnitType() { ; }
        public clsUnitType(int newID, string strDefaultBez )
        {
            m_ID = newID;
            m_strBez = strDefaultBez;
        }

        //Funktionen
        public List<ICommand> getTypeCommands(clsUnit CallingUnit)
        {
            List<ICommand> cmdlist = new List<ICommand>();

            ICommand cmd;

            if (CallingUnit.intMovement > 0)
            {
                cmd = new comMove(CallingUnit);
                cmdlist.Add(cmd);
            }

            if (CallingUnit.intResourceValue > 0)
            {
                cmd = new comDropResource(CallingUnit, null);
                cmdlist.Add(cmd);

                if (CallingUnit.UnitType.blnCanSpawnUnits)
                {
                    cmd = new comPlaceUnit();
                    cmdlist.Add(cmd);
                }
            }

            return cmdlist;
        }



        public List<ICommand> getResourceCommands(Player owner)
        {
            List<ICommand> placeUnitCommands = new List<ICommand>();
            // Todo: Accessible-Fields Property für Player: Felder die Einheiten platziert werden können (Ruleset?)
            foreach (Sektor sektor in owner.accessibleSectors)
            {
                comPlaceUnit cmd = new comPlaceUnit();
                cmd.CommandId = Guid.NewGuid().ToString();
                cmd.TargetSektor = sektor;
                cmd.UnitTypeToPlace = this;
                cmd.Owner = owner;
                placeUnitCommands.Add(cmd);
            }

            return placeUnitCommands.ToList<ICommand>();
        }
    }
}
