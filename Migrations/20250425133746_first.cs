using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo03.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8589bd8c-f18f-4225-9d1e-31ef51426941", "AQAAAAIAAYagAAAAEGPRdU3MmM/Dco+5pvC83ofEOH/3dmPqYWw/2YabV/4EJ8VzPu7jZ+wRG1XM0J+OEQ==", "877386d0-aa66-4dfe-b144-753e5008b161" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6c7246d7-8007-4cb9-853f-c3b3ed5ec5f0", "AQAAAAIAAYagAAAAEF0UqaVlrHWqe9c/i8stktIYAjF7WNXu0r2UuSs9MnhMOvwReO8WSS3nkDVQMM+p1A==", "1ee8a844-c9b4-4179-adac-3d24a955b9d9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5e23ffe7-fb2d-4e69-ac4c-de6264612b19", "AQAAAAIAAYagAAAAEPbuewsdZ9hpbs+53Y1Ci9qz6xwZ4auUHntRIq0NfK+RuHnjSW+fm+l/6Tnp++4LMw==", "941b2e21-7823-4ad2-8660-0e0b58465ba4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e08bc926-8754-4bd6-8078-c18990a41bb1", "AQAAAAIAAYagAAAAED8pPRfRHsGk3ZdA5GjZCzdtCKYlH0rY0kryicYb2RPj2YPRCBkhYVSRy3UUpHZ6zA==", "e0a99507-4b3b-42de-a50f-58ac8d303312" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f93979ad-50bd-4fa0-af3a-0851f1a5703d", "AQAAAAIAAYagAAAAEJzC4uPd9LIy8DPyDl1ba1MUYBfqwwfa9yH3EzMNfKvp5aVRp9ZEIErZb5vHM5+q4w==", "910e9ff8-076b-4708-84b9-58c5c8564777" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "22749789-24dd-41e8-a47e-d745d58a7d99", "AQAAAAIAAYagAAAAEO1Rjd62IRRyBiZrBzwAUCeO6Q+hXa6mhUHF6aCIJliJAZ+Yh2dpO9CVbBnfkqg6zw==", "6dc02de6-9c9d-4dd7-9774-1665980ad9e8" });
        }
    }
}
