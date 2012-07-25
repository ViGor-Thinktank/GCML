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

    public partial class GcmlClientPage : System.Web.UI.Page
    {


        protected void Page_PreInit(object sender, EventArgs e)
        {
            GcmlClientWeb.getCampaignController(this.Session);            // Aufrufen stellt sicher dass CampaignController vorhanden ist
        }

        protected void Page_Load(object sender, EventArgs e)
        {

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
            string id = dropDownPlayer.SelectedValue;
            GcmlClientWeb.setCurrentPlayer(id, this.Session);
            FieldControl1.drawPlayerContext();
        }
    }

   
}