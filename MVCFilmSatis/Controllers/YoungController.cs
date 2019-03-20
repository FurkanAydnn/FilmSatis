using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCFilmSatis.Controllers
{
    public class YoungController : Controller
    {
        [OnlyYoungAndAdmin] //Action Filter attribute
        // GET: Young
        public ActionResult Index()
        {
            return View();
        }
    }
}