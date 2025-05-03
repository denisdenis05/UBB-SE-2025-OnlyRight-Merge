using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WinUIApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UniqueConstraintuserdrink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserID",
                table: "Ratings");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserID_DrinkID",
                table: "Ratings",
                columns: new[] { "UserID", "DrinkID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserID_DrinkID",
                table: "Ratings");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserID",
                table: "Ratings",
                column: "UserID");
        }
    }
}
