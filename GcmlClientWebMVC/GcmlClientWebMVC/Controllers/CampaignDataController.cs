using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GcmlClientWebMVC.Controllers
{
    [Authorize]
    public class CampaignDataController : Controller
    {
        //
        // GET: /CampaignData/

        public ActionResult Index()
        {
            return View();
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
