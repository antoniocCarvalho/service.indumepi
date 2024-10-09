using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace service.indumepi.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFamilies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodFamilia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodInt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Codigo = table.Column<long>(type: "bigint", nullable: false),
                    Inativo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomeFamilia = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Families");
        }
    }
}
