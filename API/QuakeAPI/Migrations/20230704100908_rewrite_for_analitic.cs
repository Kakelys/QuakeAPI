using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuakeAPI.Migrations
{
    /// <inheritdoc />
    public partial class rewrite_for_analitic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sessions_Name",
                table: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActiveAccounts",
                table: "ActiveAccounts");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Sessions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Deleted",
                table: "Sessions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ActiveAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "Connected",
                table: "ActiveAccounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Disconnected",
                table: "ActiveAccounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActiveAccounts",
                table: "ActiveAccounts",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ActiveAccounts",
                table: "ActiveAccounts");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ActiveAccounts");

            migrationBuilder.DropColumn(
                name: "Connected",
                table: "ActiveAccounts");

            migrationBuilder.DropColumn(
                name: "Disconnected",
                table: "ActiveAccounts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActiveAccounts",
                table: "ActiveAccounts",
                columns: new[] { "AccountId", "SessionId" });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_Name",
                table: "Sessions",
                column: "Name",
                unique: true);
        }
    }
}
