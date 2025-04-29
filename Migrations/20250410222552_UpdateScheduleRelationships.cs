using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo03.Migrations
{
    /// <inheritdoc />
    public partial class UpdateScheduleRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c2dd5ee6-76a6-4e6b-ba45-551e519f4d6e", "AQAAAAIAAYagAAAAEO5XT9JoydNhuXraapayVtVEL7xkzYztkzvXZ2a5QbE0tBS6IebiDy7eGgav8o7XDw==", "62bb9236-3a10-4a93-a860-54c0530e899d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d9e4c917-c2a2-4c87-84eb-f18a5d5c1c7b", "AQAAAAIAAYagAAAAEGI6oCvJRFk1u5P2HJcdwKhB8TJsOB6sX4Z626rIC++EOFd/BTLRdF03w813vfpGmw==", "1360218c-3acf-4334-8587-0406aa04555c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "33f7b5e8-46f3-4ff8-84d1-84cbe831860e", "AQAAAAIAAYagAAAAEF3aZ2GoFsAcp3GrQoq6dsgcpMF9zKl1F8mYlaepHJeHHaUImxudBrens1xNNOa9kw==", "d767b2cd-63ce-40ad-adfc-e1223641c7bf" });
        }
    }
}
