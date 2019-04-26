using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompliXpertApp.Models
{
    public partial class CompliXperAppContext: DbContext
    {
        public CompliXperAppContext()
        {
            Database.EnsureCreated();
        }
        
        public CompliXperAppContext(DbContextOptions<CompliXperAppContext> options): base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<CallReport> CallReport { get; set; }
        public virtual DbSet<FatcaQuestionnaire> FatcaQuestionnaire { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:sydeltest01.database.windows.net,1433;Initial Catalog=CompliXpertLite;Persist Security Info=False;User ID=sydeladmin;Password=#Baseball08;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
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
                entity.Property(e => e.CallReportId).ValueGeneratedNever();

                entity.HasOne(d => d.AccountNumberNavigation)
                    .WithMany(p => p.CallReport)
                    .HasForeignKey(d => d.AccountNumber)
                    .HasConstraintName("FK__CallRepor__Accou__59063A47");
            });

            modelBuilder.Entity<FatcaQuestionnaire>(entity =>
            {
                entity.HasKey(e => e.QuestionnaireId);

                entity.Property(e => e.QuestionnaireId).ValueGeneratedNever();

                entity.HasOne(d => d.AccountNumberNavigation)
                    .WithMany(p => p.FatcaQuestionnaire)
                    .HasForeignKey(d => d.AccountNumber)
                    .HasConstraintName("FK__FatcaQues__Accou__59FA5E80");
            });
        }
    }
}
