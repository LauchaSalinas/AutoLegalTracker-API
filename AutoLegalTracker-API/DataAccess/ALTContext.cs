using AutoLegalTracker_API.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoLegalTracker_API.DataAccess
{
    public class ALTContext : DbContext
    {
        public ALTContext(DbContextOptions<ALTContext> options) : base(options)
        {

        }
        public DbSet<WeatherForecast> WeatherForecasts { get; set;}
        public DbSet<Jwt> Jwts { get; set; }
    }
}
