﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel.Commands
{
    public class comMove : clsCommandBaseClass
    {
        public comMove(clsUnit Unit) : base("Move")
        {
            this.m_objUnitToCommand = Unit;
        }

        public Sektor OriginSektor { get; set; }

        public override void Execute()
		{
            base.markExecute();
            OriginSektor.removeUnit(this.m_objUnitToCommand);
			TargetSektor.addUnit(this.m_objUnitToCommand);
        }

        public override clsFactoryBase getCommandFactory(clsUnit objUnit, Field FieldField)
        {
            return new facMoveFactory(objUnit, FieldField);

        }
        
        public override string strInfo
        {
            get
            {
                return this.OriginSektor.strUniqueID + " -> " + TargetSektor.strUniqueID;
            }
        }       
    }

    public class facMoveFactory : clsSektorFactoryBase
    {
        private List<string> m_listKnownMovements = new List<string>();

        public facMoveFactory(clsUnit u, Field FieldField)
            : base(u, FieldField)
        {

            //set Members
            m_Unit = u;


            //Init Stuff
            m_originSektor = FieldField.getSektorForUnit(u);
            m_listKnownMovements.Add(m_originSektor.strUniqueID);


        }

        private void createMoveCommandsForSektor(Sektor aktSek, int intFieldsMoved)
        {
            foreach (clsSektorKoordinaten aktVektor in m_DirectionVektors)
            {
                Sektor newSek = this.FieldField.move(aktSek, aktVektor);

                if (newSek != null && aktSek.strUniqueID != newSek.strUniqueID)
                {

                    if (!m_listKnownMovements.Contains(newSek.strUniqueID))
                    {
                        m_listKnownMovements.Add(newSek.strUniqueID);

                        comMove readyCmd = new comMove(m_Unit);


                        readyCmd.OriginSektor = m_originSektor;

                        Sektor targetSek = this.FieldField.get(newSek.objSektorKoord);

                        readyCmd.TargetSektor = targetSek;

                        this.raiseOnNewCommand(readyCmd);
                    }

                    int intNewFieldsMoved = intFieldsMoved + newSek.intMoveCost;

                    if (intNewFieldsMoved < m_Unit.intMovement)
                    {
                        createMoveCommandsForSektor(newSek, intNewFieldsMoved);
                    }
                }


            }


        }

        public override void go()
        {
            this.createMoveCommandsForSektor(this.FieldField.get(m_originSektor.objSektorKoord), 0);
        }
    }
}
