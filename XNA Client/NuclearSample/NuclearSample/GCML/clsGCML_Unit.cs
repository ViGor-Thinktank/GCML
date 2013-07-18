using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCML_XNA_Client.GCML
{
    class clsGCML_Unit 
    {
        public clsGCML_Unit(GenericCampaignMasterModel.clsUnit newUnit)
        {
            objUnit = newUnit;
        }
        
        public GenericCampaignMasterModel.clsUnit objUnit;

        private Dictionary<string, string> m_dicClientData;
        private Dictionary<string, string> dicClientData
        {
            get 
            {
                if (m_dicClientData == null)
                {
                    m_dicClientData = new Dictionary<string, string>();

                    foreach (string str in objUnit.UnitType.strClientData.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string[] strSplit = str.Split(new string[] { ":=" }, StringSplitOptions.None);
                        m_dicClientData.Add(strSplit[0], strSplit[1]);
                    }
                }
                return m_dicClientData;
            }
        }

        public string strTexName
        {
            get
            {
                return dicClientData["Texture"];
            }
        }

        public GenericCampaignMasterModel.Sektor aktSektor
        {
            get
            {
                return Program.m_objCampaign.Unit_getSektorForUnit(this.objUnit);
            }
        }
        }
}

