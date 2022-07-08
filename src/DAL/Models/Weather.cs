using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class Weather
    {
        public int Id { get; set; }
        public string City { get; set; }
        public double? Temperature { get; set; }
        public double Longitude { get; set; } 
        public double Latitude { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }    
}
