using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.SqlClient;
using Dapper;

namespace Reports
{
    public sealed class Authentication : FilterAttribute, IAuthorizationFilter
    {
        public Authentication()
        {
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                // ?token=XtsMJRrUtGiytKZ2b6H0eBgVzfNS5hr73kapmZiA4GDENdrRNRXSlis7BFrp4mtr
                string tokenStatus = "ok";
                string token = "";

                if (HttpContext.Current.Request["token"] != null)
                {
                    string encryptedToken = HttpContext.Current.Request["token"].ToString();
                    try
                    {
                        Crypto crypto = new Crypto(true);
                        token = crypto.Decrypt(HttpContext.Current.Server.HtmlEncode(encryptedToken));
                        char[] spaceSeparator = { ' ' };
                        string[] a = token.Split(spaceSeparator);
                        var timestamp = new DateTime(long.Parse(a[0]));
                        if (timestamp.AddMinutes(120) < DateTime.Now)
                        {
                            tokenStatus = "Expired token";
                            token = "";
                        }
                        else
                            HttpContext.Current.Session["token"] = token;
                    }
                    catch (Exception ex)
                    {
                        tokenStatus = ex.Message;
                    }
                }
                else
                {
                    if (HttpContext.Current.Session["token"] != null)
                        token = HttpContext.Current.Session["token"].ToString();
                    else
                        tokenStatus = "Missing token";
                }

                if (tokenStatus != "ok")
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary { { "action", "Index" }, { "controller", "Unauthorized" }, { "tokenStatus", tokenStatus } }
                    );
                }
            }
        }

        public static string GetTerritoryIDsFromToken()
        {
            try
            {
                string token = HttpContext.Current.Session["token"].ToString();
                char[] spaceSeparator = { ' ' };
                string[] a = token.Split(spaceSeparator);
                return a[1];
            }
            catch
            {
                return "";
            }
        }

        public static string GetTerritoryNamesFromToken()
        {
            try
            {
                string territoryIDs = GetTerritoryIDsFromToken();
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OOSDatabase"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "select Name from Campuses with (nolock) where CampusID in (" + territoryIDs + ")";
                    conn.Open();
                    IEnumerable<string>territoryNames = conn.Query<string>(query);
                    string s = "";
                    foreach (string territoryName in territoryNames)
                    {
                        if (s.Length > 0)
                            s += ", ";
                        s += territoryName;
                    }
                    return s;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}