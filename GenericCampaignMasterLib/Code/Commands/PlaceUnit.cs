using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class PlaceUnit : ICommand
    {
        public clsUnitType UnitTypeToPlace { get; set; }
        public Sektor TargetSektor { get; set; }
        public Player Owner { get; set; }

        # region ICommand Member
        public void Execute()
        {
            clsUnit unitToPlace = new clsUnit(UnitTypeToPlace);
            unitToPlace.strOwnerID = Owner.Id;
            Owner.ListUnits.Add(unitToPlace);
            TargetSektor.addUnit(unitToPlace);    
        }

        public void Register()
        {
            
        }

        public string strInfo { get; set; }
        public bool blnExecuted { get; set; }
        public string CommandId { get; set; }
        
        public CommandInfo getInfo()
        {
            CommandInfo nfo = new CommandInfo();
            nfo.commandId = this.CommandId;
            nfo.actingUnitId = "";
            nfo.commandType = this.GetType().ToString();
            nfo.strInfo = "Plazieren auf Sektor " + TargetSektor.strUniqueID;
          
            return nfo;    
        }

        #endregion

        
    }
}
