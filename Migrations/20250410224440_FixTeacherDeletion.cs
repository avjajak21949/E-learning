using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo03.Migrations
{
    /// <inheritdoc />
    public partial class FixTeacherDeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ba247fc2-0801-40d9-89af-099fbd5e42d3", "AQAAAAIAAYagAAAAEOATrJNJrZduFbVXI5ntifkLv3EYGm5mm7xpb+T/afAymHoDOalA6Z6SEZCw6ESKnw==", "e2b9b1d3-b7b6-40b7-9e3b-8ca7b93d5925" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "16fa962b-60de-456c-b899-ac8fd5150175", "AQAAAAIAAYagAAAAEDyiHWHlr0ay+wuVav7RBuiM2c/7jS5yI+O3PZHFS1A8W2VnHa6ItwGvDMhhmm0Ukw==", "ebef189f-2694-4ba8-aa9f-8ade9d761f88" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dba61248-fa69-47b0-b771-7cd8edbe9c5d", "AQAAAAIAAYagAAAAEOjZX3pyhGgEL53OYMh77xoVnybzoWlz6Qw8Ym5YHy4A51+SmxA7Sek1lT8Vx0NinA==", "02c65458-954f-4229-a45f-2692bf2a5081" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "69500a41-46ad-4939-8387-8343b17b5133", "AQAAAAIAAYagAAAAEJSF2HnSLFCgdVUpRNIHkklLbJk9uBNc0JrbXrFLERce41YUCjbPLq1166w2M0TX6g==", "e5ca6593-85ca-48b3-9a78-b23711e35777" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "19c99835-62cc-479c-9348-ccb6fa7ebc3e", "AQAAAAIAAYagAAAAEBtoC2Kv62GKFzttAq2Stf7E/ALkfcihY4yHWYQvC7EhLaKMAjfGs5DLSXKkUWmlKQ==", "4ccd1d6f-5687-4ea0-b60d-aa316f8cc874" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c0ea37c5-5a61-4a9e-85c6-bcb2df54b260", "AQAAAAIAAYagAAAAEAVefPkQ5errkuovWyOYMDJKBpy4fkO88P0YwDOaXlWBgnx1NOCMNE9dofidpgIS/A==", "974ba7b6-50ff-42ee-903d-b7a9c0513be6" });
        }
    }
}
