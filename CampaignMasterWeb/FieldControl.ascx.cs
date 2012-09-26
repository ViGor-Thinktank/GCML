using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using GenericCampaignMasterLib;          
//using GcmlWebService;
using CampaignMasterWeb.GcmlWsReference;
using System.Web.Script.Serialization;

namespace CampaignMasterWeb
{
    public partial class FieldControl : System.Web.UI.UserControl
    {
        private JavaScriptSerializer m_serializer = new JavaScriptSerializer();

        protected void Page_Init(object sender, EventArgs e)
        {
            drawForm();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            drawPlayerContext();
        }

        /// <summary>
        /// Erzeugt die statischen Formularelemente für die Spielfeldansicht (alles von der Spielersicht abhängt)
        /// </summary>
        private void drawForm()
        {
            if (panelField.Controls.Count > 1)
                return;

            string campaignId = (string)Session[GcmlClientKeys.CAMPAIGNID];
            string playerId = (string)Session[GcmlClientKeys.CONTEXTPLAYERID];

            List<SektorControl> sektorStack = new List<SektorControl>();
            CampaignMasterService service = StartMenu.getService(this.Session);

            clsSektorKoordinaten fieldKoord = service.getFieldKoord(campaignId);

            // Feld zeilenweise erstellen
            Table tab = new Table();
            for (int y = 0; y <= fieldKoord.Y - 1; y++)
            {
                tab.Rows.Add(new TableRow());

                for (int x = 0; x <= fieldKoord.X - 1; x++)
                {
                    clsSektorKoordinaten getkoord = new clsSektorKoordinaten();
                    getkoord.X = x;
                    getkoord.Y = y;
                    getkoord.Z = 0;
                    getkoord.XSpecified = true;
                    getkoord.YSpecified = true;
                    getkoord.ZSpecified = true;

                    SektorInfo sektor = service.getSektor(campaignId, getkoord);

                    TableCell c = new TableCell();
                    tab.Rows[y].Cells.Add(c);

                    SektorControl sc = (SektorControl)Page.LoadControl("SektorControl.ascx");
                    sc.Sektor = sektor;
                    c.Controls.Add(sc);

                    sektorStack.Add(sc);
                }
            }
            
            panelField.Controls.Add(tab);

            Session[GcmlClientKeys.SEKTORSTACK] = sektorStack;
        }

        public void drawPlayerContext()
        {
            List<SektorControl> sektorStack = (List<SektorControl>) Session[GcmlClientKeys.SEKTORSTACK] ;
            if (sektorStack == null)
                return;

            foreach (SektorControl sektorCtrl in sektorStack)
                sektorCtrl.drawContext();
        }
    }
}