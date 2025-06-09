using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reporting_and_Analytics.Migrations
{
    public partial class make_adjustments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncomeStatements_Particulars_particulartransaction_id",
                table: "IncomeStatements");

            migrationBuilder.DropIndex(
                name: "IX_IncomeStatements_particulartransaction_id",
                table: "IncomeStatements");

            migrationBuilder.DropColumn(
                name: "particulartransaction_id",
                table: "IncomeStatements");

            migrationBuilder.DropColumn(
                name: "service",
                table: "IncomeStatements");

            migrationBuilder.AddColumn<int>(
                name: "service_id",
                table: "IncomeStatements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_IncomeStatements_service_id",
                table: "IncomeStatements",
                column: "service_id");

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeStatements_Particulars_service_id",
                table: "IncomeStatements",
                column: "service_id",
                principalTable: "Particulars",
                principalColumn: "transaction_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncomeStatements_Particulars_service_id",
                table: "IncomeStatements");

            migrationBuilder.DropIndex(
                name: "IX_IncomeStatements_service_id",
                table: "IncomeStatements");

            migrationBuilder.DropColumn(
                name: "service_id",
                table: "IncomeStatements");

            migrationBuilder.AddColumn<int>(
                name: "particulartransaction_id",
                table: "IncomeStatements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "service",
                table: "IncomeStatements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeStatements_particulartransaction_id",
                table: "IncomeStatements",
                column: "particulartransaction_id");

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeStatements_Particulars_particulartransaction_id",
                table: "IncomeStatements",
                column: "particulartransaction_id",
                principalTable: "Particulars",
                principalColumn: "transaction_id");
        }
    }
}
