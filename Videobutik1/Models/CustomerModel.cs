using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Videobutik1.Models
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
    }
}