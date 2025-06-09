using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reporting_and_Analytics.Migrations
{
    public partial class ADDPATIENTRECORDTABLES : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyPatientReports",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    number_of_patients = table.Column<int>(type: "int", nullable: false),
                    total_inpatient = table.Column<int>(type: "int", nullable: false),
                    total_outpatient = table.Column<int>(type: "int", nullable: false),
                    report_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyPatientReports", x => x.report_id);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyPatientReports",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    number_of_patients = table.Column<int>(type: "int", nullable: false),
                    total_inpatient = table.Column<int>(type: "int", nullable: false),
                    total_outpatient = table.Column<int>(type: "int", nullable: false),
                    report_generated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    month = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyPatientReports", x => x.report_id);
                });

            migrationBuilder.CreateTable(
                name: "PatientRecords",
                columns: table => new
                {
                    HealthRecord = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    patient_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    admission_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PHIC = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientRecords", x => x.HealthRecord);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyPatientReports");

            migrationBuilder.DropTable(
                name: "MonthlyPatientReports");

            migrationBuilder.DropTable(
                name: "PatientRecords");
        }
    }
}
