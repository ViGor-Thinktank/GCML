using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCML_XNA_Client.GCML
{
    class clsGCML_Unit 
    {
        public clsGCML_Unit(GenericCampaignMasterModel.clsUnitGroup newUnit)
        {
            objUnit = newUnit;
        }
        
        public GenericCampaignMasterModel.clsUnitGroup objUnit;

        private Dictionary<string, List<string>> m_dicClientData;
        
        private Dictionary<string, List<string>> dicClientData
        {
            get 
            {
                if (m_dicClientData == null)
                {
                    m_dicClientData = new Dictionary<string, List<string>>();

                    foreach (string str in objUnit.strClientData.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        foreach (string strData in str.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            string[] strSplit = strData.Split(new string[] { ":=" }, StringSplitOptions.None);
                            if (!m_dicClientData.ContainsKey(strSplit[0]))
                            { 
                                m_dicClientData.Add(strSplit[0], new List<string>());
                            }
                            m_dicClientData[strSplit[0]].Add(strSplit[1]);
                        }
                    }
                }
                return m_dicClientData;
            }
        }

        public List<string> strTexNames
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

