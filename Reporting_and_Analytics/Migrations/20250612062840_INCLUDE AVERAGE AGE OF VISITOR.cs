using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reporting_and_Analytics.Migrations
{
    public partial class INCLUDEAVERAGEAGEOFVISITOR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Average_age",
                table: "MonthlyPatientReports",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Average_age",
                table: "MonthlyPatientReports");
        }
    }
}
