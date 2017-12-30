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

    // This derived class is only used to easily implement sorting of customer and movie in the rental list, 
    // since the names are not stored in the same table as the rentals in the database - in order to have normalized tables.
    public class RentalModelNames : RentalModel
    {
        public string MovieName { get; set; }
        public string CustomerName { get; set; }
    }

}