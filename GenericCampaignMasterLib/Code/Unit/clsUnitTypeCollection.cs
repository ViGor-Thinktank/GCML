using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GenericCampaignMasterLib.Unit
{
    public class clsUnitTypeCollection
    {

        private clsUnitDataDic m_tblUnitTypeData = null;

        private clsUnitDataDic tblUnitTypeData
        {
            get {
                if (m_tblUnitTypeData == null)
                    { m_tblUnitTypeData = new clsUnitDataDic(); }
                return m_tblUnitTypeData; 
            
            }
            
        }

        public class clsUnitDataDic : Dictionary<int, clsUnitType>
        {
            public clsUnitDataDic() { initNewData(); }

            public clsUnitDataDic(string strKey) { initLoadData(strKey); }


            private void initNewData()
            {
                clsUnitType newType = new clsUnitType();
                newType.strBez = "böhmische Dragonerschwadron";
                newType.intMovement = 2;
                newType.intSichtweite = 1;

                this.Add(1, newType);
            }


            private void initLoadData(string strKey)
            {
                //m_tblUnitTypeData.ReadXml(strKey / "UnitTypeData.xml");
                //Zwischenlösung:
                //for each row .. etc

            }

        }

        

        internal clsUnitType getUnitType(int intUnitTypeID)
        {
            switch (intUnitTypeID)
            {
                case 0:
                    return new clsUnitTypeDummy();

                case 1:
                    return tblUnitTypeData[intUnitTypeID];                    

                default:
                    throw new Exception("unbekannter UnitType");

            }
        }
    }
}
