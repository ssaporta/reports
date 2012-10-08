using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Reports.Models
{
    public class OrderModel
    {
        public string Affiliate { get; set; }
        public string Campus { get; set; }
        public double CartTotal { get; set; }
        public string CCAddressOnCard { get; set; }
        public string CCCityOnCard { get; set; }
        public string CCExpire { get; set; }
        public string CCNameOnCard { get; set; }
        public string CCStateOnCard { get; set; }
        public string CCType { get; set; }
        public string CCZipOnCard { get; set; }
        public int CustomerID { get; set; }
        public string CustomerPhone { get; set; }
        public string DeliveryAddr1 { get; set; }
        public string DeliveryAddr2 { get; set; }
        public string DeliveryCity { get; set; }
        public string DeliveryState { get; set; }
        public string DeliveryZip { get; set; }
        public double DeliveryFee { get; set; }
        public bool FirstOrder { get; set; }
        public string FName { get; set; }
        public bool IsLUCCRestaurant { get; set; }
        public bool IsMobileOrder { get; set; }
        public string LName { get; set; }
        public string LoginEmail { get; set; }
        public int OrderID { get; set; }
        public DateTime OrderPlacedDate { get; set; }
        public string OrderStatus { get; set; }
        public double OrderTotal { get; set; }
        public string OrderType { get; set; }
        public string PaymentMethod { get; set; }
        public string PartnerName { get; set; }
        public double ProcessingFee { get; set; }
        public string Restaurant { get; set; }
        public string RestaurantPhone { get; set; }
        public string SpecialInstructions { get; set; }
        public double Tax { get; set; }
        public double Tip { get; set; }
    }
}