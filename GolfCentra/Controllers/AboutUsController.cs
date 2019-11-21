using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GolfCentra.Controllers
{
    /// <summary>
    /// About Us Page Of APP
    /// </summary>
    public class AboutUsController : Controller
    {
        // GET: AboutUs
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DrivingRange() {
            return View();
        }

        public ActionResult Membership()
        {
            return View();
        }

        public ActionResult ClubHouse()
        {
            return View();
        }
        public ActionResult FACILITIES()
        {
            return View();
                
        }

    }
}