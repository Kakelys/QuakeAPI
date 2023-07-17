using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuakeAPI.Migrations
{
    /// <inheritdoc />
    public partial class account_tglink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TelegramId",
                table: "Accounts",
                newName: "TelegramChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TelegramChatId",
                table: "Accounts",
                newName: "TelegramId");
        }
    }
}
