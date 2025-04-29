using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo03.Migrations
{
    /// <inheritdoc />
    public partial class FixStudentDeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
