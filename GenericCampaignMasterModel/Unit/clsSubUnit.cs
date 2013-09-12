using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterModel.Commands;

namespace GenericCampaignMasterModel
{
    public class clsSubUnit
    {
        public clsSubUnit(clsUnitType objUnitType, int ID)
        {
            this.ID = ID;
            this.m_objUnitType = objUnitType;
        }

        public int ID = -1;

        public int intMovement { get { return m_objUnitType.intMovement; } }
        public int intSichtweite { get { return m_objUnitType.intSichtweite; } }
        public int intResourceValue = 0;

        public int intMaxResourceValue { get { return m_objUnitType.intMaxResourceValue; } }

        private clsUnitType m_objUnitType;
        public clsUnitType objUnitType
        {
            get { return m_objUnitType; }
        }
    }

    
}
