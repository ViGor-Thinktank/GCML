using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel.Commands
{
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
                    comPlaceUnit readyCmd = new comPlaceUnit(m_Unit, this.actingPlayer, m_Unit.UnitType.firstUnitSpawnType);

                    readyCmd.TargetSektor = this.FieldField.get(newSek.objSektorKoord);

                    this.raiseOnNewCommand(readyCmd);

                    intFieldsMoved += 1;

                    if (intFieldsMoved < 2)
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