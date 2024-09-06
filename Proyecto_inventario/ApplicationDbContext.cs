using Microsoft.EntityFrameworkCore;
using Proyecto_inventario.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<UsuarioRol> UsuarioRoles { get; set; }
    
    public DbSet<Propiedad> Propiedades { get; set; }
    public DbSet<FincaTerreno> FincasTerrenos { get; set; }
    public DbSet<Propietario> Propietarios { get; set; }
    public DbSet<ContratoAlquiler> ContratosAlquiler { get; set; }
    public DbSet<Pago> Pagos { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de UsuarioRol
        modelBuilder.Entity<UsuarioRol>()
            .HasKey(ur => new { ur.UsuarioId, ur.RolId });

        modelBuilder.Entity<UsuarioRol>()
            .HasOne(ur => ur.Usuario)
            .WithMany(u => u.UsuarioRoles)
            .HasForeignKey(ur => ur.UsuarioId);

        modelBuilder.Entity<UsuarioRol>()
            .HasOne(ur => ur.Rol)
            .WithMany(r => r.UsuarioRoles)
            .HasForeignKey(ur => ur.RolId);

        // Configuración de Propiedad
        modelBuilder.Entity<Propiedad>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Propiedad>()
            .HasOne(p => p.Propietario)
            .WithMany(pr => pr.Propiedades)
            .HasForeignKey(p => p.PropietarioId)
            .OnDelete(DeleteBehavior.Cascade); // Opcional: Configura el comportamiento en cascada

        modelBuilder.Entity<Propiedad>()
            .ToTable("Propiedades");

        // Configuración de Propietario
        modelBuilder.Entity<Propietario>()
            .HasKey(pr => pr.Id); // Usa Id en lugar de UsuarioId

        modelBuilder.Entity<Propietario>()
            .HasOne(pr => pr.Usuario)
            .WithOne(u => u.Propietario)
            .HasForeignKey<Propietario>(pr => pr.UsuarioId);

        modelBuilder.Entity<FincaTerreno>()
           .HasOne(ft => ft.Propietario)
           .WithMany(p => p.fincasTerrenos)
           .HasForeignKey(ft => ft.PropietarioId)
           .OnDelete(DeleteBehavior.Cascade); // Configura el comportamiento en cascada al eliminar un propietario

        modelBuilder.Entity<Propietario>()
            .ToTable("Propietarios");
    }
}
