using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;
using GcmlDataAccess;


namespace GcmlWebApi.Controllers
{
    [Authorize]
    public class CampaignController : ApiController
    {
        private ICampaignRepository data;

        public CampaignController()
        {
            data = new CampaignRepositorySql();
        }

        public CampaignController(ICampaignRepository data)
        {
            this.data = data;
        }

        // GET api/campaign
        public CampaignListDto Get()
        {
            var lst = data.getCampaignsForPlayer(User.Identity.Name);
            CampaignListDto campaignList = new CampaignListDto() {campaignList = lst };
            return campaignList;
        }

        // GET api/campaign/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/campaign
        public void Post([FromBody]string value)
        {
        }

        // PUT api/campaign/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/campaign/5
        public void Delete(int id)
        {
        }
    }
}
