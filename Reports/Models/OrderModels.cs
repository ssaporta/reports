using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Reports.Models
{
    public class OrderParamsModel
    {
        public string OrderID { get; set; }
        public string Date { get; set; }
        public string LoginEmail { get; set; }
        public string LastName { get; set; }
    }

    public class OrderModel
    {
        public int OrderID { get; set; }
        public double OrderTotal { get; set; }
        public string PaymentMethod { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
    }
}