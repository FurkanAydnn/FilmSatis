using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MVCFilmSatis.Controllers
{
    public class TranslateController : Controller
    {
        // GET: Translate
        public ActionResult Index(string id, string page)
        {
            HttpCookie langCookie = new HttpCookie("lang");
            langCookie.Value = id;
            Response.Cookies.Add(langCookie);

            return Redirect(page);
        }
    }
}