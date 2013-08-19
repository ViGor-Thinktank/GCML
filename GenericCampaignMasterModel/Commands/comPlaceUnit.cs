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
        
        public comPlaceUnit(clsUnit Unit)
            : base("PlaceUnit")
        {
            this.m_objUnitToCommand = Unit;
        }

        public comPlaceUnit(clsUnit Unit, Player owner, clsUnitType UnitTypeToPlace)
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

        public override clsFactoryBase getCommandFactory(clsUnit objUnit, Field FieldField)
        {
            return new facPlaceUnitFactory(objUnit, FieldField);
        }
        
    }

    public class facPlaceUnitFactory : clsSektorFactoryBase
    {

        public facPlaceUnitFactory(clsUnit u, Field FieldField)
            : base(u, FieldField)
        {
            //set Members
            m_Unit = u;

            //Init Stuff
            m_originSektor = FieldField.getSektorForUnit(u);
        }

        private void createPlaceCommandsForSektor(Sektor aktSek, int intFieldsMoved)
        {
            foreach (clsSektorKoordinaten aktVektor in m_DirectionVektors)
            {
                Sektor newSek = this.FieldField.move(aktSek, aktVektor);

                if (newSek != null && aktSek.strUniqueID != newSek.strUniqueID)
                {
                    comPlaceUnit readyCmd = new comPlaceUnit(m_Unit, this.actingPlayer, m_Unit.firstUnitSpawnType);

                    readyCmd.TargetSektor = this.FieldField.get(newSek.objSektorKoord);

                    this.raiseOnNewCommand(readyCmd);

                    intFieldsMoved += 1;

                    if (intFieldsMoved < 1)
                        createPlaceCommandsForSektor(newSek, intFieldsMoved);
                }
            }
        }

        public override void go()
        {
            this.createPlaceCommandsForSektor(this.FieldField.get(m_originSektor.objSektorKoord), 0);
        }
    }
}
