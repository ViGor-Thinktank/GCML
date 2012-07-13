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
    public struct GcmlClientKeys
    {
        public const string CAMPAIGNSTATE = "campaignstate";
        public const string CAMPAIGNID = "campaignid";
        public const string CONTEXTPLAYERID = "contextplayerid";
        public const string CAMPAIGNCONTROLLER = "campaigncontroller";
        public const string CONTEXTUNITID = "contextunitid";
        public const string CONTEXTCOMMANDLIST = "contextcommandlist";
        public const string SEKTORSTACK = "sektorstack";
    }

    public partial class GcmlClientPage : System.Web.UI.Page
    {


        protected void Page_PreInit(object sender, EventArgs e)
        {
            GcmlClientWeb.getCampaignController(this.Session);            // Aufrufen stellt sicher dass CampaignController vorhanden ist
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void setCurrentPlayer()
        {
            string id = dropDownPlayer.SelectedValue;
            Player player = GcmlClientWeb.getCampaignController(this.Session).getPlayer(id);
            Session[GcmlClientKeys.CONTEXTPLAYERID] = id;
        }

        protected Player getCurrentPlayer()
        {
            string id = (string)Session[GcmlClientKeys.CONTEXTPLAYERID];
            Player player = GcmlClientWeb.getCampaignController(this.Session).getPlayer(id);
            return player;
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            CampaignController controller = GcmlClientWeb.getCampaignController(this.Session);
            string id = controller.getPlayerList()[0].Id;
            
            // Player ID im Client ViewState speichern
            ViewState[GcmlClientKeys.CONTEXTPLAYERID] = id;


        }

        protected void Button2_Click(object sender, EventArgs e)
        {

        }

        protected void btnSelectPlayer_Click(object sender, EventArgs e)
        {
            setCurrentPlayer();
            FieldControl1.drawPlayerContext();
        }
    }

    public class GcmlClientWeb
    {
        public static CampaignController getCampaignController(HttpSessionState state)
        {
            CampaignController controller;
            CampaignBuilderTicTacTod cbttt = new CampaignBuilderTicTacTod();
            string statekey = (string)state[GcmlClientKeys.CAMPAIGNSTATE];
            string campaignkey = (string)state[GcmlClientKeys.CAMPAIGNID];

            if (String.IsNullOrEmpty(statekey) || String.IsNullOrEmpty(campaignkey))
            {
                controller = cbttt.buildNew();      // Keine State vorhanden - neu erzeugen
            }
            else if ((CampaignController)state[GcmlClientKeys.CAMPAIGNCONTROLLER] != null)
            {
                controller = (CampaignController)state[GcmlClientKeys.CAMPAIGNCONTROLLER];
            }
            else
            {
                controller = cbttt.restoreFromDb(campaignkey, statekey);
            }

            // Ersten State speichern
            string newkey = controller.saveCurrentGameState();
            state[GcmlClientKeys.CAMPAIGNSTATE] = newkey;
            state[GcmlClientKeys.CAMPAIGNID] = controller.CampaignKey;
            state[GcmlClientKeys.CAMPAIGNCONTROLLER] = controller;

            #if DEBUG
            //controller.restoreGameState(newkey);

            # endif

            return controller;
        }

        public static Field getField(HttpSessionState state)
        {
            CampaignController controller = GcmlClientWeb.getCampaignController(state);
            Field field = controller.campaignEngine.FieldField;
            return field;
        }

        public static IUnit getUnitById(string unitId, HttpSessionState state)
        {
            CampaignController controller = GcmlClientWeb.getCampaignController(state);
            IUnit unit = controller.getUnit(unitId);
            return unit;
        }
			


    }
}