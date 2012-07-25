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

    public class GcmlClientWeb
    {
        public static void setCurrentPlayer(string id, HttpSessionState state)
        {
            Player player = GcmlClientWeb.getCampaignController(state).getPlayer(id);
            state[GcmlClientKeys.CONTEXTPLAYERID] = id;
        }

        public static Player getCurrentPlayer(HttpSessionState state)
        {
            string id = (string)state[GcmlClientKeys.CONTEXTPLAYERID];
            Player player = GcmlClientWeb.getCampaignController(state).getPlayer(id);
            return player;
        }

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
