using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using GenericCampaignMasterLib;         // Todo: Keine Referenz auf die Lib im Client - nur noch WebService
using CampaignMasterWeb.GcmlWsReference;

namespace CampaignMasterWeb
{
    public partial class GcmlClientPage : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string playerid = (string)Session[GcmlClientKeys.CONTEXTPLAYERID];
            string campaignid = (string)Session[GcmlClientKeys.CAMPAIGNID];

            lbPlayer.Text = playerid;
            lbCampaign.Text = campaignid;
        }
    }

   
}