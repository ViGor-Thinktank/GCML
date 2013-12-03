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
    /// <summary>
    /// GameController liefert die Spielfelddaten für eine laufende Kampagne (WebApi).
    /// </summary>
    public class GameController : ApiController
    {
        //CampaignBuilder campaignCtx  = CampaignBuilder.Instance;
        //IGcmlDataManager dataManager = CampaignBuilder.Instance.DataManager;
        CampaignRepositorySql data = new CampaignRepositorySql();

        // GET api/game
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/game/5
        public FieldInfo Get(string id)
        {
            CampaignController controller = data.getCampaignController(id);

            // SektorInfo in 2 dimensionalen Array liefern
            int x = controller.CampaignEngine.FieldField.FieldDimension.X;
            int y = controller.CampaignEngine.FieldField.FieldDimension.Y;
            
            SektorInfo[,] sektorArr = new SektorInfo[y, x];
            
            for (int iy = 0; iy < y; iy++)
            {
                for (int jx = 0; jx < x; jx++)
                {
                    var sektor = controller.CampaignEngine.FieldField.get(new clsSektorKoordinaten(jx, iy){});
                    sektorArr[iy, jx] = sektor.getInfo();
                }
            }

            var field = new FieldInfo()
            {
                Campaign = controller.Campaign_getInfo(),
                FieldKoord = controller.CampaignEngine.FieldField.FieldDimension,
                ListSektors = controller.CampaignEngine.FieldField.getSektorList().Select(s => s.getInfo()),
                SektorField = sektorArr
            };

            return field;
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
