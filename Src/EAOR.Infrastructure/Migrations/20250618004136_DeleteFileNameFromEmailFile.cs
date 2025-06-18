using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EAOR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteFileNameFromEmailFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "EmailFile");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "EmailFile",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
