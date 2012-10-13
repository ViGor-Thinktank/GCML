using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel
{
    public class PlaceUnit : ICommand
    {
        public string UnitId { get; set; }    // ID für erzeugte Unit == ResourceID, damit der Controller die neue Unit registrieren kann.
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
   
            // UnitId setzen damit Unit registriert werden kann
            this.UnitId = unitToPlace.Id;
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
            nfo.targetId = TargetSektor.strUniqueID;
            return nfo;    
        }

        #endregion
    }
}
