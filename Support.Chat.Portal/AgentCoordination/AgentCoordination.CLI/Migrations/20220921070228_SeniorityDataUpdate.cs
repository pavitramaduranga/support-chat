using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgentCoordination.CLI.Migrations
{
    /// <inheritdoc />
    public partial class SeniorityDataUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Seniorities",
                keyColumn: "Id",
                keyValue: (short)2,
                column: "Name",
                value: "MidLevel");

            migrationBuilder.UpdateData(
                table: "Seniorities",
                keyColumn: "Id",
                keyValue: (short)4,
                column: "Name",
                value: "TeamLead");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Seniorities",
                keyColumn: "Id",
                keyValue: (short)2,
                column: "Name",
                value: " Mid-Level");

            migrationBuilder.UpdateData(
                table: "Seniorities",
                keyColumn: "Id",
                keyValue: (short)4,
                column: "Name",
                value: "Tem Lead");
        }
    }
}
