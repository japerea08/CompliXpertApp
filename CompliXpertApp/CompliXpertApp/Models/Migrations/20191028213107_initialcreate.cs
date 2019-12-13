using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MigrationsApp.Migrations
{
    public partial class initialcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountClasses",
                columns: table => new
                {
                    AccountClassCode = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountClasses", x => x.AccountClassCode);
                });

            migrationBuilder.CreateTable(
                name: "CallReportQuestions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuestionHeader = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallReportQuestions", x => x.QuestionId);
                });

            migrationBuilder.CreateTable(
                name: "CallReportType",
                columns: table => new
                {
                    Type = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallReportType", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryCode = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryCode);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerNumber = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    CustomerName = table.Column<string>(nullable: true),
                    LegalType = table.Column<string>(nullable: true),
                    CreatedOnMobile = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    IsPEP = table.Column<bool>(nullable: false),
                    MailAddress = table.Column<string>(nullable: true),
                    Citizenship = table.Column<int>(nullable: true),
                    CountryofResidence = table.Column<int>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    BusinessCode = table.Column<string>(nullable: true),
                    IndustryCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerNumber);
                });

            migrationBuilder.CreateTable(
                name: "IndustryTypes",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndustryTypes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "LinesofBusinesses",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinesofBusinesses", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "NewContact",
                columns: table => new
                {
                    ContactId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phonenumber = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewContact", x => x.ContactId);
                });

            migrationBuilder.CreateTable(
                name: "ProductCodes",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCodes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountNumber = table.Column<int>(nullable: false),
                    AccountType = table.Column<string>(nullable: true),
                    AccountClassCode = table.Column<int>(nullable: true),
                    CustomerNumber = table.Column<int>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    BusinessCode = table.Column<string>(nullable: true),
                    IndustryCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountNumber);
                    table.ForeignKey(
                        name: "FK_Account_AccountClasses_AccountClassCode",
                        column: x => x.AccountClassCode,
                        principalTable: "AccountClasses",
                        principalColumn: "AccountClassCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Account_Customer_CustomerNumber",
                        column: x => x.CustomerNumber,
                        principalTable: "Customer",
                        principalColumn: "CustomerNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CallReport",
                columns: table => new
                {
                    CallReportId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOnMobile = table.Column<bool>(nullable: false),
                    Officer = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    CallDate = table.Column<DateTime>(nullable: false),
                    Reference = table.Column<string>(nullable: true),
                    ApprovedBy = table.Column<string>(nullable: true),
                    ApprovedDate = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    AccountNumber = table.Column<int>(nullable: true),
                    CallReportType = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallReport", x => x.CallReportId);
                    table.ForeignKey(
                        name: "FK__CallRepor__Accou__59063A47",
                        column: x => x.AccountNumber,
                        principalTable: "Account",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CallReportResponse",
                columns: table => new
                {
                    ResponseId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Response = table.Column<string>(nullable: true),
                    QuestionId = table.Column<int>(nullable: false),
                    CallReportId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallReportResponse", x => x.ResponseId);
                    table.ForeignKey(
                        name: "FK_CallReportResponse_CallReport_CallReportId",
                        column: x => x.CallReportId,
                        principalTable: "CallReport",
                        principalColumn: "CallReportId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    NoteId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Subject = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatedonMobile = table.Column<bool>(nullable: false),
                    CallReportId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.NoteId);
                    table.ForeignKey(
                        name: "FK_Note_CallReport_CallReportId",
                        column: x => x.CallReportId,
                        principalTable: "CallReport",
                        principalColumn: "CallReportId",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_NewContact_AccountNumber",
            //    table: "NewContact",
            //    column: "AccountNumber",
            //    unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_AccountClassCode",
                table: "Account",
                column: "AccountClassCode");

            migrationBuilder.CreateIndex(
                name: "IX_Account_ContactId",
                table: "Account",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_CustomerNumber",
                table: "Account",
                column: "CustomerNumber");

            migrationBuilder.CreateIndex(
                name: "IX_CallReport_AccountNumber",
                table: "CallReport",
                column: "AccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_CallReportResponse_CallReportId",
                table: "CallReportResponse",
                column: "CallReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Note_CallReportId",
                table: "Note",
                column: "CallReportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallReportQuestions");

            migrationBuilder.DropTable(
                name: "CallReportResponse");

            migrationBuilder.DropTable(
                name: "CallReportType");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "IndustryTypes");

            migrationBuilder.DropTable(
                name: "LinesofBusinesses");

            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "ProductCodes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CallReport");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "AccountClasses");

            migrationBuilder.DropTable(
                name: "NewContact");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
