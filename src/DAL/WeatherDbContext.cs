using BL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext(DbContextOptions options) : base(options)  { }
        public DbSet<Weather> Weather { get; set; }
    }
}
