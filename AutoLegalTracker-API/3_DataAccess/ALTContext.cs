using AutoLegalTracker_API.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoLegalTracker_API.DataAccess
{
    public class ALTContext : DbContext
    {
        public ALTContext(DbContextOptions<ALTContext> options) : base(options)
        {

        }
        public DbSet<EmailTemplate> Emails { get; set; }
        public DbSet<EmailLog> EmailLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<LegalCase> LegalCases { get; set; }
        public DbSet<LegalNotification> LegalNotifications { get; set; }
        public DbSet<LegalAutomation> LegalAutomations { get; set; }
        public DbSet<MedicalAppointment> MedicalAppointments { get; set; }
        public DbSet<LegalCaseAction> LegalCaseActions { get; set; }
        public DbSet<LegalCaseAttribute> LegalCaseAttributes { get; set; }
        public DbSet<LegalCaseCondition> LegalCaseConditions{ get; set; }
        public DbSet<NotificationCondition> NotificationConditions { get; set; }
        public DbSet<RequestedAnalysis> RequestedAnalyses { get; set; }
        public DbSet<RequestedCourtOrder> RequestedCourtOrders { get; set; }
        public DbSet<NotificationReference> NotificationReferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LegalCaseAction>()
                    .HasOne(ac => ac.User)
                    .WithMany()
                    .HasForeignKey(ac => ac.UserId)
                    .OnDelete(DeleteBehavior.Restrict); // Specify no cascading delete


            modelBuilder.Entity<LegalCaseAction>()
                .HasMany(ac => ac.LegalCaseAttributesToAdd)
                .WithMany(at => at.LegalCaseActionsWhereItsAdded)
                .UsingEntity("LegalCaseAttributesToAdd");
            
            modelBuilder.Entity<LegalCaseAction>()
                .HasMany(ac => ac.LegalCaseAttributesToDelete)
                .WithMany(at => at.LegalCaseActionsWhereItsDeleted)
                .UsingEntity("LegalCaseAttributesToDelete");

            base.OnModelCreating(modelBuilder);

        }

    }
}
