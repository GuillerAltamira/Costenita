using Microsoft.EntityFrameworkCore;
using Costenita.Entidades;

namespace Costenita.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Lote> Lotes { get; set; }
    public DbSet<Venta> Ventas { get; set; }
    public DbSet<DetalleVenta> DetalleVentas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cliente>()
            .ToTable("Cliente");

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Nombre)
            .HasMaxLength(50);

        modelBuilder.Entity<Cliente>()
            .Property(c => c.Extension)
            .HasMaxLength(5);



        modelBuilder.Entity<Producto>()
            .ToTable("Producto");

        modelBuilder.Entity<Producto>()
            .Property(p => p.Nombre)
            .HasMaxLength(50);

        modelBuilder.Entity<Producto>()
            .Property(p => p.Tipo)
            .HasMaxLength(30);

        modelBuilder.Entity<Producto>()
            .Property(p => p.Precio)
            .HasColumnType("decimal(10,2)");



        modelBuilder.Entity<Lote>()
            .ToTable("Lote");

        modelBuilder.Entity<Lote>()
            .Property(l => l.Codigo)
            .HasMaxLength(20);

        modelBuilder.Entity<Lote>()
            .HasOne(l => l.Producto)
            .WithMany(p => p.Lotes)
            .HasForeignKey(l => l.ProductoId);



        modelBuilder.Entity<Venta>()
            .ToTable("Venta");

        modelBuilder.Entity<Venta>()
            .Property(v => v.Total)
            .HasColumnType("decimal(10,2)");



        modelBuilder.Entity<DetalleVenta>()
            .ToTable("DetalleVenta");

        modelBuilder.Entity<DetalleVenta>()
            .Property(d => d.PrecioUnitario)
            .HasColumnType("decimal(10,2)");

        modelBuilder.Entity<DetalleVenta>()
            .Property(d => d.Subtotal)
            .HasColumnType("decimal(10,2)");

        modelBuilder.Entity<DetalleVenta>()
            .HasOne(d => d.Venta)
            .WithMany(v => v.Detalles)
            .HasForeignKey(d => d.VentaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}