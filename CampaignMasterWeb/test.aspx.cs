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
    public partial class CampaignMasterClientTest : System.Web.UI.Page
    {
        const string KEY_CAMPAIGNSTATE = "campaignstate";

        public static CampaignController getCampaignController(HttpSessionState state)
        {
            CampaignController controller;  
            string statekey = (string) state[KEY_CAMPAIGNSTATE];
            if (String.IsNullOrEmpty(statekey))
            {
                // Keine State vorhanden - neu erzeugen
                CampaignBuilderTicTacTod builder = new CampaignBuilderTicTacTod();
                controller = builder.buildNew();

                // Ersten State speichern
                string newkey = controller.saveCurrentGameState();
                state[KEY_CAMPAIGNSTATE] = newkey;
            }
            else
            {


                controller = new CampaignController();
                controller.restoreGameState(statekey);
            }

            return controller;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
         
        }
    }
}