﻿using AutoLegalTracker_API.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoLegalTracker_API.DataAccess
{
    public class ALTContext : DbContext
    {
        public ALTContext(DbContextOptions<ALTContext> options) : base(options)
        {

        }
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
        // Indicando que la BD tiene una tabla llamada Emails
        public DbSet<Email> Emails { get; set; }
        public DbSet<EmailLog> EmailLogs { get; set; }
    }
}
