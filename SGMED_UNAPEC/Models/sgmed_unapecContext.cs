using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SGMED_UNAPEC.Models
{
    public partial class sgmed_unapecContext : DbContext
    {
        public sgmed_unapecContext()
        {
        }

        public sgmed_unapecContext(DbContextOptions<sgmed_unapecContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Estado> Estados { get; set; } = null!;
        public virtual DbSet<Marca> Marcas { get; set; } = null!;
        public virtual DbSet<Medicamento> Medicamentos { get; set; } = null!;
        public virtual DbSet<Medico> Medicos { get; set; } = null!;
        public virtual DbSet<Paciente> Pacientes { get; set; } = null!;
        public virtual DbSet<Permiso> Permisos { get; set; } = null!;
        public virtual DbSet<Registrovisitum> Registrovisita { get; set; } = null!;
        public virtual DbSet<Rol> Rols { get; set; } = null!;
        public virtual DbSet<Tipofarmaco> Tipofarmacos { get; set; } = null!;
        public virtual DbSet<Ubicacion> Ubicacions { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            //    //                optionsBuilder.UseMySql("server=localhost;port=3306;database=sgmed_unapec;uid=root;password=Logaritmo.05", Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.0.1-mysql"));
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.ToTable("estado");

                entity.Property(e => e.EstadoId).HasColumnName("EstadoID");

                entity.Property(e => e.Descripcion).HasMaxLength(50);
            });

            modelBuilder.Entity<Marca>(entity =>
            {
                entity.ToTable("marca");

                entity.HasIndex(e => e.EstadoId, "FK_Marca_Estado");

                entity.Property(e => e.MarcaId).HasColumnName("MarcaID");

                entity.Property(e => e.Descripcion).HasMaxLength(100);

                entity.Property(e => e.EstadoId).HasColumnName("EstadoID");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Marcas)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Marca_Estado");
            });

            modelBuilder.Entity<Medicamento>(entity =>
            {
                entity.ToTable("medicamento");

                entity.HasIndex(e => e.EstadoId, "FK_Medicamento_Estado");

                entity.HasIndex(e => e.MarcaId, "FK_Medicamento_Marca");

                entity.HasIndex(e => e.TipoFarmacoId, "FK_Medicamento_TipoFarmaco");

                entity.HasIndex(e => e.UbicacionId, "FK_Medicamento_Ubicacion");

                entity.Property(e => e.MedicamentoId).HasColumnName("MedicamentoID");

                entity.Property(e => e.Descripcion).HasMaxLength(100);

                entity.Property(e => e.Dosis).HasMaxLength(50);

                entity.Property(e => e.EstadoId).HasColumnName("EstadoID");

                entity.Property(e => e.MarcaId).HasColumnName("MarcaID");

                entity.Property(e => e.TipoFarmacoId).HasColumnName("TipoFarmacoID");

                entity.Property(e => e.UbicacionId).HasColumnName("UbicacionID");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Medicamentos)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Medicamento_Estado");

                entity.HasOne(d => d.Marca)
                    .WithMany(p => p.Medicamentos)
                    .HasForeignKey(d => d.MarcaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Medicamento_Marca");

                entity.HasOne(d => d.TipoFarmaco)
                    .WithMany(p => p.Medicamentos)
                    .HasForeignKey(d => d.TipoFarmacoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Medicamento_TipoFarmaco");

                entity.HasOne(d => d.Ubicacion)
                    .WithMany(p => p.Medicamentos)
                    .HasForeignKey(d => d.UbicacionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Medicamento_Ubicacion");
            });

            modelBuilder.Entity<Medico>(entity =>
            {
                entity.ToTable("medico");

                entity.HasIndex(e => e.EstadoId, "FK_Medico_Estado");

                entity.Property(e => e.MedicoId).HasColumnName("MedicoID");

                entity.Property(e => e.Cedula).HasMaxLength(11);

                entity.Property(e => e.Especialidad).HasMaxLength(100);

                entity.Property(e => e.EstadoId).HasColumnName("EstadoID");

                entity.Property(e => e.Nombre).HasMaxLength(100);

                entity.Property(e => e.TandaLabor).HasMaxLength(50);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Medicos)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Medico_Estado");
            });

            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.ToTable("paciente");

                entity.HasIndex(e => e.EstadoId, "FK_Paciente_Estado");

                entity.Property(e => e.PacienteId).HasColumnName("PacienteID");

                entity.Property(e => e.Cedula).HasMaxLength(11);

                entity.Property(e => e.EstadoId).HasColumnName("EstadoID");

                entity.Property(e => e.NoCarnet).HasMaxLength(20);

                entity.Property(e => e.Nombre).HasMaxLength(100);

                entity.Property(e => e.TipoPaciente).HasMaxLength(50);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Pacientes)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Paciente_Estado");
            });

            modelBuilder.Entity<Permiso>(entity =>
            {
                entity.ToTable("permiso");

                entity.Property(e => e.PermisoId).HasColumnName("PermisoID");

                entity.Property(e => e.Descripcion).HasMaxLength(100);
            });

            modelBuilder.Entity<Registrovisitum>(entity =>
            {
                entity.HasKey(e => e.VisitaId)
                    .HasName("PRIMARY");

                entity.ToTable("registrovisita");

                entity.HasIndex(e => e.EstadoId, "FK_RegistroVisita_Estado");

                entity.HasIndex(e => e.MedicamentoId, "FK_RegistroVisita_Medicamento");

                entity.HasIndex(e => e.MedicoId, "FK_RegistroVisita_Medico");

                entity.HasIndex(e => e.PacienteId, "FK_RegistroVisita_Paciente");

                entity.Property(e => e.VisitaId).HasColumnName("VisitaID");

                entity.Property(e => e.EstadoId).HasColumnName("EstadoID");

                entity.Property(e => e.HoraVisita).HasColumnType("time");
                entity.Property(e => e.FechaVisita)
                      .HasConversion(
                          v => v.ToDateTime(TimeOnly.MinValue), 
                          v => DateOnly.FromDateTime(v));      

                entity.Property(e => e.MedicamentoId).HasColumnName("MedicamentoID");

                entity.Property(e => e.MedicoId).HasColumnName("MedicoID");

                entity.Property(e => e.PacienteId).HasColumnName("PacienteID");

                entity.Property(e => e.Recomendaciones).HasMaxLength(255);

                entity.Property(e => e.Sintomas).HasMaxLength(255);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Registrovisita)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegistroVisita_Estado");

                entity.HasOne(d => d.Medicamento)
                    .WithMany(p => p.Registrovisita)
                    .HasForeignKey(d => d.MedicamentoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegistroVisita_Medicamento");

                entity.HasOne(d => d.Medico)
                    .WithMany(p => p.Registrovisita)
                    .HasForeignKey(d => d.MedicoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegistroVisita_Medico");

                entity.HasOne(d => d.Paciente)
                    .WithMany(p => p.Registrovisita)
                    .HasForeignKey(d => d.PacienteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegistroVisita_Paciente");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("rol");

                entity.Property(e => e.RolId).HasColumnName("RolID");

                entity.Property(e => e.Descripcion).HasMaxLength(50);

                entity.HasMany(d => d.Permisos)
                    .WithMany(p => p.Rols)
                    .UsingEntity<Dictionary<string, object>>(
                        "Rolpermiso",
                        l => l.HasOne<Permiso>().WithMany().HasForeignKey("PermisoId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_RolPermiso_Permiso"),
                        r => r.HasOne<Rol>().WithMany().HasForeignKey("RolId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_RolPermiso_Rol"),
                        j =>
                        {
                            j.HasKey("RolId", "PermisoId").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("rolpermiso");

                            j.HasIndex(new[] { "PermisoId" }, "FK_RolPermiso_Permiso");

                            j.IndexerProperty<int>("RolId").HasColumnName("RolID");

                            j.IndexerProperty<int>("PermisoId").HasColumnName("PermisoID");
                        });
            });

            modelBuilder.Entity<Tipofarmaco>(entity =>
            {
                entity.ToTable("tipofarmaco");

                entity.HasIndex(e => e.EstadoId, "FK_TipoFarmaco_Estado");

                entity.Property(e => e.TipoFarmacoId).HasColumnName("TipoFarmacoID");

                entity.Property(e => e.Descripcion).HasMaxLength(100);

                entity.Property(e => e.EstadoId).HasColumnName("EstadoID");

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Tipofarmacos)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TipoFarmaco_Estado");
            });

            modelBuilder.Entity<Ubicacion>(entity =>
            {
                entity.ToTable("ubicacion");

                entity.HasIndex(e => e.EstadoId, "FK_Ubicacion_Estado");

                entity.Property(e => e.UbicacionId).HasColumnName("UbicacionID");

                entity.Property(e => e.Celda).HasMaxLength(50);

                entity.Property(e => e.Descripcion).HasMaxLength(100);

                entity.Property(e => e.EstadoId).HasColumnName("EstadoID");

                entity.Property(e => e.Estante).HasMaxLength(50);

                entity.Property(e => e.Tramo).HasMaxLength(50);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Ubicacions)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ubicacion_Estado");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario");

                entity.HasIndex(e => e.EstadoId, "FK_Usuario_Estado");

                entity.HasIndex(e => e.RolId, "FK_Usuario_Rol");

                entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

                entity.Property(e => e.EstadoId).HasColumnName("EstadoID");

                entity.Property(e => e.Nombre).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.RolId).HasColumnName("RolID");

                entity.Property(e => e.Username).HasMaxLength(50);

                entity.HasOne(d => d.Estado)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.EstadoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuario_Estado");

                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.RolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuario_Rol");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
