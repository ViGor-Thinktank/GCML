using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;
using GcmlDataAccess;


namespace GcmlClientWebMVC.Controllers
{
    public class GameController : ApiController
    {
        CampaignBuilder campaignCtx  = CampaignBuilder.Instance;

        // GET api/game
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/game/5
        public FieldInfo Get(string id)
        {
            CampaignController controller = campaignCtx.getCurrentGame(id);
            return new FieldInfo()
            {
                Campaign = controller.Campaign_getInfo(),
                FieldKoord = controller.CampaignEngine.FieldField.FieldDimension,
                ListSektors = controller.CampaignEngine.FieldField.getSektorList().Select(s => s.getInfo())
            };
        }

        // POST api/game
        public void Post([FromBody]string value)
        {
        }

        // PUT api/game/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/game/5
        public void Delete(int id)
        {
        }
    }
}
