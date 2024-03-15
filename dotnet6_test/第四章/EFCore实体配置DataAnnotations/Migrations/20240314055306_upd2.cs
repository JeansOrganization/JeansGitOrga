using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore实体配置DataAnnotations.Migrations
{
    public partial class upd2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "indate",
                table: "T_Teacher",
                newName: "inputDate");

            migrationBuilder.UpdateData(
                table: "T_Teacher",
                keyColumn: "name",
                keyValue: null,
                column: "name",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "T_Teacher",
                type: "varchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "inputDate",
                table: "T_Teacher",
                newName: "indate");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "T_Teacher",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(40)",
                oldMaxLength: 40)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
