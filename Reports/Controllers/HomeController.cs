using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reports.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // ?token=WYkI1mwUdLogAlWBn0J4TOOXmqVKeEC4lN6O%2bPWS5jvDzqjAMuddqbT0bgdp8sPE
            string tokenStatus = "ok";
            string token = "";

            if (Request["token"] != null)
            {
                string encryptedToken = Request["token"].ToString();
                try
                {
                    Crypto crypto = new Crypto(true);
                    token = crypto.Decrypt(Server.HtmlEncode(encryptedToken));
                    char[] spaceSeparator = { ' ' };
                    string[] a = token.Split(spaceSeparator);
                    var timestamp = new DateTime(long.Parse(a[0]));
                    if (timestamp.AddMinutes(120) < DateTime.Now)
                    {
                        tokenStatus = "Expired token";
                        token = "";
                    }
                    else
                        Session["token"] = token;
                }
                catch (Exception ex)
                {
                    tokenStatus = ex.Message;
                }
            }

            if (Session["token"] != null)
                token = Session["token"].ToString();
            else
                tokenStatus = "Missing token";

            ViewBag.TokenStatus = tokenStatus;
            ViewBag.Token = token;
            return View();
        }
    }
}
