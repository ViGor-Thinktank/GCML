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
        //
        // GET: /CampaignData/

        public ActionResult Index()
        {
            string playername = User.Identity.Name;
            string playerid = CampaignBuilder.Instance.DataManager.getPlayerId(playername);

            List<CampaignInfo> lstCampaigns = CampaignBuilder.Instance.DataManager.getRunningPlayerCampaigns(playerid);

            return View(lstCampaigns);
        }

        //
        // GET: /CampaignData/Details/5

        public ActionResult Details(int id)
        {
            return View();
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
                // TODO: Add insert logic here

                string name = collection.Get("campaignname");
                int x = -1;
                int y = -1;

                Int32.TryParse(collection.Get("fieldx"), out x);
                Int32.TryParse(collection.Get("fieldy"), out y);

                string newcampaignid = CampaignBuilder.Instance.DataManager.createNewCampaign(name, new clsSektorKoordinaten() { X = x, Y = y });

                // Angemeldeten Spieler hinzufügen
                PlayerInfo pinfo = CampaignBuilder.Instance.DataManager.getPlayerByName(User.Identity.Name);

                CampaignController ctrl = CampaignBuilder.Instance.DataManager.getController(newcampaignid);
                ctrl.CampaignEngine.addPlayer(new Player(pinfo) { });

                CampaignBuilder.Instance.DataManager.safeCampaignState(ctrl.CampaignEngine.getState());

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /CampaignData/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /CampaignData/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
