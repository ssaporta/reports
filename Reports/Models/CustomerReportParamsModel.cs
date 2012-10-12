using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reports.Models
{
    public class CustomerReportParamsModel
    {
        public int CampusID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}