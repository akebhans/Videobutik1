using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Videobutik1.Models
{
    public class RentalModel
    {
        public int RentalId { get; set; }
        public int MovieId { get; set; }

        public string MovieTitle { get; set; }
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }
        public DateTime RentalDate { get; set; }

        public DateTime ActualReturnDate { get; set; }

        public DateTime LastReturnDate { get; set; }

    }
}