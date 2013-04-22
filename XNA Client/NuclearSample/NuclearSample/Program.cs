using System;
using GenericCampaignMasterLib; 

namespace GCML_XNA_Client
{
    static class Program
    {
        public static CampaignController m_objCampaign;
        public static GCML.clsCampaignInfo objCampaignState;

        static void Main( string[] args )
        {
            // This sample uses an ApplicationMutex to prevent running the game multiple times at once.
            // Useful for games where progress or settings might be overwritten if two instances are running at the same time.
            using( NuclearWinter.ApplicationMutex mutex = new NuclearWinter.ApplicationMutex() )
            {
                if( mutex.HasHandle )
                {
                    using( NuclearSampleGame game = new NuclearSampleGame() )
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

