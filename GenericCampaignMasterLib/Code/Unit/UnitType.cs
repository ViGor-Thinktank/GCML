using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib.Code.Unit
{
    public class UnitTypeDummy : UnitTypeBase 
    {
        public UnitTypeDummy() : base("UnitTypeDummy")
        { 
        
        }

        public override List<ICommand> getCommands(Unit.BaseUnit CallingUnit)
        {
            Move cmd = new Move();
            cmd.IntRange = 1;
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

        public UnitTypeBase(string strDefaultBez )
        {
            m_strDefaultBez = strDefaultBez;
        }

        public abstract List<ICommand> getCommands(Unit.BaseUnit CallingUnit);
    }
}
