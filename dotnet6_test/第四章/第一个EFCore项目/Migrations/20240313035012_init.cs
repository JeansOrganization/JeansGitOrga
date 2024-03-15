using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace 第一个EFCore项目.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "T_Book",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, comment: "主键")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "姓名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    price = table.Column<double>(type: "double", maxLength: 10, nullable: false, defaultValue: 0.0, comment: "价格")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Book", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "T_Student",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, comment: "主键")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "姓名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    grade = table.Column<int>(type: "int", maxLength: 2, nullable: false, comment: "年级"),
                    indate = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "入学时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Student", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "T_Teacher",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, comment: "主键")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "姓名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    salary = table.Column<double>(type: "double", nullable: false, comment: "薪水"),
                    indate = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "入职时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Teacher", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_Book");

            migrationBuilder.DropTable(
                name: "T_Student");

            migrationBuilder.DropTable(
                name: "T_Teacher");
        }
    }
}
