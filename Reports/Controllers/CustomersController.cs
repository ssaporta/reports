using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using Dapper;

namespace Reports.Controllers
{
    public class CustomersController : Controller
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OOSDatabase"].ConnectionString;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lookup()
        {
            Reports.Models.CustomerParamsModel customerParams = new Reports.Models.CustomerParamsModel();
            return View(customerParams);
        }

        public ActionResult LookupResults(Reports.Models.CustomerParamsModel customerParams)
        {
            string query = "select top 100 cu.CustomerID, cu.FName, cu.LName, cu.LoginEmail, cu.Phone1 as Phone from Customers cu where";
            bool firstClause = true;
            if (customerParams.CustomerID != null && customerParams.CustomerID.Trim().Length > 0)
            {
                query += " cu.CustomerID=" + customerParams.CustomerID.Trim();
                firstClause = false;
            }
            if (customerParams.Date != null && customerParams.Date.Trim().Length > 0)
            {
                DateTime d = DateTime.Parse(customerParams.Date.Trim());
                string startDate = "'" + d.Year + "-" + d.Month + "-" + d.Day + "'";
                d = d.AddDays(1);
                string endDate = "'" + d.Year + "-" + d.Month + "-" + d.Day + "'";
                query += (firstClause ? "" : " and") + " cu.CreateDate between " + startDate + " and " + endDate;
                firstClause = false;
            }
            if (customerParams.LoginEmail != null && customerParams.LoginEmail.Trim().Length > 0)
            {
                query += (firstClause ? "" : " and") + " cu.LoginEmail like '" + customerParams.LoginEmail.Trim() + "%'";
                firstClause = false;
            }
            if (customerParams.LastName != null && customerParams.LastName.Trim().Length > 0)
            {
                query += (firstClause ? "" : " and") + " cu.LName like '" + customerParams.LastName.Trim() + "%'";
                firstClause = false;
            }
            query += " order by LoginEmail";

            List<Reports.Models.CustomerModel> customers = new List<Models.CustomerModel>();
            if (!firstClause)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    customers = conn.Query<Reports.Models.CustomerModel>(query).ToList();
                }
            }
            return View(customers);
        }

        public ActionResult LookupDetails(int customerID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "select cu.CreateDate, cu.CustomerID, cu.FName, cu.IsActive, cu.LName, cu.LoginEmail";
                query += ", cu.OptIn, cu.Phone1 as Phone, cu.SendCoupons";
                query += ", case isnull(cu.RestaurantID, 0) when 0 then '' else c.Name + ' (#' + convert(varchar, r.CampusID) + ')' end as Campus";
                query += ", case isnull(cu.RestaurantID, 0) when 0 then '' else r.Name + ' (#' + convert(varchar, cu.RestaurantID) + ')' end as Restaurant";
                query += ", case isnull(cu.AffiliateID, 0) when 0 then '' else a.Name + ' (#' + convert(varchar, cu.AffiliateID) + ')' end as Affiliate";
                query += " from Customers cu";
                query += " left outer join Affiliates a on a.AffiliateID = isnull(cu.AffiliateID, 0)";
                query += " left outer join Restaurants r on r.RestaurantID = isnull(cu.RestaurantID, 0)";
                query += " left outer join Campuses c on c.CampusID = isnull(r.CampusID, 0)";
                query += " where cu.CustomerID = " + customerID;
                Reports.Models.CustomerModel customer = conn.Query<Reports.Models.CustomerModel>(query).ToList().First();
                return View(customer);
            }
        }

        public ActionResult Reports()
        {
            return View();
        }
    }
}
