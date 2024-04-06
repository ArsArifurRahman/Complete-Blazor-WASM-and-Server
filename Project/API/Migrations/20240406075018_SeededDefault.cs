using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class SeededDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "070a9777-88b4-49c6-bf12-12e2fbc3817f", null, "Administrator", "ADMINISTRATOR" },
                    { "bafdf2bf-c6d1-4064-a431-df82f89e3fdd", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "41a921d4-44a9-48e1-8482-8816d20b0281", 0, "3e9119b3-76d2-4d53-80a3-c47a7c3c7edc", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "USER@BOOKSTORE.COM", "AQAAAAIAAYagAAAAECoyx93mJpPO1aI0bTk6JLBMeK1vkJqyShNyLrBQvnoEf5WKgoNWw++CmOb+/m3xlA==", null, false, "4d7b1620-9488-476e-b7c5-b97cb30e919c", false, "user@bookstore.com" },
                    { "f9a88723-6704-49a0-b1c1-730927840c45", 0, "859034f2-29a5-43c9-9f67-0f0bc1fad0a6", "admin@bookstore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAIAAYagAAAAENVrK8Gfo/DflLJRIY7tePOM+xgEKEWOKirpRjjBSdZU+dCXqlYVhVxb0k28IEHDTg==", null, false, "42d7793d-0790-4ab2-b05d-2c41ba4ec71d", false, "admin@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "bafdf2bf-c6d1-4064-a431-df82f89e3fdd", "41a921d4-44a9-48e1-8482-8816d20b0281" },
                    { "070a9777-88b4-49c6-bf12-12e2fbc3817f", "f9a88723-6704-49a0-b1c1-730927840c45" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "bafdf2bf-c6d1-4064-a431-df82f89e3fdd", "41a921d4-44a9-48e1-8482-8816d20b0281" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "070a9777-88b4-49c6-bf12-12e2fbc3817f", "f9a88723-6704-49a0-b1c1-730927840c45" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "070a9777-88b4-49c6-bf12-12e2fbc3817f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bafdf2bf-c6d1-4064-a431-df82f89e3fdd");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "41a921d4-44a9-48e1-8482-8816d20b0281");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f9a88723-6704-49a0-b1c1-730927840c45");
        }
    }
}
