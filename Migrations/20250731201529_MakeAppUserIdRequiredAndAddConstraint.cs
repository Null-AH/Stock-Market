using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class MakeAppUserIdRequiredAndAddConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c1200e98-fd8a-4a29-bf58-9f88f9dd9834");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e102a961-1ec5-488f-a8a5-4c3b76914bd8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6bc4c635-ef4a-494e-bd2e-d18fd6186886", null, "Admin", "ADMIN" },
                    { "cb91c0a8-0a95-4a37-84cd-90ecc0277df7", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6bc4c635-ef4a-494e-bd2e-d18fd6186886");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb91c0a8-0a95-4a37-84cd-90ecc0277df7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "c1200e98-fd8a-4a29-bf58-9f88f9dd9834", null, "Admin", "ADMIN" },
                    { "e102a961-1ec5-488f-a8a5-4c3b76914bd8", null, "User", "USER" }
                });
        }
    }
}
