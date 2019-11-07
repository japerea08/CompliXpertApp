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
        public virtual DbSet<CallReportType> CallReportType { get; set; }
        public virtual DbSet<CallReportQuestions> CallReportQuestions { get; set; }
        public virtual DbSet<CallReportResponse> CallReportResponse { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<AccountClass> AccountClasses { get; set; }
        public DbSet<IndustryType> IndustryTypes { get; set; }
        public DbSet<LinesofBusiness> LinesofBusinesses { get; set; }
        public DbSet<ProductCode> ProductCodes { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<NewContact> NewContacts { get; set; }
        //public DbSet<QuestionandResponse> QuestionandResponses { get; set; }
        //public DbSet<User> Users { get; set; }

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
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("CompliXpertApp.Models.Account", b =>
            {
                b.Property<int>("AccountNumber");

                b.Property<int?>("AccountClassCode");

                b.Property<string>("AccountType");

                b.Property<string>("BusinessCode");

                b.Property<int?>("CustomerNumber");

                b.Property<string>("IndustryCode");

                b.Property<string>("ProductCode");

                b.HasKey("AccountNumber");

                b.HasIndex("AccountClassCode");

                b.HasIndex("CustomerNumber");

                b.ToTable("Account");
            });

            modelBuilder.Entity("CompliXpertApp.Models.AccountClass", b =>
            {
                b.Property<int>("AccountClassCode")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Description");

                b.HasKey("AccountClassCode");

                b.ToTable("AccountClasses");
            });

            modelBuilder.Entity("CompliXpertApp.Models.CallReport", b =>
            {
                b.Property<int>("CallReportId")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("AccountNumber");

                b.Property<string>("ApprovedBy");

                b.Property<DateTime>("ApprovedDate");

                b.Property<DateTime>("CallDate");

                b.Property<string>("CallReportType");

                b.Property<bool>("CreatedOnMobile");

                b.Property<string>("LastUpdated");

                b.Property<DateTime>("LastUpdatedDate");

                b.Property<string>("Officer");

                b.Property<string>("Position");

                b.Property<string>("Reason");

                b.Property<string>("Reference");

                b.HasKey("CallReportId");

                b.HasIndex("AccountNumber");

                b.ToTable("CallReport");
            });

            modelBuilder.Entity("CompliXpertApp.Models.CallReportQuestions", b =>
            {
                b.Property<int>("QuestionId")
                    .ValueGeneratedOnAdd();

                b.Property<string>("QuestionHeader");

                b.Property<bool>("Status");

                b.Property<string>("Type");

                b.HasKey("QuestionId");

                b.ToTable("CallReportQuestions");
            });

            modelBuilder.Entity("CompliXpertApp.Models.CallReportResponse", b =>
            {
                b.Property<int>("ResponseId")
                    .ValueGeneratedOnAdd();

                b.Property<int>("CallReportId");

                b.Property<int>("QuestionId");

                b.Property<string>("Response");

                b.HasKey("ResponseId");

                b.HasIndex("CallReportId");

                b.ToTable("CallReportResponse");
            });

            modelBuilder.Entity("CompliXpertApp.Models.CallReportType", b =>
            {
                b.Property<string>("Type")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Description");

                b.HasKey("Type");

                b.ToTable("CallReportType");
            });

            modelBuilder.Entity("CompliXpertApp.Models.Country", b =>
            {
                b.Property<int>("CountryCode")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Description");

                b.HasKey("CountryCode");

                b.ToTable("Countries");
            });

            modelBuilder.Entity("CompliXpertApp.Models.Customer", b =>
            {
                b.Property<int>("CustomerNumber");

                b.Property<string>("BusinessCode");

                b.Property<int?>("Citizenship");

                b.Property<int?>("CountryofResidence");

                b.Property<bool>("CreatedOnMobile");

                b.Property<int>("CustomerId");

                b.Property<string>("CustomerName");

                b.Property<string>("Email");

                b.Property<string>("IndustryCode");

                b.Property<bool>("IsPEP");

                b.Property<string>("LegalType");

                b.Property<string>("MailAddress");

                b.HasKey("CustomerNumber");

                b.ToTable("Customer");
            });

            modelBuilder.Entity("CompliXpertApp.Models.IndustryType", b =>
            {
                b.Property<string>("Code")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Description");

                b.HasKey("Code");

                b.ToTable("IndustryTypes");
            });

            modelBuilder.Entity("CompliXpertApp.Models.LinesofBusiness", b =>
            {
                b.Property<string>("Code")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Description");

                b.HasKey("Code");

                b.ToTable("LinesofBusinesses");
            });

            modelBuilder.Entity("CompliXpertApp.Models.NewContact", b =>
            {
                b.Property<int>("ContactId")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Comments");

                b.Property<string>("Company");

                b.Property<string>("Email");

                b.Property<string>("Name");

                b.Property<string>("Phonenumber");

                b.Property<string>("Title");

                b.HasKey("ContactId");

                b.ToTable("NewContact");
            });

            modelBuilder.Entity("CompliXpertApp.Models.Note", b =>
            {
                b.Property<int>("NoteId")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("CallReportId");

                b.Property<bool>("CreatedonMobile");

                b.Property<string>("Description");

                b.Property<string>("Subject");

                b.HasKey("NoteId");

                b.HasIndex("CallReportId");

                b.ToTable("Note");
            });

            modelBuilder.Entity("CompliXpertApp.Models.ProductCode", b =>
            {
                b.Property<string>("Code")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Description");

                b.HasKey("Code");

                b.ToTable("ProductCodes");
            });

            modelBuilder.Entity("CompliXpertApp.Models.Account", b =>
            {
                b.HasOne("CompliXpertApp.Models.AccountClass", "AccountClass")
                    .WithMany()
                    .HasForeignKey("AccountClassCode");

                b.HasOne("CompliXpertApp.Models.Customer", "CustomerNumberNavigation")
                    .WithMany("Account")
                    .HasForeignKey("CustomerNumber");
            });

            modelBuilder.Entity("CompliXpertApp.Models.CallReport", b =>
            {
                b.HasOne("CompliXpertApp.Models.Account", "AccountNumberNavigation")
                    .WithMany("CallReport")
                    .HasForeignKey("AccountNumber")
                    .HasConstraintName("FK__CallRepor__Accou__59063A47");
            });

            modelBuilder.Entity("CompliXpertApp.Models.CallReportResponse", b =>
            {
                b.HasOne("CompliXpertApp.Models.CallReport")
                    .WithMany("Responses")
                    .HasForeignKey("CallReportId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("CompliXpertApp.Models.Note", b =>
            {
                b.HasOne("CompliXpertApp.Models.CallReport")
                    .WithMany("Notes")
                    .HasForeignKey("CallReportId");
            });

        }
    }
}
