using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CompliXpertApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerNumber = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    CustomerName = table.Column<string>(nullable: true),
                    LegalType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerNumber);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountNumber = table.Column<int>(nullable: false),
                    AccountType = table.Column<string>(nullable: true),
                    AccountClass = table.Column<string>(nullable: true),
                    CustomerNumber = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountNumber);
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
                    Nationality = table.Column<string>(nullable: true),
                    ReasonforAlert = table.Column<string>(nullable: true),
                    CustomerResponse = table.Column<string>(nullable: true),
                    Officer = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    CallDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    ApprovedBy = table.Column<string>(nullable: true),
                    ApprovedDate = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    Purpose = table.Column<string>(nullable: true),
                    OfficerComments = table.Column<string>(nullable: true),
                    OtherComments = table.Column<string>(nullable: true),
                    CustomerComments = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<int>(nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_Account_CustomerNumber",
                table: "Account",
                column: "CustomerNumber");

            migrationBuilder.CreateIndex(
                name: "IX_CallReport_AccountNumber",
                table: "CallReport",
                column: "AccountNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallReport");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
