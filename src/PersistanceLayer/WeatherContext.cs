using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Models;

namespace PersistanceLayer
{
    public class WeatherContext : DbContext
    {
        public WeatherContext(DbContextOptions options):base(options) { }
        public DbSet <Weather> Weather { get; set; }
    }
}
