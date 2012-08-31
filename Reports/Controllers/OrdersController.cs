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
            Reports.Models.OrderParamsModel orderParams = new Reports.Models.OrderParamsModel();
            return View(orderParams);
        }

        public ActionResult LookupResults(Reports.Models.OrderParamsModel orderParams)
        {
            string query = "select top 100 o.OrderID, o.PaymentMethod, o.OrderPlacedDate, o.OrderTotal, o.OrderStatus, case o.IsTakeout when 1 then 'Takeout' else 'Delivery' end as OrderType, cu.FName, cu.LName, cu.LoginEmail, c.Name as CampusName, r.Name as RestaurantName from AllOrders o join Customers cu on cu.CustomerID = o.CustomerID join Restaurants r on r.RestaurantID = o.RestaurantID join Campuses c on c.CampusID = r.CampusID where";
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

        public ActionResult LookupDetails(int orderID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "select a.Name as AffiliateName, c.Name as CampusName, o.CartTotal, o.CCAddressOnCard, o.CCCityOnCard";
                query += ", o.CCExpire, o.CCNameOnCard, o.CCStateOnCard, o.CCType, o.CCZipOnCard, o.CustomerID";
                query += ", c.Phone as CustomerPhone, o.DeliveryAddr1, o.DeliveryAddr2, o.DeliveryCity, o.DeliveryState";
                query += ", o.DeliveryZip, o.DeliveryFee, o.FirstOrder, o.FName, o.IsLUCCRestaurant, o.IsMobileOrder, o.LName";
                query += ", cu.LoginEmail, o.OrderID, o.OrderPlacedDate, o.OrderStatus, o.OrderTotal";
                query += ", case o.IsTakeout when 1 then 'Takeout' else 'Delivery' end as OrderType";
                query += ", o.PaymentMethod, p.PartnerName, o.ProcessingFee, o.RestaurantID, r.Name as RestaurantName";
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
