using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace service.indumepi.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEstoq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "SegundaSeparacao",
                table: "OrderProduct",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PrimeiraSeparacao",
                table: "OrderProduct",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ncm",
                table: "OrderProduct",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Conferido",
                table: "OrderProduct",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CodigoTabelaPreco",
                table: "OrderProduct",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CodigoProduto",
                table: "OrderProduct",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CodigoEmpresa",
                table: "OrderProduct",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CodigoCliente",
                table: "OrderProduct",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Estoque",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CCodigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CDescricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoLocalEstoque = table.Column<long>(type: "bigint", nullable: false),
                    EstoqueMinimo = table.Column<int>(type: "int", nullable: false),
                    Fisico = table.Column<int>(type: "int", nullable: false),
                    NCMC = table.Column<double>(type: "float", nullable: false),
                    NCodProd = table.Column<long>(type: "bigint", nullable: false),
                    NPendente = table.Column<int>(type: "int", nullable: false),
                    NPrecoUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NSaldo = table.Column<int>(type: "int", nullable: false),
                    Reservado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estoque", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Estoque");

            migrationBuilder.AlterColumn<long>(
                name: "SegundaSeparacao",
                table: "OrderProduct",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "PrimeiraSeparacao",
                table: "OrderProduct",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Ncm",
                table: "OrderProduct",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<long>(
                name: "Conferido",
                table: "OrderProduct",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CodigoTabelaPreco",
                table: "OrderProduct",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CodigoProduto",
                table: "OrderProduct",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CodigoEmpresa",
                table: "OrderProduct",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CodigoCliente",
                table: "OrderProduct",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
