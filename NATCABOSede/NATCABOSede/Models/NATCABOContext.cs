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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=C0K3\\SQLEXPRESS;Database=NATCABO;User ID=sa;Password=080506;TrustServerCertificate=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DatosKpi>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IdProducto).IsFixedLength();
        });

        modelBuilder.Entity<Linea>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
