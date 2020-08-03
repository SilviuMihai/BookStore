using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Migrations
{
    public partial class Added_UsersWithBooksDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserBooksConnectionDB",
                columns: table => new
                {
                    UserBooksId = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    BookId = table.Column<int>(nullable: false),
                    BookToBuy = table.Column<string>(nullable: true),
                    NrBooksOrdered = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBooksConnectionDB", x => x.UserBooksId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBooksConnectionDB");
        }
    }
}
