using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Videobutik1.Models
{
    public class RentalModel
    {
        [Key]
        public int RentalId { get; set; }
        public int MovieId { get; set; }

        public int CustomerId { get; set; }

        public DateTime RentalDate { get; set; }

        public DateTime ActualReturnDate { get; set; }

        public DateTime LastReturnDate { get; set; }

    }

    public class RentalListModel : RentalModel
    {
        public string Movie { get; set; }
        public string Customer { get; set; }
    }
}