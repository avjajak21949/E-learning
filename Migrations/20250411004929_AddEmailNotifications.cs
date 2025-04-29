using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo03.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5a5b636e-9e74-4f85-b5d2-616971342e28", "AQAAAAIAAYagAAAAEPdvqfUhzkTDH4hkEk539KvpMMGf7+52VhpQfZOCgj+nHDFUNPgRuZdlBJv79T84xg==", "d7d8a240-51c9-48f2-861d-33b154569bbf" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1697bb52-e4de-4fcb-bf7d-60442fc3e30a", "AQAAAAIAAYagAAAAEC4BEmAdyVq/AWfeUQZAymQPuU13aWUbf8WEqEByIfBlXpcUVrUkCVAoOTR+bnQJ8w==", "6078c49f-ab24-4b67-9e26-bac3b724a8a1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cc2c36c2-3ec2-4899-a976-b8590e743572", "AQAAAAIAAYagAAAAEIvpg3X9InTp7KQRg2pdWtcnWwIpxUsqmPptWlbXAn6t0/vGDO2BedDNqXE5VfIN0g==", "b6463c51-c623-46f4-a248-f4410b96eec1" });
        }
    }
}
