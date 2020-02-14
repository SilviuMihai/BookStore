using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Migrations
{
    public partial class SeedBooksInTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BooksInStore",
                columns: new[] { "Id", "BookGenre", "BooksInStore" },
                values: new object[,]
                {
                    { 1, "Comedy", "Bossypants" },
                    { 2, "Comedy", "Yes please" },
                    { 3, "Comedy", "Me Talk Pretty One Day" },
                    { 4, "Drama", "Hamlet" },
                    { 5, "Drama", "Visul unei nopti de vara" },
                    { 6, "Drama", "Vanatorii de zmeie" },
                    { 7, "Science-Fiction", "Razboiul Lumilor" },
                    { 8, "Science-Fiction", "Solaris" },
                    { 9, "Science-Fiction", "The Left Hand of Darkness" },
                    { 10, "Nature-Science", "Walden" },
                    { 11, "Nature-Science", "Almanahul unui tinut de nisip" },
                    { 12, "Nature-Science", "H is for Hawk" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BooksInStore",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BooksInStore",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BooksInStore",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BooksInStore",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "BooksInStore",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "BooksInStore",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "BooksInStore",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "BooksInStore",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "BooksInStore",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "BooksInStore",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "BooksInStore",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "BooksInStore",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
