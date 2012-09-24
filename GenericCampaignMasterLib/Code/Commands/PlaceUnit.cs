using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class PlaceUnit : ICommand
    {
        public clsUnit UnitToPlace { get; set; }
        public Sektor TargetSektor { get; set; }

        # region ICommand Member
        public void Execute()
        {
            TargetSektor.addUnit(UnitToPlace);    
        }

        public void Register()
        {
            throw new NotImplementedException();
        }

        public string strInfo { get; set; }
        public bool blnExecuted { get; set; }
        public string CommandId { get; set; }
        
        public CommandInfo getInfo()
        {
            CommandInfo nfo = new CommandInfo();
            //nfo.commandId = this.CommandId;
            //nfo.actingUnitId = this.UnitToPlace.Id;
            //nfo.commandType = this.GetType().ToString();
            //nfo.strInfo = this.strInfo;
            //nfo.isActive = (this.UnitToPlace.aktCommand == this) ? true : false;
            return nfo;    
        }

        #endregion
    }
}
