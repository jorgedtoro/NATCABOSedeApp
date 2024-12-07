using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NATCABOSede.Models;

public partial class NATCABOContext : DbContext
{
    public NATCABOContext()
    {
    }

    public NATCABOContext(DbContextOptions<NATCABOContext> options)
        : base(options)
    {
    }
    public virtual DbSet<DatosKpi> DatosKpis { get; set; }
    public virtual DbSet<Linea> Lineas { get; set; }
    public virtual DbSet<KpisHistorico> KpisHistoricos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=C0K3\\SQLEXPRESS;Database=NATCABO;User ID=sa;Password=080506;TrustServerCertificate=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KpisHistorico>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("KPIsHistorico");

            entity.Property(e => e.Confeccion).HasMaxLength(250);
            entity.Property(e => e.FTarget).HasColumnName("fTarget");
            entity.Property(e => e.Fecha).HasColumnType("date");
            entity.Property(e => e.KpiExtrapeso).HasColumnName("KPI_Extrapeso");
            entity.Property(e => e.KpiPm).HasColumnName("KPI_PM");
            entity.Property(e => e.KpiPpm).HasColumnName("KPI_PPM");
            entity.Property(e => e.LineaId).HasColumnName("LineaID");
            entity.Property(e => e.NMinutos).HasColumnName("N_Minutos");
            entity.Property(e => e.NOperaciones).HasColumnName("N_Operaciones");
            entity.Property(e => e.NPaquetes).HasColumnName("N_Paquetes");
            entity.Property(e => e.SName)
                .HasMaxLength(100)
                .HasColumnName("sName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}


//linea comandos scaffold:  Scaffold-DbContext "Server=C0K3\SQLEXPRESS;Database=NATCABO;User ID=sa;Password=080506;TrustServerCertificate=True;Encrypt=False;" Microsoft.EntityFrameworkCore.SqlServer -Context NATCABOContext -Tables KPIsHistorico -OutputDir Models -Force