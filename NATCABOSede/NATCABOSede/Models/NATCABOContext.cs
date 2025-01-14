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

    public virtual DbSet<KpisHistorico> KpisHistoricos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS01;Database=dbGrupalia_aux;User ID=sa;Password=870104;TrustServerCertificate=True;Encrypt=False;");
        //=> optionsBuilder.UseSqlServer("Server=C0K3\\SQLEXPRESS;Database=NATCABO;User ID=sa;Password=080506;TrustServerCertificate=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DatosKpi>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DatosKPIs_Live");

            entity.Property(e => e.CosteHora).HasColumnName("costeHora");
            entity.Property(e => e.CosteKg).HasColumnName("costeKg");
            entity.Property(e => e.HoraInicioLote)
                .HasColumnType("datetime")
                .HasColumnName("horaInicioLote");
            entity.Property(e => e.HoraInicioProduccion)
                .HasColumnType("datetime")
                .HasColumnName("horaInicioProduccion");
            entity.Property(e => e.HoraUltimoPaquete)
                .HasColumnType("datetime")
                .HasColumnName("horaUltimoPaquete");
            entity.Property(e => e.IdLinea).HasColumnName("idLinea");
            entity.Property(e => e.MinutosTrabajados).HasColumnName("minutosTrabajados");
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(100)
                .HasColumnName("nombreCliente");
            entity.Property(e => e.NombreLinea)
                .HasMaxLength(100)
                .HasColumnName("nombreLinea");
            entity.Property(e => e.NombreLote)
                .HasMaxLength(100)
                .HasColumnName("nombreLote");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(100)
                .HasColumnName("nombreProducto");
            entity.Property(e => e.NumeroOperadores).HasColumnName("numeroOperadores");
            entity.Property(e => e.PaquetesRechazadosDisc).HasColumnName("paquetesRechazados_Disc");
            entity.Property(e => e.PaquetesRequeridos).HasColumnName("paquetesRequeridos");
            entity.Property(e => e.PaquetesTotales).HasColumnName("paquetesTotales");
            entity.Property(e => e.PaquetesTotalesDisc).HasColumnName("paquetesTotales_Disc");
            entity.Property(e => e.PaquetesValidos).HasColumnName("paquetesValidos");
            entity.Property(e => e.PesoObjetivo).HasColumnName("pesoObjetivo");
            entity.Property(e => e.PesoTotalDesperdicio).HasColumnName("pesoTotalDesperdicio");
            entity.Property(e => e.PesoTotalReal).HasColumnName("pesoTotalReal");
            entity.Property(e => e.PesoTotalRealDisc).HasColumnName("pesoTotalReal_Disc");
            entity.Property(e => e.PpmObjetivo).HasColumnName("ppm_Objetivo");

            entity.Property(e => e.PpmPromedio_5min).HasColumnName("nPaquetes_5min");
            entity.Property(e => e.PPM_Marco).HasColumnName("PPM_Marco");
            entity.Property(e => e.PPM_Bizerba).HasColumnName("PPM_Bizerba");
        });

        modelBuilder.Entity<KpisHistorico>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DatosKPIS_Historico");

            //entity.Property(e => e.Confeccion).HasMaxLength(255); //??

            entity.Property(e => e.Confeccion)
    .HasMaxLength(255)
    .HasColumnName("Confeccion");


            //entity.Property(e => e.FTarget).HasColumnName("fTarget");
            entity.Property(e => e.Fecha).HasColumnType("Fecha");
            entity.Property(e => e.Extrapeso_Marco).HasColumnName("Extrapeso_Marco");
            entity.Property(e => e.Extrapeso_Bizerba).HasColumnName("Extrapeso_Bizerba");
            entity.Property(e => e.PM_Marco).HasColumnName("PM_Marco");
            entity.Property(e => e.PM_Bizerba).HasColumnName("PM_Bizerba");
            entity.Property(e => e.PPM_Marco).HasColumnName("PPM_Marco");
            //entity.Property(e => e.PPM_Bizerba).HasColumnName("PPM_Bizerba");
            entity.Property(e => e.LineaId).HasColumnName("IdLinea");
            entity.Property(e => e.Desecho_Kg).HasColumnName("Desecho_Kg");
            entity.Property(e => e.Desecho_Perc).HasColumnName("Desecho_Perc");
            //entity.Property(e => e.Lote).HasMaxLength(255);
            //entity.Property(e => e.NMinutos).HasColumnName("N_Minutos");
            //entity.Property(e => e.NOperarios).HasColumnName("N_Operarios");
            //entity.Property(e => e.NPaquetes).HasColumnName("N_Paquetes");


            entity.Property(e => e.NombreCliente)
                .HasMaxLength(255)
                .HasColumnName("NombreCliente");

            entity.Property(e => e.NombreLinea)
                .HasMaxLength(255)
                .HasColumnName("NombreLinea");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
