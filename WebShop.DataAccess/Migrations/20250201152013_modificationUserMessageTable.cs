using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class modificationUserMessageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UsersMessages",
                keyColumn: "Id",
                keyValue: new Guid("737f0c25-043a-4c5d-8cc8-6dea3826c8f6"));

            migrationBuilder.AddColumn<DateOnly>(
                name: "CreateDate",
                table: "UsersMessages",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "CreateTime",
                table: "UsersMessages",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.InsertData(
                table: "UsersMessages",
                columns: new[] { "Id", "CreateDate", "CreateTime", "Email", "IsRead", "Message", "Subject", "UserId" },
                values: new object[] { new Guid("ba2591e1-245f-42c0-961e-6c66c9e1a335"), new DateOnly(1, 1, 1), new TimeOnly(0, 0, 0), "mati.kaweczynski@gmail.com", false, "Message Body", "Test", "93399d71-1573-4aec-b0cc-981f7dd388c4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UsersMessages",
                keyColumn: "Id",
                keyValue: new Guid("ba2591e1-245f-42c0-961e-6c66c9e1a335"));

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "UsersMessages");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "UsersMessages");

            migrationBuilder.InsertData(
                table: "UsersMessages",
                columns: new[] { "Id", "Email", "IsRead", "Message", "Subject", "UserId" },
                values: new object[] { new Guid("737f0c25-043a-4c5d-8cc8-6dea3826c8f6"), "mati.kaweczynski@gmail.com", false, "Message Body", "Test", "93399d71-1573-4aec-b0cc-981f7dd388c4" });
        }
    }
}
