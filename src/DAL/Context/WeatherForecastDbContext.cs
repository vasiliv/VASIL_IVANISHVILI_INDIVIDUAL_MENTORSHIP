using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace DAL.Context
{
    public partial class WeatherForecastDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public WeatherForecastDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public WeatherForecastDbContext(DbContextOptions<WeatherForecastDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            }            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
