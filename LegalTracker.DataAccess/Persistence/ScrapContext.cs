using LegalTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegalTracker.DataAccess.Persistence
{
    public class ScrapContext : DbContext
    {
        public ScrapContext(DbContextOptions<ScrapContext> options) : base(options)
        {

        }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    }
}
