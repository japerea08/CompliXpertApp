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
    }
}
