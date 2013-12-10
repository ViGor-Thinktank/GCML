using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GcmlDataAccess.SQL.GcmlDataTableAdapters;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;
using GcmlDataAccess;
using Microsoft.Ajax.Utilities;


namespace GcmlWebApi.Controllers
{
    public class UnitController : ApiController
    {
        private ICampaignRepository data;

        public UnitController()
        {
            data = new CampaignRepositorySql();
        }

        public UnitController(ICampaignRepository data)
        {
            this.data = data;
        }

        // GET api/unit
        public UnitListDto Get(string campaignId, string sektorId="")
        {
            GenericCampaignMasterLib.CampaignController controller = data.getCampaignController(campaignId);
            Player player = controller.Player_getByName(User.Identity.Name);
            var units = controller.Player_getUnitsForPlayer(player);
            List<UnitInfo> lstUnitInfos = units.Select(u => controller.CampaignEngine.getUnitInfo(u)).ToList();

            return new UnitListDto() {unitList = lstUnitInfos};
        }

        // GET api/unit/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/unit
        public void Post([FromBody]string value)
        {
        }

        // PUT api/unit/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/unit/5
        public void Delete(int id)
        {
        }
    }
}
