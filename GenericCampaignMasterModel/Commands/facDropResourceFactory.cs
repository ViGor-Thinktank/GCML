using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel.Commands
{
    public class facDropResourceFactory : clsSektorFactoryBase
    {
        private clsUnitGroup m_objDeliverUnit
        {
            get
            {
                return this.m_Unit;
            }
        }

        private List<clsUnitGroup> m_listPossibleDrops = new List<clsUnitGroup>();

        public facDropResourceFactory(clsUnitGroup objDeliverUnit, Field FieldField)
            : base(objDeliverUnit, FieldField)
        {
            //Init Stuff
            m_originSektor = FieldField.getSektorForUnit(m_objDeliverUnit);

        }

        public override void go()
        {
            this.createDropResourceCommandsForSektor(this.FieldField.get(m_originSektor.objSektorKoord), 0);
        }

        private void createDropResourceCommandsForSektor(Sektor aktSek, int intDistance)
        {
            foreach (clsSektorKoordinaten aktVektor in m_DirectionVektors)
            {
                Sektor newSek = this.FieldField.move(aktSek, aktVektor);

                if (newSek != null && aktSek.strUniqueID != newSek.strUniqueID)
                {
                    if (newSek.ListUnits.Count > 0)
                    {
                        foreach (clsUnitGroup objPosibleRecieverUnit in newSek.ListUnits)
                        {
                            if (objPosibleRecieverUnit.Id != m_objDeliverUnit.Id
                                    && objPosibleRecieverUnit.blnCanStoreResourceValue 
                                    && objPosibleRecieverUnit.strOwnerID == m_objDeliverUnit.strOwnerID
                                    && !m_listPossibleDrops.Contains(objPosibleRecieverUnit)
                                )
                            {
                                m_listPossibleDrops.Add(objPosibleRecieverUnit);

                                comDropResource readyCmd = new comDropResource(m_objDeliverUnit, objPosibleRecieverUnit);
                                

                                Sektor targetSek = this.FieldField.get(newSek.objSektorKoord);

                                readyCmd.TargetSektor = targetSek;

                                this.raiseOnNewCommand(readyCmd);
                            }
                        }
                    }

                    intDistance += 1;

                    //atm zu Dev zwecken: es kann ins eigene Feld und in anliegende Deliverd werden 
                    if (intDistance < 1)
                    {
                        createDropResourceCommandsForSektor(newSek, intDistance);
                    }
                }


            }
            
        }
    }
}
