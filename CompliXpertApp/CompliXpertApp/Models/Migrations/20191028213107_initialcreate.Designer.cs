﻿// <auto-generated />
using System;
using CompliXpertApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MigrationsApp.Migrations
{
    [DbContext(typeof(CompliXperAppContext))]
    [Migration("20191028213107_initialcreate")]
    partial class initialcreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("CompliXperLite.Models.Account", b =>
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

            modelBuilder.Entity("CompliXperLite.Models.AccountClass", b =>
            {
                b.Property<int>("AccountClassCode");

                b.Property<string>("Description");

                b.HasKey("AccountClassCode");

                b.ToTable("AccountClasses");
            });

            modelBuilder.Entity("CompliXperLite.Models.CallReport", b =>
            {
                b.Property<int>("CallReportId");

                b.Property<int?>("AccountNumber");

                b.Property<string>("ApprovedBy");

                b.Property<DateTime>("ApprovedDate");

                b.Property<DateTime>("CallDate");
                b.Property<DateTime>("CreatedDate");

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

            modelBuilder.Entity("CompliXperLite.Models.CallReportQuestions", b =>
            {
                b.Property<int>("QuestionId");

                b.Property<string>("QuestionHeader");

                b.Property<bool>("Status");

                b.Property<string>("Type");

                b.HasKey("QuestionId");

                b.ToTable("CallReportQuestions");
            });

            modelBuilder.Entity("CompliXperLite.Models.CallReportResponse", b =>
            {
                b.Property<int>("ResponseId");

                b.Property<int>("CallReportId");

                b.Property<int>("QuestionId");

                b.Property<string>("Response");

                b.HasKey("ResponseId");

                b.HasIndex("CallReportId");

                b.ToTable("CallReportResponse");
            });

            modelBuilder.Entity("CompliXperLite.Models.CallReportType", b =>
            {
                b.Property<string>("Type")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Description");

                b.HasKey("Type");

                b.ToTable("CallReportType");
            });

            modelBuilder.Entity("CompliXperLite.Models.Country", b =>
            {
                b.Property<int>("CountryCode");

                b.Property<string>("Description");

                b.HasKey("CountryCode");

                b.ToTable("Countries");
            });

            modelBuilder.Entity("CompliXperLite.Models.Customer", b =>
            {
                b.Property<int>("CustomerNumber");

                b.Property<string>("BusinessCode");

                b.Property<int?>("Citizenship");

                b.Property<int?>("CountryofResidence");

                b.Property<bool>("CreatedOnMobile");

                b.Property<DateTime>("CreatedDate");

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

            modelBuilder.Entity("CompliXperLite.Models.IndustryType", b =>
            {
                b.Property<string>("Code")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Description");

                b.HasKey("Code");

                b.ToTable("IndustryTypes");
            });

            modelBuilder.Entity("CompliXperLite.Models.LinesofBusiness", b =>
            {
                b.Property<string>("Code")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Description");

                b.HasKey("Code");

                b.ToTable("LinesofBusinesses");
            });

            modelBuilder.Entity("CompliXperLite.Models.NewContact", b =>
            {
                b.Property<int>("ContactId")
                .ValueGeneratedOnAdd();

                b.Property<string>("Comments");
                b.Property<DateTime>("CreatedDate");

                b.Property<string>("Company");

                b.Property<string>("Email");

                b.Property<string>("FirstName");

                b.Property<string>("LastName");

                b.Property<string>("Phonenumber");

                b.Property<string>("Title");

                b.HasKey("ContactId");

                b.ToTable("NewContact");
            });

            modelBuilder.Entity("CompliXperLite.Models.Note", b =>
            {
                b.Property<int>("NoteId")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("CallReportId");

                b.Property<bool>("CreatedonMobile");
                b.Property<DateTime>("CreatedDate");

                b.Property<string>("Description");

                b.Property<string>("Subject");

                b.HasKey("NoteId");

                b.HasIndex("CallReportId");

                b.ToTable("Note");
            });

            modelBuilder.Entity("CompliXperLite.Models.Person", b =>
            {
                b.Property<int>("PersonId")
                    .ValueGeneratedOnAdd();

                b.Property<int?>("CallReportId");

                b.Property<bool>("CreatedonMobile");
                b.Property<DateTime>("CreatedDate");

                b.Property<string>("FirstName");

                b.Property<string>("LastName");
                b.Property<string>("Email");

                b.Property<string>("Position");
                b.Property<string>("PhoneNumber");

                b.HasKey("PersonId");

                b.HasIndex("CallReportId");

                b.ToTable("Person");
            });

            modelBuilder.Entity("CompliXperLite.Models.ProductCode", b =>
            {
                b.Property<string>("Code")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Description");

                b.HasKey("Code");

                b.ToTable("ProductCodes");
            });

            modelBuilder.Entity("CompliXperLite.Models.User", b =>
            {
                b.Property<int>("UserID");

                b.Property<string>("Password");

                b.Property<string>("UserName");

                b.HasKey("UserID");

                b.ToTable("Users");
            });

            modelBuilder.Entity("CompliXperLite.Models.Account", b =>
            {
                b.HasOne("CompliXperLite.Models.AccountClass", "AccountClass")
                    .WithMany()
                    .HasForeignKey("AccountClassCode");

                b.HasOne("CompliXperLite.Models.Customer", "CustomerNumberNavigation")
                    .WithMany("Account")
                    .HasForeignKey("CustomerNumber");
            });

            modelBuilder.Entity("CompliXperLite.Models.CallReport", b =>
            {
                b.HasOne("CompliXperLite.Models.Account", "AccountNumberNavigation")
                    .WithMany("CallReport")
                    .HasForeignKey("AccountNumber");
            });

            modelBuilder.Entity("CompliXperLite.Models.CallReportResponse", b =>
            {
                b.HasOne("CompliXperLite.Models.CallReport")
                    .WithMany("Responses")
                    .HasForeignKey("CallReportId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("CompliXperLite.Models.Note", b =>
            {
                b.HasOne("CompliXperLite.Models.CallReport")
                    .WithMany("Notes")
                    .HasForeignKey("CallReportId");
            });
            modelBuilder.Entity("CompliXperLite.Models.Person", b =>
            {
                b.HasOne("CompliXperLite.Models.CallReport")
                    .WithMany("Persons")
                    .HasForeignKey("CallReportId");
            });
#pragma warning restore 612, 618
        }
    }
}
