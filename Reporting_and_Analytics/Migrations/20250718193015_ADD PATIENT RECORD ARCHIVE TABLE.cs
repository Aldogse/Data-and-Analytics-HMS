using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reporting_and_Analytics.Migrations
{
    public partial class ADDPATIENTRECORDARCHIVETABLE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientRecordArchives",
                columns: table => new
                {
                    report_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    patient_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Full_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    admission_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PHIC = table.Column<bool>(type: "bit", nullable: false),
                    type_of_service = table.Column<int>(type: "int", nullable: false),
                    tracked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientRecordArchives", x => x.report_Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientRecordArchives");
        }
    }
}
