using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel.Commands
{
    public class comPlaceUnit : clsCommandBaseClass
    {
      
        public string UnitId { get; private set; }    // ID für erzeugte Unit == ResourceID, damit der Controller die neue Unit registrieren kann.
        private clsUnitType m_UnitTypeToPlace = null;

        private Player m_Owner = null;
        
        public comPlaceUnit(clsUnitGroup Unit)
            : base("PlaceUnit")
        {
            this.m_objUnitToCommand = Unit;
        }

        public comPlaceUnit(clsUnitGroup Unit, Player owner, clsUnitType UnitTypeToPlace)
            : base("PlaceUnit")
        {   
            this.m_objUnitToCommand = Unit;
            this.m_Owner = owner;
            this.m_UnitTypeToPlace = UnitTypeToPlace;
        }

        public string strNewOwner()
        {
            return this.m_objUnitToCommand.strOwnerID;
        }
        public int intNewUnitTypeID()
        {
            return this.m_UnitTypeToPlace.ID;
        }

        public override void Execute() 
        {
            base.markExecute();
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

        public override clsFactoryBase getCommandFactory(clsUnitGroup objUnit, Field FieldField)
        {
            return new facPlaceUnitFactory(objUnit, FieldField);
        }
        
    }
}
