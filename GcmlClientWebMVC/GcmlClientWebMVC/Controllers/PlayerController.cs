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
    public class PlayerController : ApiController
    {
        CampaignBuilder campaignCtx = CampaignBuilder.Instance;

        // GET api/player
        public IEnumerable<PlayerInfo> Get()
        {
            return campaignCtx.DataManager.getPlayerList().Select(kv => new PlayerInfo() { playerId = kv.Value.playerId, playerName = kv.Value.playerName }).ToList<PlayerInfo>();
        }

        // GET api/player/5
        public PlayerInfo Get(string id)
        {
            var result = campaignCtx.DataManager.getPlayerList().Where(kv => kv.Value.playerId == id).Select(kv => kv.Value).FirstOrDefault();
            return result;
        }

        // POST api/player
        public void Post([FromBody]PlayerInfo pinfo)
        {
            if(String.IsNullOrEmpty(pinfo.playerId))
                campaignCtx.DataManager.addPlayer(new PlayerInfo() { playerName = pinfo.playerName });
        }

        // PUT api/player/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/player/5
        public void Delete(int id)
        {
        }
    }
}
