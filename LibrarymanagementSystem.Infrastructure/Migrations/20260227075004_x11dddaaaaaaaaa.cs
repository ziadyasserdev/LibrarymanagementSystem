using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarymanagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class x11dddaaaaaaaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Locations",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Locations");
        }
    }
}
