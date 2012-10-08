using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Http.Validation;
using System.Web.Http.Metadata;

namespace Reports.Models
{
    [NotAllBlank(ErrorMessage="Please specify at least one value")]
    public class ParamsModel
    {
        [Range(1, 100000000)]
        public string ID { get; set; }

        [DateAttribute]
        public string Date { get; set; }

        public string LastName { get; set; }
        public string LoginEmail { get; set; }
    }

    public class DateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || value.ToString().Trim().Length == 0)
                return true;
            try { DateTime dt = DateTime.Parse(value.ToString()); } catch { return false; }
            return true;
        }
    }

    public class NotAllBlank : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            ParamsModel paramsModel = (ParamsModel)value;
            if (paramsModel.ID != null && paramsModel.ID.ToString().Trim().Length > 0)
                return true;
            if (paramsModel.Date != null && paramsModel.Date.ToString().Trim().Length > 0)
                return true;
            if (paramsModel.LastName != null && paramsModel.LastName.ToString().Trim().Length > 0)
                return true;
            if (paramsModel.LoginEmail != null && paramsModel.LoginEmail.ToString().Trim().Length > 0)
                return true;
            return false;
        }
    }
}