using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reporting_and_Analytics.Migrations
{
    public partial class ADDEMPLOYEETABLES : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    employee_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    adherance_rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    shift_start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    shift_end = table.Column<DateTime>(type: "datetime2", nullable: false),
                    start_off_day = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_off_day = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.employee_id);
                });

            migrationBuilder.CreateTable(
                name: "AdheranceReports",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employee_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    clock_in = table.Column<DateTime>(type: "datetime2", nullable: false),
                    clock_out = table.Column<DateTime>(type: "datetime2", nullable: false),
                    report_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdheranceReports", x => x.report_id);
                    table.ForeignKey(
                        name: "FK_AdheranceReports_Employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "Employees",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdheranceReports_employee_id",
                table: "AdheranceReports",
                column: "employee_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdheranceReports");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
