using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserIdForTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 9, 5, 17, 8, 23, 535, DateTimeKind.Local).AddTicks(4646), new DateTime(2024, 9, 5, 17, 8, 23, 535, DateTimeKind.Local).AddTicks(4709) });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserID",
                table: "Tasks",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_UserID",
                table: "Tasks",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_UserID",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_UserID",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Tasks");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 9, 4, 15, 43, 10, 88, DateTimeKind.Local).AddTicks(6760), new DateTime(2024, 9, 4, 15, 43, 10, 88, DateTimeKind.Local).AddTicks(6806) });
        }
    }
}
