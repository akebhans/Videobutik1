﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Videobutik1.Models;

namespace Videobutik1.Context
{
    public class DB_Context : DbContext
    {
            public DB_Context() : base("MyContextDB") { }
            public DbSet<CustomerModel> Customers { get; set; }
            public DbSet<MovieModel> Movies { get; set; }
            public DbSet<RentalModel> Rentals { get; set; }

    }
}