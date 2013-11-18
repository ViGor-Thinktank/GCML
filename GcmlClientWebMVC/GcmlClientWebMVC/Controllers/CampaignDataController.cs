using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GenericCampaignMasterLib;
using GenericCampaignMasterModel;
using GcmlDataAccess;

namespace GcmlClientWebMVC.Controllers
{
    [Authorize]
    public class CampaignDataController : Controller
    {
        private IGcmlDataAccess data = new GcmlDataAccessSqlServer();


        //
        // GET: /CampaignData/

        public ActionResult Index()
        {
            List <CampaignInfo> lstCampaigns = new List<CampaignInfo>();
            string playername = User.Identity.Name;

            var firstOrDefault = data.getPlayers().FirstOrDefault(p => p.playerName == playername);
            if (firstOrDefault != null)
            {
                string playerid = firstOrDefault.playerName;
                lstCampaigns = data.getCampaignsForPlayer(playerid);
            }

            return View(lstCampaigns);
        }

        //
        // GET: /CampaignData/Details/5

        public ActionResult Details(string id)
        {
            return View();
        }


        
        // Lädt die Spielfeldansicht für die Kampagne
        public ActionResult Board(string id)
        {
            CampaignInfo cmpinf = data.getCampaignController(id).Campaign_getInfo();
            return View(cmpinf);
        }


        //
        // GET: /CampaignData/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /CampaignData/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                string name = collection.Get("campaignname");
                int x = -1;
                int y = -1;

                Int32.TryParse(collection.Get("fieldx"), out x);
                Int32.TryParse(collection.Get("fieldy"), out y);

                CampaignInfo newCampaignInfo = new CampaignInfo()
                {
                    campaignId = "",
                    campaignName = name,
                    FieldDimension = new clsSektorKoordinaten() {X = x, Y = y}
                };

                string newcampaignid = data.createNewCampaign(newCampaignInfo);

                // Angemeldeten Spieler hinzufügen
                PlayerInfo pinfo = data.getPlayerInfo(User.Identity.Name);

                CampaignController ctrl = data.getCampaignController(newcampaignid);
                ctrl.CampaignEngine.addPlayer(new Player(pinfo) { });
                data.safeCampaignState(ctrl.CampaignEngine.getState());

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /CampaignData/Edit/5

        public ActionResult Edit(string id)
        {
            CampaignInfo cmpinf = data.getCampaignController(id).Campaign_getInfo();
            return PartialView("_Edit", cmpinf);
        }

        //
        // POST: /CampaignData/Edit/5

        [HttpPost]
        public ActionResult Edit(CampaignInfo cmpinfo)
        {
            try
            {
                CampaignController ctrl = data.getCampaignController(cmpinfo.campaignId);
                ctrl.CampaignEngine.CampaignName = cmpinfo.campaignName;

                data.safeCampaignState(ctrl.CampaignEngine.getState());

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /CampaignData/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /CampaignData/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
               
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
