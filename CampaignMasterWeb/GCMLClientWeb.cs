using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using GenericCampaignMasterLib;
using CampaignMasterWeb;            // Nur zum Testen des Ws. Todo: Aufruf über Webverweis

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
            string id = (string)state [GcmlClientKeys.CONTEXTPLAYERID];
            if (string.IsNullOrEmpty(id))
                return null;

            Player player = GcmlClientWeb.getCampaignController(state).getPlayer(id);
            return player;
        }

        public static CampaignController getCampaignController(HttpSessionState state)
        {
            // TEST --> auf Webverweis umstellen
            GcmlWebService.CampaignMasterService gcmlservice = (GcmlWebService.CampaignMasterService)state ["testservice"];
            if (gcmlservice == null)
            {
                gcmlservice = new GcmlWebService.CampaignMasterService();
                state ["testservice"] = gcmlservice;
            }

            CampaignController controller;
            //CampaignBuilderTicTacTod cbttt = new CampaignBuilderTicTacTod();
            string statekey = (string)state [GcmlClientKeys.CAMPAIGNSTATE];
            string campaignkey = (string)state [GcmlClientKeys.CAMPAIGNID];
            string currentPlayerId = (string)state [GcmlClientKeys.CONTEXTPLAYERID];

            if (string.IsNullOrEmpty(currentPlayerId))      // Spieler muss eingeloggt sein
                return null;

            if (String.IsNullOrEmpty(statekey) || String.IsNullOrEmpty(campaignkey))
            {
                //controller = cbttt.buildNew();      // Keine State vorhanden - neu erzeugen
                string newCampaignId = gcmlservice.createNewCampaign(
                    currentPlayerId,
                    new clsSektorKoordinaten(5, 5).ToString()
                );
                state [GcmlClientKeys.CAMPAIGNID] = newCampaignId;


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
