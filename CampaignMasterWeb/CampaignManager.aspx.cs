using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CampaignMasterWeb.GcmlWsReference;

namespace CampaignMasterWeb
{
    public partial class frmCampaignView : System.Web.UI.Page
    {

        
        private string strCCKey
        {
            get
            {
                return (string)Session[GcmlClientKeys.CAMPAIGNID];
            }
        }
        
        private CampaignMasterService m_service = null;
        private CampaignMasterService objGMCL 
        {
            get 
            {
              if (m_service == null)
                  m_service = StartMenu.getService(this.Session);
              return m_service;
            }
        }
                

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
               clsUnitType[] objCPMInfo = objGMCL.getCampaignInfo_UnitTypes(this.strCCKey);

               /*ListBox1.Items.Clear();

                foreach (clsUnitType x in objCPMInfo)
                {
                    ListItem newItem = new ListItem();
                    newItem.Text = x.strBez;
                    newItem.Value = x.ID.ToString();
                    ListBox1.Items.Add(newItem);
                }*/

               ListBox1.DataSource = objCPMInfo;
               ListBox1.DataTextField = "strBez";
               ListBox1.DataBind();
                

                
            }
            catch (Exception ex)
            {
                txtEx.Text = ex.ToString();
            
            }
        }

        protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {


                int intUnitTypeID = Convert.ToInt32(ListBox1.SelectedItem.Value);
                clsUnitType objUnitType = objGMCL.getCampaignInfo_UnitTypeByID(this.strCCKey, intUnitTypeID, true);


            }
            catch (Exception ex)
            {
                txtEx.Text = ex.ToString();
            }
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}