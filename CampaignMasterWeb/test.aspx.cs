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
        public const string CAMPAIGNCONTROLLER = "campaigncontroller";
    }

    public partial class CampaignMasterClientTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            getCampaignController(this.Session);            // Aufrufen stellt sicher dass CampaignController vorhanden ist
        }

        public static CampaignController getCampaignController(HttpSessionState state)
        {
            CampaignController controller;  
            CampaignBuilderTicTacTod cbttt = new CampaignBuilderTicTacTod();
            string statekey = (string) state[CampaignMasterClientKeys.CAMPAIGNSTATE];
            string campaignkey = (string)state[CampaignMasterClientKeys.CAMPAIGNID];

            if (String.IsNullOrEmpty(statekey) || String.IsNullOrEmpty(campaignkey))
            {
                controller = cbttt.buildNew();      // Keine State vorhanden - neu erzeugen
            }
            else if ((CampaignController)state[CampaignMasterClientKeys.CAMPAIGNCONTROLLER] != null)
            {
                controller = (CampaignController)state[CampaignMasterClientKeys.CAMPAIGNCONTROLLER];
            }
            else
            {
                controller = cbttt.restoreFromDb(campaignkey, statekey);
            }

            // Ersten State speichern
            string newkey = controller.saveCurrentGameState();
            state[CampaignMasterClientKeys.CAMPAIGNSTATE] = newkey;
            state[CampaignMasterClientKeys.CAMPAIGNID] = controller.CampaignKey;
            state[CampaignMasterClientKeys.CAMPAIGNCONTROLLER] = controller;
            return controller;
        }

        public static Field getField(HttpSessionState state)
        {
            CampaignController controller = CampaignMasterClientTest.getCampaignController(state);
            Field field = controller.campaignEngine.FieldField;
            return field;
        }

        public static Player getContextPlayer(HttpSessionState state)
        {
            string contextplayerid = (string) state[CampaignMasterClientKeys.CONTEXTPLAYERID];
            CampaignController controller = CampaignMasterClientTest.getCampaignController(state);
            Player contextPlayer = controller.getPlayer(contextplayerid);
            return contextPlayer;
        }
    }
}