using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarymanagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class xcx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LockedByAdminId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LockedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LockedByAdminId",
                table: "AspNetUsers");
        }
    }
}
