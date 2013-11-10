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
        IGcmlDataManager dataManager = CampaignBuilder.Instance.DataManager;


        //
        // GET: /CampaignData/

        public ActionResult Index()
        {
            string playername = User.Identity.Name;
            string playerid = dataManager.getPlayerId(playername);

            List<CampaignInfo> lstCampaigns  = dataManager.getRunningPlayerCampaigns(playerid);

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
            CampaignInfo cmpinf = dataManager.getController(id).Campaign_getInfo();
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

                string newcampaignid = dataManager.createNewCampaign(name, new clsSektorKoordinaten() { X = x, Y = y });

                // Angemeldeten Spieler hinzufügen
                PlayerInfo pinfo = dataManager.getPlayerByName(User.Identity.Name);

                CampaignController ctrl = dataManager.getController(newcampaignid);
                ctrl.CampaignEngine.addPlayer(new Player(pinfo) { });

                dataManager.safeCampaignState(ctrl.CampaignEngine.getState());

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
            CampaignInfo cmpinf = dataManager.getController(id).Campaign_getInfo();
            return PartialView("_Edit", cmpinf);
        }

        //
        // POST: /CampaignData/Edit/5

        [HttpPost]
        public ActionResult Edit(CampaignInfo cmpinfo)
        {
            try
            {
                dataManager.updateCampaign(cmpinfo);

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
