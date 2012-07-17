using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GenericCampaignMasterLib;

namespace Playground
{
    static class Program
    {
        public static CampaignController m_objCampaign;

        public static void Global_onStatus(string strText)
        {
            frmStatus.Status(strText);
        }
        
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);                        

            Program.m_objCampaign = new CampaignBuilderSchach().buildNew();                
            Program.m_objCampaign.onStatus += new Field.delStatus(Global_onStatus);
            Application.Run(new frmGameMainForm());            
        }
    }
}
