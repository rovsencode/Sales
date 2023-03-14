using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirelloProject.Migrations
{
    public partial class genre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookGenres_Author_AuthorId",
                table: "BookGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_BookGenres_Books_BookId",
                table: "BookGenres");

            migrationBuilder.DropIndex(
                name: "IX_BookGenres_AuthorId",
                table: "BookGenres");

            migrationBuilder.DropIndex(
                name: "IX_BookGenres_BookId",
                table: "BookGenres");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "BookGenres");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "BookGenres",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookGenres_AuthorId",
                table: "BookGenres",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BookGenres_BookId",
                table: "BookGenres",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenres_Author_AuthorId",
                table: "BookGenres",
                column: "AuthorId",
                principalTable: "Author",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenres_Books_BookId",
                table: "BookGenres",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
