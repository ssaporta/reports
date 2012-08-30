using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using Dapper;

namespace Reports.Controllers
{
    public class OrdersController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lookup()
        {
            Reports.Models.OrderParamsModel orderParams = new Reports.Models.OrderParamsModel();
            return View(orderParams);
        }

        public ActionResult LookupResults(Reports.Models.OrderParamsModel orderParams)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OOSDatabase"].ConnectionString;
            string query = "select top 100 * from AllOrders o join Customers cu on cu.CustomerID = o.CustomerID join Restaurants r on r.RestaurantID = o.RestaurantID join Campuses c on c.CampusID = r.CampusID where";
            bool firstClause = true;
            if (orderParams.OrderID != null && orderParams.OrderID.Trim().Length > 0)
            {
                query += " o.OrderID=" + orderParams.OrderID.Trim();
                firstClause = false;
            }
            if (orderParams.Date != null && orderParams.Date.Trim().Length > 0)
            {
                DateTime d = DateTime.Parse(orderParams.Date.Trim());
                string startDate = "'" + d.Year + "-" + d.Month + "-" + d.Day + "'";
                d = d.AddDays(1);
                string endDate = "'" + d.Year + "-" + d.Month + "-" + d.Day + "'";
                query += (firstClause ? "" : " and") + " o.OrderPlacedDate between " + startDate + " and " + endDate;
                firstClause = false;
            }
            if (orderParams.LoginEmail != null && orderParams.LoginEmail.Trim().Length > 0)
            {
                query += (firstClause ? "" : " and") + " cu.LoginEmail like '" + orderParams.LoginEmail.Trim() + "%'";
                firstClause = false;
            }
            if (orderParams.LastName != null && orderParams.LastName.Trim().Length > 0)
            {
                query += (firstClause ? "" : " and") + " cu.LName like '" + orderParams.LastName.Trim() + "%'";
                firstClause = false;
            }

            List<Reports.Models.OrderModel> orders = new List<Models.OrderModel>();
            if (!firstClause)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    orders = conn.Query<Reports.Models.OrderModel>(query).ToList();
                }
            }
            return View(orders);
        }

        public ActionResult Reports()
        {
            return View();
        }
    }
}
