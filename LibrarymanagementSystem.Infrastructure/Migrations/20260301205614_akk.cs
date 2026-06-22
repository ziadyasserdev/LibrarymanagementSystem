using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarymanagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class akk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCopy_Books_BookId",
                table: "BookCopy");

            migrationBuilder.DropForeignKey(
                name: "FK_BookCopy_Locations_LocationId",
                table: "BookCopy");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Locations_LocationId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanBooks_BookCopy_BookCopyId",
                table: "LoanBooks");

            migrationBuilder.DropIndex(
                name: "IX_Books_LocationId",
                table: "Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookCopy",
                table: "BookCopy");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Books");

            migrationBuilder.RenameTable(
                name: "BookCopy",
                newName: "BookCopies");

            migrationBuilder.RenameIndex(
                name: "IX_BookCopy_LocationId",
                table: "BookCopies",
                newName: "IX_BookCopies_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_BookCopy_BookId",
                table: "BookCopies",
                newName: "IX_BookCopies_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookCopies",
                table: "BookCopies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCopies_Books_BookId",
                table: "BookCopies",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookCopies_Locations_LocationId",
                table: "BookCopies",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LoanBooks_BookCopies_BookCopyId",
                table: "LoanBooks",
                column: "BookCopyId",
                principalTable: "BookCopies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCopies_Books_BookId",
                table: "BookCopies");

            migrationBuilder.DropForeignKey(
                name: "FK_BookCopies_Locations_LocationId",
                table: "BookCopies");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanBooks_BookCopies_BookCopyId",
                table: "LoanBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookCopies",
                table: "BookCopies");

            migrationBuilder.RenameTable(
                name: "BookCopies",
                newName: "BookCopy");

            migrationBuilder.RenameIndex(
                name: "IX_BookCopies_LocationId",
                table: "BookCopy",
                newName: "IX_BookCopy_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_BookCopies_BookId",
                table: "BookCopy",
                newName: "IX_BookCopy_BookId");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookCopy",
                table: "BookCopy",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Books_LocationId",
                table: "Books",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCopy_Books_BookId",
                table: "BookCopy",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookCopy_Locations_LocationId",
                table: "BookCopy",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
