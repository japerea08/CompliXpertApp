using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace CompliXpertApp.Models
{
    public partial class CompliXperAppContext: DbContext
    {
        private string databaseName = "CompliXpertDB.db";
        public CompliXperAppContext()
        {
            //run this to when updating the database; working with migrations tedious
            //Database.EnsureDeleted();
            Database.Migrate();
        }
        
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<CallReport> CallReport { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),databaseName);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Filename={databasePath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountNumber);

                entity.Property(e => e.AccountNumber).ValueGeneratedNever();

                entity.HasOne(d => d.CustomerNumberNavigation)
                .WithMany(p => p.Account)
                .HasForeignKey(d => d.CustomerNumber);
            });

            modelBuilder.Entity<CallReport>(entity =>
            {
                entity.Property(e => e.CallReportId).ValueGeneratedOnAdd();

                entity.HasOne(d => d.AccountNumberNavigation)
                    .WithMany(p => p.CallReport)
                    .HasForeignKey(d => d.AccountNumber)
                    .HasConstraintName("FK__CallRepor__Accou__59063A47");
            });

            modelBuilder.Entity<Customer>(entity => 
            {
                entity.HasKey(e => e.CustomerNumber);

                entity.Property(e => e.CustomerNumber).ValueGeneratedNever();

            });

        }
    }
}
