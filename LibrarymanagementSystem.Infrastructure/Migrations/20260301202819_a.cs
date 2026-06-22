using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarymanagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Locations_LocationId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanBooks_Books_BookId",
                table: "LoanBooks");

            migrationBuilder.DropColumn(
                name: "IsLost",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "LoanBooks",
                newName: "BookCopyId");

            migrationBuilder.RenameIndex(
                name: "IX_LoanBooks_BookId",
                table: "LoanBooks",
                newName: "IX_LoanBooks_BookCopyId");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "BookCopy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsLost = table.Column<bool>(type: "bit", nullable: false),
                    IsDamaged = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCopy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookCopy_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookCopy_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookCopy_BookId",
                table: "BookCopy",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCopy_LocationId",
                table: "BookCopy",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Locations_LocationId",
                table: "Books",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanBooks_BookCopy_BookCopyId",
                table: "LoanBooks",
                column: "BookCopyId",
                principalTable: "BookCopy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Locations_LocationId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanBooks_BookCopy_BookCopyId",
                table: "LoanBooks");

            migrationBuilder.DropTable(
                name: "BookCopy");

            migrationBuilder.RenameColumn(
                name: "BookCopyId",
                table: "LoanBooks",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_LoanBooks_BookCopyId",
                table: "LoanBooks",
                newName: "IX_LoanBooks_BookId");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLost",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Locations_LocationId",
                table: "Books",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoanBooks_Books_BookId",
                table: "LoanBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
