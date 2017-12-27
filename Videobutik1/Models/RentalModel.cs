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

        public string RentalDate { get; set; }

        public string ActualReturnDate { get; set; }

        public string LastReturnDate { get; set; }

        public RentalModel()
        {
            RentalDate = DateTime.Today.ToString();
        }

    }

}