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

            this.addNewType(newType);
        }


        internal clsUnitType getUnitType(int intUnitTypeID)
        {
            if (dicUnitTypeData.ContainsKey(intUnitTypeID.ToString()))
                return dicUnitTypeData[intUnitTypeID.ToString()];
            else
                throw new Exception("unbekannte UnitType ID");
        }

        public int addNewType(clsUnitType newUnitType)
        {
            int index = dicUnitTypeData.Count;
            newUnitType.ID = index;
            dicUnitTypeData.Add(index.ToString(), newUnitType);
            return index;
        }

        public clsUnitType getTypeByName(string strUnitTypeName)
        {
            foreach (clsUnitType aktType in dicUnitTypeData.Values)
            {
                if (aktType.strBez == strUnitTypeName)
                    return aktType;
            }

            return null;
        }
    }
}
