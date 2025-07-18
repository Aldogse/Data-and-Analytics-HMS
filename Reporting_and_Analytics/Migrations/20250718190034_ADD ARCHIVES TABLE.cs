using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reporting_and_Analytics.Migrations
{
    public partial class ADDARCHIVESTABLE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyIncomeReportArchives",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    total_income = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    day = table.Column<int>(type: "int", nullable: false),
                    month = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyIncomeReportArchives", x => x.report_id);
                });

            migrationBuilder.CreateTable(
                name: "DailyPatientReportArchives",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    number_of_patients = table.Column<int>(type: "int", nullable: false),
                    total_inpatient = table.Column<int>(type: "int", nullable: false),
                    total_outpatient = table.Column<int>(type: "int", nullable: false),
                    phic_members = table.Column<int>(type: "int", nullable: false),
                    report_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyPatientReportArchives", x => x.report_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyIncomeReportArchives");

            migrationBuilder.DropTable(
                name: "DailyPatientReportArchives");
        }
    }
}
