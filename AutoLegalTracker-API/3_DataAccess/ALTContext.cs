using AutoLegalTracker_API.Models;
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
        public DbSet<EmailTemplate> Emails { get; set; }
        public DbSet<EmailLog> EmailLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LegalCase> LegalCases { get; set; }
        public DbSet<LegalNotification> LegalNotifications { get; set; }
        public DbSet<LegalAutomation> LegalAutomations { get; set; }
        public DbSet<MedicalAppointment> MedicalAppointments { get; set; }

    }
}
