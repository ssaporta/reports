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
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OOSDatabase"].ConnectionString;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lookup()
        {
            Reports.Models.ParamsModel myParams = new Reports.Models.ParamsModel();
            return View(myParams);
        }

        public ActionResult LookupResults(Reports.Models.ParamsModel myParams)
        {
            if (ModelState.IsValid)
            {
                string query = "select top 100 o.OrderID, o.PaymentMethod, o.OrderPlacedDate, o.OrderTotal, o.OrderStatus, case o.IsTakeout when 1 then 'Takeout' else 'Delivery' end as OrderType, cu.FName, cu.LName, cu.LoginEmail, c.Name as Campus, r.Name as Restaurant from AllOrders o join Customers cu on cu.CustomerID = o.CustomerID join Restaurants r on r.RestaurantID = o.RestaurantID join Campuses c on c.CampusID = r.CampusID where";
                bool firstClause = true;
                if (myParams.ID != null && myParams.ID.Trim().Length > 0)
                {
                    query += " o.OrderID=" + myParams.ID.Trim();
                    firstClause = false;
                }
                if (myParams.Date != null && myParams.Date.Trim().Length > 0)
                {
                    DateTime d = DateTime.Parse(myParams.Date.Trim());
                    string startDate = "'" + d.Year + "-" + d.Month + "-" + d.Day + "'";
                    d = d.AddDays(1);
                    string endDate = "'" + d.Year + "-" + d.Month + "-" + d.Day + "'";
                    query += (firstClause ? "" : " and") + " o.OrderPlacedDate between " + startDate + " and " + endDate;
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
                query += " order by OrderID desc";

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
            return View("Lookup");
        }

        public ActionResult LookupDetails(int orderID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "select case isnull(o.AffiliateID, 0) when 0 then '' else a.Name + ' (#' + convert(varchar, o.AffiliateID) + ')' end as Affiliate";
                query += ", case isnull(r.CampusID, 0) when 0 then '' else c.Name + ' (#' + convert(varchar, r.CampusID) + ')' end as Campus";
                query += ", case isnull(o.RestaurantID, 0) when 0 then '' else r.Name + ' (#' + convert(varchar, o.RestaurantID) + ')' end as Restaurant";
                query += ", o.CartTotal, o.CCAddressOnCard, o.CCCityOnCard";
                query += ", o.CCExpire, o.CCNameOnCard, o.CCStateOnCard, o.CCType, o.CCZipOnCard, o.CustomerID";
                query += ", c.Phone as CustomerPhone, o.DeliveryAddr1, o.DeliveryAddr2, o.DeliveryCity, o.DeliveryState";
                query += ", o.DeliveryZip, o.DeliveryFee, o.FirstOrder, o.FName, o.IsLUCCRestaurant, o.IsMobileOrder, o.LName";
                query += ", cu.LoginEmail, o.OrderID, o.OrderPlacedDate, o.OrderStatus, o.OrderTotal";
                query += ", case o.IsTakeout when 1 then 'Takeout' else 'Delivery' end as OrderType";
                query += ", o.PaymentMethod, p.PartnerName, o.ProcessingFee";
                query += ", r.Phone1 as RestaurantPhone, o.SpecialIntructions"; // sic
                query += ", o.Tax, o.Tip";
                query += " from AllOrders o join Customers cu on cu.CustomerID = o.CustomerID";
                query += " join Restaurants r on r.RestaurantID = o.RestaurantID join Campuses c on c.CampusID = r.CampusID";
                query += " join Partners p on p.PartnerID = o.PartnerID";
                query += " left outer join Affiliates a on a.AffiliateID = o.AffiliateID";
                query += " where o.OrderID = " + orderID;
                Reports.Models.OrderModel order = conn.Query<Reports.Models.OrderModel>(query).ToList().First();
                return View(order);
            }
        }

        public ActionResult Reports()
        {
            return View();
        }
    }
}
