using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Playground
{
    public class clsCampaignInfo
    {
        

        public string strCCKey = "";
        public string strSaveKey = "";

        public void save()
        {
            System.IO.StreamWriter m_stwSave = new System.IO.StreamWriter(".\\CCDate.dat", false);
            m_stwSave.WriteLine(strCCKey);
            m_stwSave.WriteLine(strSaveKey);
            m_stwSave.Close();
        }

        public void load()
        {
            
            System.IO.StreamReader m_strLoad = new System.IO.StreamReader(".\\CCDate.dat", false);
            strCCKey = m_strLoad.ReadLine();
            strSaveKey = m_strLoad.ReadLine();
            
            m_strLoad.Close();
        }

    }
}
