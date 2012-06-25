using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using GenericCampaignMasterLib;

namespace CampaignMasterWeb
{
    public struct CampaignMasterClientKeys
    {
        public const string CAMPAIGNSTATE = "campaignstate";
        public const string CAMPAIGNID = "campaignid";
        public const string CONTEXTPLAYERID = "contextplayerid";
    }

    public partial class CampaignMasterClientTest : System.Web.UI.Page
    {
        public static CampaignController getCampaignController(HttpSessionState state)
        {
            CampaignController controller;  
            string statekey = (string) state[CampaignMasterClientKeys.CAMPAIGNSTATE];
            if (String.IsNullOrEmpty(statekey))
            {
                // Keine State vorhanden - neu erzeugen
                CampaignBuilderTicTacTod builder = new CampaignBuilderTicTacTod();
                controller = builder.buildNew();

                // Ersten State speichern
                string newkey = controller.saveCurrentGameState();
                state[CampaignMasterClientKeys.CAMPAIGNSTATE] = newkey;
            }
            else
            {
                controller = new CampaignController();
                controller.restoreGameState(statekey);
            }

            return controller;
        }

        public static Player getContextPlayer(HttpSessionState state)
        {
            //string contextplayerid = (string) state[CampaignMasterClientKeys.CONTEXTPLAYERID];
            //CampaignController controller = CampaignMasterClientTest.getCampaignController(state);
            //Player contextPlayer = controller.g
            throw new NotImplementedException();
           
        }


        protected void Page_Load(object sender, EventArgs e)
        {
         
        }
    }
}