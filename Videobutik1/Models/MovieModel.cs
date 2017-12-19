using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Videobutik1.Models
{
    public class MovieModel
    {
        [Key]
        public int MovieId { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int LengthMinutes { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }
    }
}