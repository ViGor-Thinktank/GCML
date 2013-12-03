using System;
using GenericCampaignMasterLib; 
using GcmlDataAccess;

namespace GCML_XNA_Client
{
    static class Program
    {
        public static CampaignController m_objCampaign;
        public static GCML.clsCampaignInfo objCampaignState;
        public static CampaignRepositorySql gcmlData;

        static void Main( string[] args )
        {
            // This sample uses an ApplicationMutex to prevent running the game multiple times at once.
            // Useful for games where progress or settings might be overwritten if two instances are running at the same time.
            gcmlData = new CampaignRepositorySql();
            using( NuclearWinter.ApplicationMutex mutex = new NuclearWinter.ApplicationMutex() )
            {
                if( mutex.HasHandle )
                {
                    using( GCML_XNA_Client game = new GCML_XNA_Client() )
                    {
                        game.Run();
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show( "An instance of NuclearSample is already running.", "Could not start NuclearSample", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Stop );
                }
            }
        }
    }
}

