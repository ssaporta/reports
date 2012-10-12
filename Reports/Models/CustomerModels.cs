using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Http.Validation;
using System.Web.Http.Metadata;

namespace Reports.Models
{
    public class CustomerModel
    {
        public ParamsModel Params { get; set; }
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

    public class CustomerListModel
    {
        public int CampusID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<CustomerModel> Output { get; set; }
    }
}