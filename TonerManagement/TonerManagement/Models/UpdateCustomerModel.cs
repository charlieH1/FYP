using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TonerManagement.Models
{
    public class UpdateCustomerModel
    {
        public int CustomerId { get; set; }
        public string CustomerPostalAddress { get; set; }
        public string CustomerPhoneNumber { get; set; }
    }
}