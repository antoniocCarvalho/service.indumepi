using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace service.indumepi.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CnpjCpf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoCliente = table.Column<long>(type: "bigint", nullable: false),
                    NomeFantasia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RazaoSocial = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoProduto = table.Column<long>(type: "bigint", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CodigoFamilia = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroPedido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoCliente = table.Column<long>(type: "bigint", nullable: true),
                    CodigoEmpresa = table.Column<long>(type: "bigint", nullable: true),
                    Etapa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoPedido = table.Column<long>(type: "bigint", nullable: true),
                    Cancelada = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Encerrado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DtPrevisao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderProduct",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroPedido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoCliente = table.Column<long>(type: "bigint", nullable: true),
                    CodigoEmpresa = table.Column<long>(type: "bigint", nullable: true),
                    Etapa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cfop = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CnpjFabricante = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodigoProduto = table.Column<long>(type: "bigint", nullable: true),
                    CodigoTabelaPreco = table.Column<long>(type: "bigint", nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ean = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndicadorEscala = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotivoIcmsDesonerado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ncm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PercentualDesconto = table.Column<long>(type: "bigint", nullable: true),
                    Quantidade = table.Column<long>(type: "bigint", nullable: true),
                    Reservado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoDesconto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValorDeducao = table.Column<long>(type: "bigint", nullable: true),
                    ValorDesconto = table.Column<long>(type: "bigint", nullable: true),
                    ValorIcmsDesonerado = table.Column<long>(type: "bigint", nullable: true),
                    ValorMercadoria = table.Column<long>(type: "bigint", nullable: true),
                    ValorTotal = table.Column<long>(type: "bigint", nullable: true),
                    ValorUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PrimeiraSeparacao = table.Column<long>(type: "bigint", nullable: true),
                    SegundaSeparacao = table.Column<long>(type: "bigint", nullable: true),
                    Conferido = table.Column<long>(type: "bigint", nullable: true),
                    Enviado = table.Column<long>(type: "bigint", nullable: true),
                    Conferencia = table.Column<long>(type: "bigint", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Editado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProduct", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Families");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "OrderList");

            migrationBuilder.DropTable(
                name: "OrderProduct");
        }
    }
}
