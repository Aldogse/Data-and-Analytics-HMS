using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reporting_and_Analytics.Migrations
{
    public partial class ADJUSTMODELS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HealthRecord",
                table: "PatientRecords",
                newName: "patient_id");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "PatientRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "type_of_service",
                table: "PatientRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "phic_members",
                table: "MonthlyPatientReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "phic_members",
                table: "DailyPatientReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "PatientRecords");

            migrationBuilder.DropColumn(
                name: "type_of_service",
                table: "PatientRecords");

            migrationBuilder.DropColumn(
                name: "phic_members",
                table: "MonthlyPatientReports");

            migrationBuilder.DropColumn(
                name: "phic_members",
                table: "DailyPatientReports");

            migrationBuilder.RenameColumn(
                name: "patient_id",
                table: "PatientRecords",
                newName: "HealthRecord");
        }
    }
}
