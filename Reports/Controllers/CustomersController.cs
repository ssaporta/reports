using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using Dapper;
using System.Data;

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
            Models.ParamsModel myParams = new Models.ParamsModel();
            return View(myParams);
        }

        public ActionResult LookupResults(Models.ParamsModel myParams)
        {
            if (ModelState.IsValid)
            {
                string query = "select top 100 cu.CustomerID, cu.FName, cu.LName, cu.LoginEmail, cu.Phone1 as Phone from Customers cu where";
                bool firstClause = true;
                if (myParams.ID != null && myParams.ID.Trim().Length > 0)
                {
                    query += " cu.CustomerID=" + myParams.ID.Trim();
                    firstClause = false;
                }
                if (myParams.Date != null && myParams.Date.Trim().Length > 0)
                {
                    DateTime d = DateTime.Parse(myParams.Date.Trim());
                    string startDate = "'" + d.Year + "-" + d.Month + "-" + d.Day + "'";
                    d = d.AddDays(1);
                    string endDate = "'" + d.Year + "-" + d.Month + "-" + d.Day + "'";
                    query += (firstClause ? "" : " and") + " cu.CreateDate between " + startDate + " and " + endDate;
                    firstClause = false;
                }
                if (myParams.LoginEmail != null && myParams.LoginEmail.Trim().Length > 0)
                {
                    query += (firstClause ? "" : " and") + " cu.LoginEmail like '" + myParams.LoginEmail.Trim() + "%'";
                    firstClause = false;
                }
                if (myParams.LastName != null && myParams.LastName.Trim().Length > 0)
                {
                    query += (firstClause ? "" : " and") + " cu.LName like '" + myParams.LastName.Trim() + "%'";
                    firstClause = false;
                }
                query += " order by LoginEmail";

                List<Models.CustomerModel> customers = new List<Models.CustomerModel>();
                if (!firstClause)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        customers = conn.Query<Models.CustomerModel>(query).ToList();
                    }
                }
                return View(new Tuple<Models.ParamsModel, List<Models.CustomerModel>>(myParams, customers));
            }
            return View("Lookup");
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
                Models.CustomerModel customer = conn.Query<Models.CustomerModel>(query).ToList().First();
                return View(customer);
            }
        }

        public ActionResult Reports()
        {
            return View();
        }

        public ActionResult CustomerList(Models.CustomerListModel customerListModel)
        {
            List<Models.CustomerListModel> customers = new List<Models.CustomerListModel>();
            customerListModel.Output = new List<Models.CustomerModel>();
            if (customerListModel.CampusID != 0)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    customerListModel.Output = conn.Query<Models.CustomerModel>(
                        "CustomerList", new { campusID = customerListModel.CampusID, startDate = customerListModel.StartDate, endDate = customerListModel.EndDate }
                        , commandType: CommandType.StoredProcedure
                    ).ToList();
                }
            }
            return View(customerListModel);
        }
    }
}
