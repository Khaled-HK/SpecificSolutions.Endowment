using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpecificSolutions.Endowment.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationToOffice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Offices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Offices");
        }
    }
}
