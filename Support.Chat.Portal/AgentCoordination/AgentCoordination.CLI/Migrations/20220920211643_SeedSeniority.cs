using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AgentCoordination.CLI.Migrations
{
    /// <inheritdoc />
    public partial class SeedSeniority : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Seniorities",
                columns: new[] { "Id", "AssignOrder", "Efficiency", "Name" },
                values: new object[,]
                {
                    { (short)1, 0, 0.40000000000000002, "Junior" },
                    { (short)2, 0, 0.59999999999999998, " Mid-Level" },
                    { (short)3, 0, 0.80000000000000004, "Senior" },
                    { (short)4, 0, 0.5, "Tem Lead" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Seniorities",
                keyColumn: "Id",
                keyValue: (short)1);

            migrationBuilder.DeleteData(
                table: "Seniorities",
                keyColumn: "Id",
                keyValue: (short)2);

            migrationBuilder.DeleteData(
                table: "Seniorities",
                keyColumn: "Id",
                keyValue: (short)3);

            migrationBuilder.DeleteData(
                table: "Seniorities",
                keyColumn: "Id",
                keyValue: (short)4);
        }
    }
}
