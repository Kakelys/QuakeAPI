using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuakeAPI.Migrations
{
    /// <inheritdoc />
    public partial class update_relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ActiveAccounts_AccountId",
                table: "ActiveAccounts");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveAccounts_AccountId",
                table: "ActiveAccounts",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ActiveAccounts_AccountId",
                table: "ActiveAccounts");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveAccounts_AccountId",
                table: "ActiveAccounts",
                column: "AccountId",
                unique: true);
        }
    }
}
