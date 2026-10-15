using AppForSEII.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.DTOs.ApplicationUserDTO;

namespace AppForSEII.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);


    }


    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<CompraModelo3D> ComprasModelo3D { get; set; }
    public DbSet<LineaCompraModelo> LineasCompraModelo { get; set; }
    public DbSet<Modelo3D> Modelos3D { get; set; }
    public DbSet<LicenciaModelo3D> LicenciasModelo3D { get; set; }

    public DbSet<Impresora3D> Impresora3Ds { get; set; }
    public DbSet<ReservaImpresora> ReservaImpresoras { get; set; }
    public DbSet<LineaReserva> LineaReservas { get; set; }
    public DbSet<Accesorio> Accesorios { get; set; }
    public DbSet<CompraAccesorios> ComprasAccesorios { get; set; }
    public DbSet<EncargoImpresion> EncargosImpresion { get; set; }
    public DbSet<LineaCompraAccesorio> LineasCompraAccesorio { get; set; }
    public DbSet<LineaEncargo> LineasEncargo { get; set; }
    public DbSet<Material> Materiales { get; set; }
    public DbSet<Pieza3D> Piezas3D { get; set; }

}