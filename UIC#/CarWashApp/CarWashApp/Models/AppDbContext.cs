using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CarWashApp.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Almacen> Almacens { get; set; }

    public virtual DbSet<Auditorium> Auditoria { get; set; }

    public virtual DbSet<Caja> Cajas { get; set; }

    public virtual DbSet<Carwash> Carwashes { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<Modelo> Modelos { get; set; }

    public virtual DbSet<Ordene> Ordenes { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<TiposServicio> TiposServicios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Vehiculo> Vehiculos { get; set; }

    public virtual DbSet<VistaCajaDiarium> VistaCajaDiaria { get; set; }

    public virtual DbSet<VistaInventarioActual> VistaInventarioActuals { get; set; }

    public virtual DbSet<VistaOrdenesPendiente> VistaOrdenesPendientes { get; set; }

    public virtual DbSet<VistaServiciosHoy> VistaServiciosHoys { get; set; }

    public virtual DbSet<VistaVehiculosPorCliente> VistaVehiculosPorClientes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=CarWashDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Almacen>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__ALMACEN__09889210C8874D21");

            entity.ToTable("ALMACEN");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Auditorium>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria).HasName("PK__Auditori__7FD13FA0C5FAED1C");

            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Operacion)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TablaAfectada)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Usuario)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Caja>(entity =>
        {
            entity.HasKey(e => e.IdCaja).HasName("PK__CAJA__3B7BF2C584530BAC");

            entity.ToTable("CAJA");

            entity.Property(e => e.Fecha).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TipoPago)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Tipo_Pago");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.Cajas)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CAJA__IdServicio__76969D2E");
        });

        modelBuilder.Entity<Carwash>(entity =>
        {
            entity.HasKey(e => e.IdCarwash).HasName("PK__CARWASH__FD1583BBC8ABEBD9");

            entity.ToTable("CARWASH");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasMany(d => d.IdEmpleados).WithMany(p => p.IdCarwashes)
                .UsingEntity<Dictionary<string, object>>(
                    "CarwashHasEmpleado",
                    r => r.HasOne<Empleado>().WithMany()
                        .HasForeignKey("IdEmpleado")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__CARWASH_H__IdEmp__5DCAEF64"),
                    l => l.HasOne<Carwash>().WithMany()
                        .HasForeignKey("IdCarwash")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__CARWASH_H__IdCar__5CD6CB2B"),
                    j =>
                    {
                        j.HasKey("IdCarwash", "IdEmpleado").HasName("PK__CARWASH___01F35B02D71BFE63");
                        j.ToTable("CARWASH_HAS_EMPLEADOS");
                    });
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__CLIENTES__D594664273082B82");

            entity.ToTable("CLIENTES");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Materno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Paterno");
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PK__EMPLEADO__CE6D8B9EECE01515");

            entity.ToTable("EMPLEADOS", tb => tb.HasTrigger("trg_AuditarEmpleado"));

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Materno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_Paterno");
            entity.Property(e => e.Cargo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaIngreso).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Sueldo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Turno)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.IdInventario).HasName("PK__INVENTAR__1927B20CBDC6E2B0");

            entity.ToTable("INVENTARIO");

            entity.Property(e => e.CostoUnitario).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.Inventarios)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__INVENTARI__IdPro__03F0984C");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.HasKey(e => e.IdMarca).HasName("PK__MARCAS__4076A8875B40185D");

            entity.ToTable("MARCAS");

            entity.HasIndex(e => e.NombreMarca, "UQ__MARCAS__42FE0ACB4ADC5FA8").IsUnique();

            entity.Property(e => e.NombreMarca)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Modelo>(entity =>
        {
            entity.HasKey(e => e.IdModelo).HasName("PK__MODELOS__CC30D30C0474D1CD");

            entity.ToTable("MODELOS");

            entity.Property(e => e.NombreModelo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdMarcaNavigation).WithMany(p => p.Modelos)
                .HasForeignKey(d => d.IdMarca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MODELOS__IdMarca__66603565");
        });

        modelBuilder.Entity<Ordene>(entity =>
        {
            entity.HasKey(e => e.IdOrden).HasName("PK__ORDENES__C38F300DC5D06196");

            entity.ToTable("ORDENES");

            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("pendiente");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InstruccionesEspeciales)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Ordenes)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ORDENES__IdClien__7B5B524B");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.Ordenes)
                .HasForeignKey(d => d.IdEmpleado)
                .HasConstraintName("FK__ORDENES__IdEmple__7E37BEF6");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.Ordenes)
                .HasForeignKey(d => d.IdServicio)
                .HasConstraintName("FK__ORDENES__IdServi__7F2BE32F");

            entity.HasOne(d => d.IdTipoServicioNavigation).WithMany(p => p.Ordenes)
                .HasForeignKey(d => d.IdTipoServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ORDENES__IdTipoS__7D439ABD");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Ordenes)
                .HasForeignKey(d => d.IdVehiculo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ORDENES__IdVehic__7C4F7684");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PK__PROVEEDO__E8B631AF517660E8");

            entity.ToTable("PROVEEDORES");

            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasMany(d => d.IdInventarios).WithMany(p => p.IdProveedors)
                .UsingEntity<Dictionary<string, object>>(
                    "ProveedoresHasInventario",
                    r => r.HasOne<Inventario>().WithMany()
                        .HasForeignKey("IdInventario")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__PROVEEDOR__IdInv__09A971A2"),
                    l => l.HasOne<Proveedore>().WithMany()
                        .HasForeignKey("IdProveedor")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__PROVEEDOR__IdPro__08B54D69"),
                    j =>
                    {
                        j.HasKey("IdProveedor", "IdInventario").HasName("PK__PROVEEDO__39244A8FD2C816BF");
                        j.ToTable("PROVEEDORES_HAS_INVENTARIO");
                    });
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__ROLES__2A49584C19A689FA");

            entity.ToTable("ROLES");

            entity.HasIndex(e => e.NombreRol, "UQ__ROLES__4F0B537F33F9726D").IsUnique();

            entity.Property(e => e.Descripcion)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.IdServicio).HasName("PK__SERVICIO__2DCCF9A260554FD2");

            entity.ToTable("SERVICIOS");

            entity.Property(e => e.Costo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Fecha).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SERVICIOS__IdCli__70DDC3D8");

            entity.HasOne(d => d.IdTipoServicioNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdTipoServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SERVICIOS__IdTip__72C60C4A");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdVehiculo)
                .HasConstraintName("FK__SERVICIOS__IdVeh__71D1E811");
        });

        modelBuilder.Entity<TiposServicio>(entity =>
        {
            entity.HasKey(e => e.IdTipoServicio).HasName("PK__TIPOS_SE__E29B3EA7AE7E0DD5");

            entity.ToTable("TIPOS_SERVICIO");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PrecioBase).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__USUARIOS__5B65BF979C4DE92E");

            entity.ToTable("USUARIOS");

            entity.HasIndex(e => e.NombreUsuario, "UQ__USUARIOS__6B0F5AE015F337A3").IsUnique();

            entity.HasIndex(e => e.IdEmpleado, "UQ__USUARIOS__CE6D8B9F975589CC").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Contrasena)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEmpleadoNavigation).WithOne(p => p.Usuario)
                .HasForeignKey<Usuario>(d => d.IdEmpleado)
                .HasConstraintName("FK__USUARIOS__IdEmpl__571DF1D5");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__USUARIOS__IdRol__5629CD9C");
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasKey(e => e.IdVehiculo).HasName("PK__VEHICULO__70861215B47C9797");

            entity.ToTable("VEHICULOS");

            entity.HasIndex(e => e.Placa, "UQ__VEHICULO__8310F99DD3CA4AF2").IsUnique();

            entity.Property(e => e.Placa)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Tipo)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VEHICULOS__IdCli__6A30C649");

            entity.HasOne(d => d.IdModeloNavigation).WithMany(p => p.Vehiculos)
                .HasForeignKey(d => d.IdModelo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VEHICULOS__IdMod__6B24EA82");
        });

        modelBuilder.Entity<VistaCajaDiarium>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vista_Caja_Diaria");

            entity.Property(e => e.PromedioPorTransaccion).HasColumnType("decimal(38, 6)");
            entity.Property(e => e.TipoPago)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Tipo_Pago");
            entity.Property(e => e.Total).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<VistaInventarioActual>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vista_Inventario_Actual");

            entity.Property(e => e.CostoUnitario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Producto)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ValorInventario).HasColumnType("decimal(21, 2)");
        });

        modelBuilder.Entity<VistaOrdenesPendiente>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vista_Ordenes_Pendientes");

            entity.Property(e => e.Cliente)
                .HasMaxLength(101)
                .IsUnicode(false);
            entity.Property(e => e.EmpleadoAsignado)
                .HasMaxLength(101)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.InstruccionesEspeciales)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Placa)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TipoServicio)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VistaServiciosHoy>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vista_Servicios_Hoy");

            entity.Property(e => e.Cliente)
                .HasMaxLength(152)
                .IsUnicode(false);
            entity.Property(e => e.Costo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Placa)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TipoServicio)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VistaVehiculosPorCliente>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Vista_Vehiculos_Por_Cliente");

            entity.Property(e => e.Cliente)
                .HasMaxLength(152)
                .IsUnicode(false);
            entity.Property(e => e.Marca)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Modelo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Placa)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TipoVehiculo)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
