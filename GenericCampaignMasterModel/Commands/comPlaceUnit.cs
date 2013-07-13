using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel.Commands
{
    public class comPlaceUnit : clsCommandBaseClass
    {
        public string UnitId { get; set; }    // ID für erzeugte Unit == ResourceID, damit der Controller die neue Unit registrieren kann.
        public clsUnitType UnitTypeToPlace { get; set; }
        public Player Owner { get; set; }

        public comPlaceUnit()
            : base("PlaceUnit")
        { }
       
        public override void Execute() 
        {
            base.markExecute();

            clsUnit unitToPlace = new clsUnit(UnitTypeToPlace);
            unitToPlace.strOwnerID = Owner.Id;
            Owner.ListUnits.Add(unitToPlace);
            TargetSektor.addUnit(unitToPlace); 
   
            // UnitId setzen damit Unit registriert werden kann
            this.UnitId = unitToPlace.Id;
        }

        public new void Register()
        {
            
        }

        public override string strInfo
        {
            get
            {

                return "";
            }
        }

        public override clsFactoryBase getCommandFactory(clsUnit objUnit, Field FieldField)
        {
            return null;
        }
        
    }
}
