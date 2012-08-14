using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class UnitTypeDummy : UnitTypeBase 
    {
        public UnitTypeDummy() : base("UnitTypeDummy")
        {
            this.m_intMovement = 1;
            this.m_intSichtweite = 1;
        }

        public override List<ICommand> getTypeCommands(BaseUnit CallingUnit)
        {
            Move cmd = new Move();
            cmd.Unit = CallingUnit;

            List<ICommand> cmdlist = new List<ICommand>();
            cmdlist.Add(cmd);
            return cmdlist;
        }
    }

    public abstract class UnitTypeBase
    {
        private string m_strDefaultBez = "";
        public string strDefaultBez { get { return m_strDefaultBez; } }

        protected  int m_intMovement = 0;
        protected int m_intSichtweite = 0;
        
        public int intSichtweite { get { return m_intSichtweite; } }
        public int intMovement { get { return m_intMovement; } }

        public UnitTypeBase() { ; }

        public UnitTypeBase(string strDefaultBez )
        {
            m_strDefaultBez = strDefaultBez;
        }

        public abstract List<ICommand> getTypeCommands(BaseUnit CallingUnit);
    }
}
