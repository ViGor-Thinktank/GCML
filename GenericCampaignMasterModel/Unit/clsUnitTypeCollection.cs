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
                }
                return m_dicUnitTypeData;
            }
            set
            {
                m_dicUnitTypeData = value;
            }

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
