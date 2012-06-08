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

            if (MessageBox.Show("Schachbrett?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                Program.m_objCampaign = new CampaignController(new CampaignEngine(new Field_Schachbrett(5, 5)));                
            }
            else
            {
                Program.m_objCampaign = new CampaignController(new CampaignEngine(new Field_Schlauch(8)));                
            }
            Program.m_objCampaign.onStatus += new Field.delStatus(Global_onStatus);
            Application.Run(new frmGameMainForm());            
        }
    }
}
