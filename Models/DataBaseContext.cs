﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebWordGame.Models
{
    public class DataBaseContext : DbContext
    {
        DbSet<PersonModel> people { get; set; }
        public DataBaseContext(DbContextOptions<DataBaseContext> dbContextOptions) 
            : base(dbContextOptions)
        {
            Database.EnsureCreated();
        }
    }
}