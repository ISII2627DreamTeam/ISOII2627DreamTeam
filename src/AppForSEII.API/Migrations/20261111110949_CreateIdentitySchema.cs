using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppForSEII.API.Migrations
{
    /// <inheritdoc />
    public partial class CreateIdentitySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accesorios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Categoria = table.Column<int>(type: "int", nullable: false),
                    Compatibilidad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CantidadDisponible = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accesorios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    DireccionFacturacion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComprasAccesorios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCompra = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NombreCliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApellidosCliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DireccionEnvio = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NumeroTelefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MetodoPago = table.Column<int>(type: "int", nullable: false),
                    PrecioTotal = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprasAccesorios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComprasModelo3D",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCompra = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NombreCliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApellidosCliente = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CorreoElectronico = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DireccionFacturacion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PrecioTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MetodoPago = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprasModelo3D", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Impresora3Ds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Modelo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PrecioKilovatioHora = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    PrecioReserva = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impresora3Ds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materiales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrecioPorGramo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockGramos = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materiales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modelos3D",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Categoría = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Formato = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modelos3D", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Piezas3D",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Peso = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Categoria = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Piezas3D", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReservaImpresoras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaReserva = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NombreCliente = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApellidosCliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DireccionFacturacion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PrecioTotal = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    MetodoPago = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservaImpresoras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EncargosImpresion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaEncargo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NombreCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApellidosCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DireccionEnvio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroTelefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrecioTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MetodoPago = table.Column<int>(type: "int", nullable: false),
                    ClienteId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncargosImpresion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EncargosImpresion_AspNetUsers_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LineasCompraAccesorio",
                columns: table => new
                {
                    AccesorioId = table.Column<int>(type: "int", nullable: false),
                    CompraAccesoriosId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnidad = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineasCompraAccesorio", x => new { x.AccesorioId, x.CompraAccesoriosId });
                    table.ForeignKey(
                        name: "FK_LineasCompraAccesorio_Accesorios_AccesorioId",
                        column: x => x.AccesorioId,
                        principalTable: "Accesorios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineasCompraAccesorio_ComprasAccesorios_CompraAccesoriosId",
                        column: x => x.CompraAccesoriosId,
                        principalTable: "ComprasAccesorios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LicenciasModelo3D",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modelo3DId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenciasModelo3D", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LicenciasModelo3D_Modelos3D_Modelo3DId",
                        column: x => x.Modelo3DId,
                        principalTable: "Modelos3D",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LineasCompraModelo",
                columns: table => new
                {
                    Modelo3DID = table.Column<int>(type: "int", nullable: false),
                    CompraModelo3DID = table.Column<int>(type: "int", nullable: false),
                    CantidadLicencias = table.Column<int>(type: "int", nullable: false),
                    PrecioUnidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineasCompraModelo", x => new { x.Modelo3DID, x.CompraModelo3DID });
                    table.ForeignKey(
                        name: "FK_LineasCompraModelo_ComprasModelo3D_CompraModelo3DID",
                        column: x => x.CompraModelo3DID,
                        principalTable: "ComprasModelo3D",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineasCompraModelo_Modelos3D_Modelo3DID",
                        column: x => x.Modelo3DID,
                        principalTable: "Modelos3D",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialPieza3D",
                columns: table => new
                {
                    MaterialesValidosId = table.Column<int>(type: "int", nullable: false),
                    Piezas3DId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialPieza3D", x => new { x.MaterialesValidosId, x.Piezas3DId });
                    table.ForeignKey(
                        name: "FK_MaterialPieza3D_Materiales_MaterialesValidosId",
                        column: x => x.MaterialesValidosId,
                        principalTable: "Materiales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialPieza3D_Piezas3D_Piezas3DId",
                        column: x => x.Piezas3DId,
                        principalTable: "Piezas3D",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LineaReservas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ReservaImpresoraId = table.Column<int>(type: "int", nullable: false),
                    TiempoReserva = table.Column<int>(type: "int", nullable: false),
                    PrecioSubtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Impresora3DId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineaReservas", x => new { x.Id, x.ReservaImpresoraId });
                    table.ForeignKey(
                        name: "FK_LineaReservas_Impresora3Ds_Impresora3DId",
                        column: x => x.Impresora3DId,
                        principalTable: "Impresora3Ds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineaReservas_ReservaImpresoras_ReservaImpresoraId",
                        column: x => x.ReservaImpresoraId,
                        principalTable: "ReservaImpresoras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LineasEncargo",
                columns: table => new
                {
                    EncargoImpresionId = table.Column<int>(type: "int", nullable: false),
                    PiezaId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnidad = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineasEncargo", x => new { x.EncargoImpresionId, x.PiezaId });
                    table.ForeignKey(
                        name: "FK_LineasEncargo_EncargosImpresion_EncargoImpresionId",
                        column: x => x.EncargoImpresionId,
                        principalTable: "EncargosImpresion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineasEncargo_Materiales_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materiales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineasEncargo_Piezas3D_PiezaId",
                        column: x => x.PiezaId,
                        principalTable: "Piezas3D",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EncargosImpresion_ClienteId",
                table: "EncargosImpresion",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenciasModelo3D_Modelo3DId",
                table: "LicenciasModelo3D",
                column: "Modelo3DId");

            migrationBuilder.CreateIndex(
                name: "IX_LineaReservas_Impresora3DId",
                table: "LineaReservas",
                column: "Impresora3DId");

            migrationBuilder.CreateIndex(
                name: "IX_LineaReservas_ReservaImpresoraId",
                table: "LineaReservas",
                column: "ReservaImpresoraId");

            migrationBuilder.CreateIndex(
                name: "IX_LineasCompraAccesorio_CompraAccesoriosId",
                table: "LineasCompraAccesorio",
                column: "CompraAccesoriosId");

            migrationBuilder.CreateIndex(
                name: "IX_LineasCompraModelo_CompraModelo3DID",
                table: "LineasCompraModelo",
                column: "CompraModelo3DID");

            migrationBuilder.CreateIndex(
                name: "IX_LineasEncargo_MaterialId",
                table: "LineasEncargo",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_LineasEncargo_PiezaId",
                table: "LineasEncargo",
                column: "PiezaId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialPieza3D_Piezas3DId",
                table: "MaterialPieza3D",
                column: "Piezas3DId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "LicenciasModelo3D");

            migrationBuilder.DropTable(
                name: "LineaReservas");

            migrationBuilder.DropTable(
                name: "LineasCompraAccesorio");

            migrationBuilder.DropTable(
                name: "LineasCompraModelo");

            migrationBuilder.DropTable(
                name: "LineasEncargo");

            migrationBuilder.DropTable(
                name: "MaterialPieza3D");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Impresora3Ds");

            migrationBuilder.DropTable(
                name: "ReservaImpresoras");

            migrationBuilder.DropTable(
                name: "Accesorios");

            migrationBuilder.DropTable(
                name: "ComprasAccesorios");

            migrationBuilder.DropTable(
                name: "ComprasModelo3D");

            migrationBuilder.DropTable(
                name: "Modelos3D");

            migrationBuilder.DropTable(
                name: "EncargosImpresion");

            migrationBuilder.DropTable(
                name: "Materiales");

            migrationBuilder.DropTable(
                name: "Piezas3D");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
