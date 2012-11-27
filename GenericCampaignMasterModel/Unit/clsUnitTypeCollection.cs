using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GenericCampaignMasterModel
{
    public class clsUnitTypeCollection
    {

        private Dictionary<string, clsUnitType> m_dicUnitTypeData = null;

        public Dictionary<string, clsUnitType> dicUnitTypeData
        {
            get
            {
                if (m_dicUnitTypeData == null)
                {
                    m_dicUnitTypeData = new Dictionary<string, clsUnitType>();
                    this.initNewData();
                }
                return m_dicUnitTypeData;
            }
            set
            {
                m_dicUnitTypeData = value;
            }

        }

        public void initNewData()
        {
            //erster Debugtype
            clsUnitType newType = new clsUnitType();
            newType.strBez = "böhmische Dragonerschwadron";
            newType.intMovement = 2;
            newType.intSichtweite = 1;

            this.Add(newType);
        }


        internal clsUnitType getUnitType(int intUnitTypeID)
        {
            switch (intUnitTypeID)
            {
                case 0:
                    return new clsUnitTypeDummy(0);

                case 1:
                    return dicUnitTypeData[intUnitTypeID.ToString()];

                default:
                    throw new Exception("unbekannter UnitType");

            }
        }

        public int Add(clsUnitType testtyp)
        {
            int index = dicUnitTypeData.Count;
            dicUnitTypeData.Add(index.ToString(), testtyp);
            return index;
        }
    }
}
