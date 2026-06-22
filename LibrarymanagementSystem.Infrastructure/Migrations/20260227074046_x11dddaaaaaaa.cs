using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarymanagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class x11dddaaaaaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Locations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Locations");
        }
    }
}
