using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Reports.Models
{
    public class CustomerParamsModel
    {
        [Required]
        public string CustomerID { get; set; }

        public string Date { get; set; }
        public string LastName { get; set; }
        public string LoginEmail { get; set; }
    }

    public class CustomerModel
    {
        public string Affiliate { get; set; }
        public string Campus { get; set; }
        public DateTime CreateDate { get; set; }
        public int CustomerID { get; set; }
        public string FName { get; set; }
        public bool IsActive { get; set; }
        public string LName { get; set; }
        public string LoginEmail { get; set; }
        public bool OptIn { get; set; }
        public string Phone { get; set; }
        public string Restaurant { get; set; }
        public bool SendCoupons { get; set; }
    }
}