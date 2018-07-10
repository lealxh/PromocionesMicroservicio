using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Promociones.Presentation.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Promociones",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    _MedioPagoIds = table.Column<string>(nullable: true),
                    _TipoMedioPagoIds = table.Column<string>(nullable: true),
                    _EntidadFinancieraIds = table.Column<string>(nullable: true),
                    _ProductoCategoriaIds = table.Column<string>(nullable: true),
                    MaxCantidadDeCuotas = table.Column<int>(nullable: true),
                    PorcentajeDecuento = table.Column<decimal>(nullable: true),
                    FechaInicio = table.Column<DateTime>(nullable: false),
                    FechaFin = table.Column<DateTime>(nullable: false),
                    Activo = table.Column<bool>(nullable: false),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promociones", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Promociones");
        }
    }
}
