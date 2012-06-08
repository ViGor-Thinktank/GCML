using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GenericCampaignMasterLib;

namespace Playground
{
    static class Program
    {
        public static CampaignEngine m_objEngine;

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

            Program.m_objEngine = new CampaignEngine();

            if (MessageBox.Show("Schachbrett?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                m_objEngine.FieldField = new Field_Schachbrett(5, 5);
            }
            else
            {
                m_objEngine.FieldField = new Field_Schlauch(8);
            }

            m_objEngine.FieldField.onFieldStatus += new Field.delStatus(Global_onStatus);
            m_objEngine.onEngineStatus += new Field.delStatus(Global_onStatus);

            
            Application.Run(new frmPlayerMainForm());
        }
    }
}
