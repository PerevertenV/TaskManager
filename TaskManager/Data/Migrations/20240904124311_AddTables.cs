using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManager.Migrations
{
    /// <inheritdoc />
    public partial class AddTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "PasswordHash", "UpdatedAt", "Username" },
                values: new object[] { 1, new DateTime(2024, 9, 4, 15, 43, 10, 88, DateTimeKind.Local).AddTicks(6760), "admin@gmail.com", "AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAkb64s18hYUCWIzdN904QWQAAAAACAAAAAAAQZgAAAAEAACAAAAC8BqtfKq0oKZXOMmpOl9zvnhRpXCPkjgiuADALF7cPaAAAAAAOgAAAAAIAACAAAAAeqoVyRo/VH58lJwtphsCe/yuD48lWVbh8XKZ1Gm5jNBAAAAAxsSxUte/Rcl4Qg++FLxnhQAAAANcvVZSHD5MWdtuEGJwY8wQiFy5XO+pU2cnL7rOxjE4RjD8Jb4CKKqjE7RCjd2hCPobJpS2WKLRUGMU0D12ra/Q=", new DateTime(2024, 9, 4, 15, 43, 10, 88, DateTimeKind.Local).AddTicks(6806), "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
