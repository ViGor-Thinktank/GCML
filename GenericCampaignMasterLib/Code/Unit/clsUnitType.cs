using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class clsUnitTypeDummy : clsUnitType 
    {
        public clsUnitTypeDummy() : base("UnitTypeDummy")
        {
            this.m_intMovement = 1;
            this.m_intSichtweite = 1;
        }

        
    }

    public class clsUnitType
    {
        //Eigenschaften
        private string m_strBez = "";
        protected  int m_intMovement = 0;
        protected int m_intSichtweite = 0;

        public string strBez { get { return m_strBez; } set { m_strBez = value; } }
        public int intSichtweite { get { return m_intSichtweite; } set { m_intSichtweite = value; } }
        public int intMovement { get { return m_intMovement; } set { m_intMovement = intMovement; } }

        //Konstruktoren
        public clsUnitType() { ; }
        public clsUnitType(string strDefaultBez )
        {
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
        
        
    }
}
