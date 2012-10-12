using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reports.Models
{
    public class CustomerReportModel
    {
        public int CampusID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Results> results { get; set; }
    }

    public class Results
    {
        public DateTime CreateDate { get; set; }
        public string Fname { get; set; }
        public bool IsActive { get; set; }
        public string Lname { get; set; }
        public string LoginEmail { get; set; }
        public bool OptIn { get; set; }
        public string Phone { get; set; }
        public bool SendCoupons { get; set; }
        public string RestaurantName { get; set; }
    }
}