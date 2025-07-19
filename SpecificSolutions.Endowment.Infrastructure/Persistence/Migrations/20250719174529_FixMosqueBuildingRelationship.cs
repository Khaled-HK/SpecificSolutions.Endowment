using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixMosqueBuildingRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Mosques_BuildingId",
                table: "Mosques");

            migrationBuilder.CreateIndex(
                name: "IX_Mosques_BuildingId",
                table: "Mosques",
                column: "BuildingId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Mosques_BuildingId",
                table: "Mosques");

            migrationBuilder.CreateIndex(
                name: "IX_Mosques_BuildingId",
                table: "Mosques",
                column: "BuildingId");
        }
    }
}
