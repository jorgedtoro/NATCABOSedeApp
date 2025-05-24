using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

    public virtual DbSet<DatosKpisHistorico> DatosKpisHistoricos { get; set; }

    public virtual DbSet<DatosKpisLive> DatosKpisLives { get; set; }
    public virtual DbSet<KpisHistoricoDto> KpisHistoricoDtos { get; set; }


    public virtual DbSet<TMarcoBizerba> TMarcoBizerbas { get; set; }

    public virtual DbSet<TObjetivosConfeccione> TObjetivosConfecciones { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<VwObjetivosConfeccione> VwObjetivosConfecciones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .AddJsonFile("appsettings.Development.Mac.json", optional: true)
                .Build();

            optionsBuilder.UseSqlServer(
                configuration.GetConnectionString("NATCABOConnection"),
                options => options.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DatosKpisHistorico>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DatosKPIs_Historico");

            entity.Property(e => e.Confeccion).HasMaxLength(255);
            entity.Property(e => e.CosteHora).HasColumnName("costeHora");
            entity.Property(e => e.CosteKg).HasColumnName("costeKg");
            entity.Property(e => e.DesechoKg).HasColumnName("Desecho_Kg");
            entity.Property(e => e.DesechoPerc).HasColumnName("Desecho_Perc");
            entity.Property(e => e.ExtrapesoBizerba).HasColumnName("Extrapeso_Bizerba");
            entity.Property(e => e.ExtrapesoMarco).HasColumnName("Extrapeso_Marco");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Ftt).HasColumnName("FTT");
            entity.Property(e => e.HoraInicioProduccion)
                .HasColumnType("datetime")
                .HasColumnName("horaInicioProduccion");
            entity.Property(e => e.IdLinea).HasColumnName("idLinea");
            entity.Property(e => e.Mod).HasColumnName("MOD");
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(255)
                .HasColumnName("nombreCliente");
            entity.Property(e => e.NombreLinea)
                .HasMaxLength(255)
                .HasColumnName("nombreLinea");
            entity.Property(e => e.PaquetesRechazadosDisc).HasColumnName("paquetesRechazados_Disc");
            entity.Property(e => e.PaquetesTotales).HasColumnName("paquetesTotales");
            entity.Property(e => e.PaquetesTotalesDisc).HasColumnName("paquetesTotales_Disc");
            entity.Property(e => e.PaquetesValidos).HasColumnName("paquetesValidos");
            entity.Property(e => e.PaquetesValidosDisc).HasColumnName("paquetesValidos_Disc");
            entity.Property(e => e.PesoObjetivo).HasColumnName("pesoObjetivo");
            entity.Property(e => e.PesoTotalReal).HasColumnName("pesoTotalReal");
            entity.Property(e => e.PesoTotalRealDisc).HasColumnName("pesoTotalReal_Disc");
            entity.Property(e => e.PmBizerba).HasColumnName("PM_Bizerba");
            entity.Property(e => e.PmBizerbaTotal).HasColumnName("PM_Bizerba_Total");
            entity.Property(e => e.PmMarco).HasColumnName("PM_Marco");
            entity.Property(e => e.PpmBizerba).HasColumnName("PPM_Bizerba");
            entity.Property(e => e.PpmMarco).HasColumnName("PPM_Marco");
        });

        modelBuilder.Entity<DatosKpisLive>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DatosKPIs_Live");

            entity.Property(e => e.CosteHora).HasColumnName("costeHora");
            entity.Property(e => e.CosteKg).HasColumnName("costeKg");
            entity.Property(e => e.ExpulsionAireOk).HasColumnName("expulsionAire_Ok");
            entity.Property(e => e.FInputWeight).HasColumnName("fInputWeight");
            entity.Property(e => e.Ftt).HasColumnName("FTT");
            entity.Property(e => e.FttObj).HasColumnName("FTT_Obj");
            entity.Property(e => e.HoraInicioLote)
                .HasColumnType("datetime")
                .HasColumnName("horaInicioLote");
            entity.Property(e => e.HoraInicioProduccion)
                .HasColumnType("datetime")
                .HasColumnName("horaInicioProduccion");
            entity.Property(e => e.HoraUltimoPaquete)
                .HasColumnType("datetime")
                .HasColumnName("horaUltimoPaquete");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdLinea).HasColumnName("idLinea");
            entity.Property(e => e.LBatchId).HasColumnName("lBatchID");
            entity.Property(e => e.MinutosTrabajados).HasColumnName("minutosTrabajados");
            entity.Property(e => e.Mod).HasColumnName("MOD");
            entity.Property(e => e.ModObj).HasColumnName("MOD_Obj");
            entity.Property(e => e.NPaquetes5min).HasColumnName("nPaquetes_5min");
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(255)
                .HasColumnName("nombreCliente");
            entity.Property(e => e.NombreLinea)
                .HasMaxLength(255)
                .HasColumnName("nombreLinea");
            entity.Property(e => e.NombreLote)
                .HasMaxLength(255)
                .HasColumnName("nombreLote");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(255)
                .HasColumnName("nombreProducto");
            entity.Property(e => e.NumeroOperadores).HasColumnName("numeroOperadores");
            entity.Property(e => e.PaquetesRechazadosDisc).HasColumnName("paquetesRechazados_Disc");
            entity.Property(e => e.PaquetesRequeridos).HasColumnName("paquetesRequeridos");
            entity.Property(e => e.PaquetesTotales).HasColumnName("paquetesTotales");
            entity.Property(e => e.PaquetesTotalesDisc).HasColumnName("paquetesTotales_Disc");
            entity.Property(e => e.PaquetesValidos).HasColumnName("paquetesValidos");
            entity.Property(e => e.PaquetesValidosDisc).HasColumnName("paquetesValidos_Disc");
            entity.Property(e => e.PesoObjetivo).HasColumnName("pesoObjetivo");
            entity.Property(e => e.PesoTotalDesperdicio).HasColumnName("pesoTotalDesperdicio");
            entity.Property(e => e.PesoTotalReal).HasColumnName("pesoTotalReal");
            entity.Property(e => e.PesoTotalRealDisc).HasColumnName("pesoTotalReal_Disc");
            entity.Property(e => e.PmBizerba).HasColumnName("PM_Bizerba");
            entity.Property(e => e.PmBizerbaTotal).HasColumnName("PM_Bizerba_Total");
            entity.Property(e => e.PpmBalanzasOk).HasColumnName("PPM_Balanzas_Ok");
            entity.Property(e => e.PpmBizerba).HasColumnName("PPM_Bizerba");
            entity.Property(e => e.PpmLinea).HasColumnName("PPM_Linea");
            entity.Property(e => e.PpmLineaOk).HasColumnName("PPM_Linea_Ok");
            entity.Property(e => e.PpmMarco).HasColumnName("PPM_Marco");
            entity.Property(e => e.PpmObj).HasColumnName("PPM_Obj");
            entity.Property(e => e.PpmObjPersonaEnBalanza).HasColumnName("PPM_Obj_PersonaEnBalanza");
            entity.Property(e => e.PpmObjetivo).HasColumnName("ppm_Objetivo");
            entity.Property(e => e.PpmPersonalEnBalanza).HasColumnName("PPM_PersonalEnBalanza");
            entity.Property(e => e.RangosOk).HasColumnName("rangos_Ok");
        });

        modelBuilder.Entity<TMarcoBizerba>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T_Marco_Bizerba");

            entity.Property(e => e.DeviceNoBizerba).HasColumnName("DeviceNo_BIZERBA");
            entity.Property(e => e.IdLineaMarco).HasColumnName("IdLinea_MARCO");
        });

        modelBuilder.Entity<TObjetivosConfeccione>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("T_ObjetivosConfecciones");

            entity.Property(e => e.FttObj).HasColumnName("FTT_Obj");
            entity.Property(e => e.ModObj).HasColumnName("MOD_Obj");
            entity.Property(e => e.PercExtraOper).HasColumnName("percExtraOper");
            entity.Property(e => e.PpmObj).HasColumnName("PPM_Obj");
            entity.Property(e => e.SCode)
                .HasMaxLength(255)
                .HasColumnName("sCode");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC0797E1CD27");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D10534E4733E62").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .HasDefaultValueSql("('Usuario')");
        });

        modelBuilder.Entity<VwObjetivosConfeccione>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ObjetivosConfecciones");

            entity.Property(e => e.FttObj).HasColumnName("FTT_Obj");
            entity.Property(e => e.ModObj).HasColumnName("MOD_Obj");
            entity.Property(e => e.PercExtraOper).HasColumnName("percExtraOper");
            entity.Property(e => e.PpmObj).HasColumnName("PPM_Obj");
            entity.Property(e => e.SCode)
                .HasMaxLength(255)
                .HasColumnName("sCode");
            entity.Property(e => e.SName)
                .HasMaxLength(255)
                .HasColumnName("sName");
        });
        //Jorge: KpisHistoricoDto no tiene primary Key. Lo incluimos aquí para que no de fallo la consulta del SP.
        modelBuilder.Entity<KpisHistoricoDto>()
            .HasNoKey()
            .ToView(null);
        OnModelCreatingPartial(modelBuilder);

        //Jorge: Al no tener Key debemos indicar una para guardar en base de datos
        modelBuilder.Entity<TObjetivosConfeccione>(entity =>
        {
            entity.HasKey(e => e.SCode);

            entity.ToTable("T_ObjetivosConfecciones");

            entity.Property(e => e.SCode).HasMaxLength(255).HasColumnName("sCode");

        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
