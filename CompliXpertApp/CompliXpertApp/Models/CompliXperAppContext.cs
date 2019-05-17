using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CompliXpertApp.Models
{
    public partial class CompliXperAppContext: DbContext
    {
        private string databaseName = "CompliXpertDB.db";
        public CompliXperAppContext()
        {
            Database.EnsureCreated();
        }
        
        public CompliXperAppContext(DbContextOptions<CompliXperAppContext> options): base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<CallReport> CallReport { get; set; }

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
            });

            modelBuilder.Entity<CallReport>(entity =>
            {
                entity.Property(e => e.CallReportId).ValueGeneratedOnAdd();

                entity.HasOne(d => d.AccountNumberNavigation)
                    .WithMany(p => p.CallReport)
                    .HasForeignKey(d => d.AccountNumber)
                    .HasConstraintName("FK__CallRepor__Accou__59063A47");
            });

        }
    }
}
