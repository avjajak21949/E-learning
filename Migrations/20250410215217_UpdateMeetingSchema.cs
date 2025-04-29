using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo03.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMeetingSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Meetings");

            migrationBuilder.AlterColumn<string>(
                name: "MeetingLink",
                table: "Meetings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "HostUserId",
                table: "Meetings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MeetingLink",
                table: "Meetings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "HostUserId",
                table: "Meetings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Meetings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Meetings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "403a59e7-c5e0-4f40-abcf-eb3feb210ddb", "AQAAAAIAAYagAAAAEG+jHooltjSk1NhihLbDGzx46kdUFVikfs9p7CO3Dnj8M/dnq9Trx2TUDitPJOOHug==", "d6820129-42f5-4b08-8cf9-3587ba71c332" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f7f7b35e-ad46-4802-9291-d659ab42d529", "AQAAAAIAAYagAAAAENBY860rXS1iarsWap1EFVBS5iGo3ZTS+M9+9hmckbrNa+eFMz0oLHGMn24BGhOS7A==", "084f134c-1e2d-4e22-b84f-d011b5f01cf3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4560025e-d4a5-481a-95c8-006e393081eb", "AQAAAAIAAYagAAAAEOzCYoJNtthJ+Ox0YtRYfcsSvEPh6T/2FJ/brWTZE97sTs8uCJu541QiA2AjI38KJA==", "1efe5b98-2fbc-4d77-b49c-037c653272a2" });
        }
    }
}
