using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reports.Controllers
{
    [AllowAnonymous]
    public class UnauthorizedController : Controller
    {
        //
        // GET: /Unauthorized/

        public ActionResult Index()
        {
            return View();
        }

    }
}
