using Microsoft.EntityFrameworkCore;
using persist_net_backend.Models;

namespace persist_net_backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Entidades de autenticación
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserSession> UserSessions { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;

        // Entidades de gestión hotelera
        public DbSet<Hotel> Hotels { get; set; } = null!;
        public DbSet<TipoHabitacion> TiposHabitacion { get; set; } = null!;
        public DbSet<EstadoHabitacion> EstadosHabitacion { get; set; } = null!;
        public DbSet<Habitacion> Habitaciones { get; set; } = null!;
        public DbSet<Cliente> Clientes { get; set; } = null!;
        public DbSet<Temporada> Temporadas { get; set; } = null!;
        public DbSet<Tarifa> Tarifas { get; set; } = null!;
        public DbSet<Reserva> Reservas { get; set; } = null!;
        public DbSet<EstadoReserva> EstadosReserva { get; set; } = null!;
        public DbSet<Estancia> Estancias { get; set; } = null!;
        public DbSet<EstadoEstancia> EstadosEstancia { get; set; } = null!;
        public DbSet<ServicioExtra> ServiciosExtra { get; set; } = null!;
        public DbSet<ConsumoExtra> ConsumosExtra { get; set; } = null!;
        public DbSet<Factura> Facturas { get; set; } = null!;
        public DbSet<Pago> Pagos { get; set; } = null!;
        public DbSet<MetodoPago> MetodosPago { get; set; } = null!;
        public DbSet<Regimen> Regimenes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar relaciones y restricciones
            modelBuilder.Entity<Habitacion>()
                .HasOne(h => h.Hotel)
                .WithMany(h => h.Habitaciones)
                .HasForeignKey(h => h.HotelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Habitacion>()
                .HasOne(h => h.TipoHabitacion)
                .WithMany(t => t.Habitaciones)
                .HasForeignKey(h => h.TipoHabitacionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tarifa>()
                .HasOne(t => t.Temporada)
                .WithMany(te => te.Tarifas)
                .HasForeignKey(t => t.TemporadaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Cliente)
                .WithMany(c => c.Reservas)
                .HasForeignKey(r => r.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Habitacion)
                .WithMany(h => h.Reservas)
                .HasForeignKey(r => r.HabitacionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Estancia>()
                .HasOne(e => e.Reserva)
                .WithMany(r => r.Estancias)
                .HasForeignKey(e => e.ReservaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Estancia)
                .WithMany(e => e.Facturas)
                .HasForeignKey(f => f.EstanciaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Cliente)
                .WithMany(c => c.Facturas)
                .HasForeignKey(f => f.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ConsumoExtra>()
                .HasOne(c => c.Estancia)
                .WithMany(e => e.ConsumosExtra)
                .HasForeignKey(c => c.EstanciaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ConsumoExtra>()
                .HasOne(c => c.ServicioExtra)
                .WithMany(s => s.ConsumosExtra)
                .HasForeignKey(c => c.ServicioExtraId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Factura)
                .WithMany(f => f.Pagos)
                .HasForeignKey(p => p.FacturaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
