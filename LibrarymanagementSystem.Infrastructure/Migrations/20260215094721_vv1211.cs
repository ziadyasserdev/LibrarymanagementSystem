using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarymanagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class vv1211 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Publishers",
                newName: "Website");

            migrationBuilder.RenameColumn(
                name: "ContactEmail",
                table: "Publishers",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Publishers",
                newName: "Email");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Publishers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Publishers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Publishers");

            migrationBuilder.RenameColumn(
                name: "Website",
                table: "Publishers",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Publishers",
                newName: "ContactEmail");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Publishers",
                newName: "Address");
        }
    }
}
