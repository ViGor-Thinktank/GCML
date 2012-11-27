using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel
{
    public class clsUnitTypeDummy : clsUnitType 
    {
        public clsUnitTypeDummy(int ID) : base(ID, "UnitTypeDummy")
        {
            this.m_intMovement = 1;
            this.m_intSichtweite = 1;
        }

        
    }

    public class clsUnitType : IResourceable
    {

        //Eigenschaften
        private string m_strBez = "";
        protected  int m_intMovement = 0;
        protected int m_intSichtweite = 0;
        private int m_ID = -1;

        public int ID { get { return m_ID; } set { m_ID = value; } }
        public string strBez { get { return m_strBez; } set { m_strBez = value; } }
        public int intSichtweite { get { return m_intSichtweite; } set { m_intSichtweite = value; } }
        public int intMovement { get { return m_intMovement; } set { m_intMovement = intMovement; } }

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
            Move cmd = new Move();
            cmd.Unit = CallingUnit;

            List<ICommand> cmdlist = new List<ICommand>();
            cmdlist.Add(cmd);
            return cmdlist;
        }



        public List<ICommand> getResourceCommands(Player owner)
        {
            List<ICommand> placeUnitCommands = new List<ICommand>();
            // Todo: Accessible-Fields Property für Player: Felder die Einheiten platziert werden können (Ruleset?)
            foreach (Sektor sektor in owner.accessibleSectors)
            {
                PlaceUnit cmd = new PlaceUnit();
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
