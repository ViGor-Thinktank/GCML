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

        public static List<frmPlayerMainForm> lisForms = new List<frmPlayerMainForm>();

        public static void Global_onStatus(string strText)
        {
            frmStatus.Status(strText);
        }

        public static clsCampaignInfo objinf;

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Program.objinf = new clsCampaignInfo();
                

            if (MessageBox.Show("laden?", "", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                Program.m_objCampaign = new CampaignBuilderSchach().buildNew();
            }
            else
            {

                Program.objinf.load();
                Program.m_objCampaign = new CampaignBuilderSchach().restoreFromDb(objinf.strCCKey, objinf.strSaveKey);
                List<Player> listPlayers = Program.m_objCampaign.getPlayerList();
                foreach (Player newP in listPlayers)
                {
                    frmPlayerMainForm frmP = new frmPlayerMainForm();
                    frmP.myPlayer = newP;
                    frmP.button1.Visible = false;
                    frmP.Text = newP.Playername;
                    frmP.Show();
                    lisForms.Add(frmP);
                }



            }
            Program.m_objCampaign.onStatus += new Field.delStatus(Global_onStatus);


            frmGameMainForm frm = new frmGameMainForm();
            frm.Text = Program.m_objCampaign.CampaignKey;
            Application.Run(frm);            
        }
    }
}
