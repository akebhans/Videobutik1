﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Videobutik1.Models;

namespace Videobutik1.Context
{
    public class CustomerContext:DbContext
    {
        public DbSet<CustomerModel> Customers { get; set; }
    }
}