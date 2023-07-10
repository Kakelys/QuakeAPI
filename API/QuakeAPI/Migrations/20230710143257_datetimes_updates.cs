using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuakeAPI.Migrations
{
    /// <inheritdoc />
    public partial class datetimes_updates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Expires",
                table: "Tokens",
                newName: "ExpiresAt");

            migrationBuilder.RenameColumn(
                name: "Deleted",
                table: "Sessions",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Sessions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Disconnected",
                table: "ActiveAccounts",
                newName: "DisconnectedAt");

            migrationBuilder.RenameColumn(
                name: "Connected",
                table: "ActiveAccounts",
                newName: "ConnectedAt");

            migrationBuilder.AddColumn<int>(
                name: "MaxPlayers",
                table: "Locations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Accounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoggedAt",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxPlayers",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "LastLoggedAt",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "ExpiresAt",
                table: "Tokens",
                newName: "Expires");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "Sessions",
                newName: "Deleted");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Sessions",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "DisconnectedAt",
                table: "ActiveAccounts",
                newName: "Disconnected");

            migrationBuilder.RenameColumn(
                name: "ConnectedAt",
                table: "ActiveAccounts",
                newName: "Connected");
        }
    }
}
