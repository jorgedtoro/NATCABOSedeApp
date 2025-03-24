
## Linea comando Scaffold las dos tablas:
Scaffold-DbContext "Server=C0K3\SQLEXPRESS;Database=dbGrupalia_aux;User ID=sa;Password=080506;TrustServerCertificate=True;Encrypt=False;" Microsoft.EntityFrameworkCore.SqlServer -Context NATCABOContext -Tables dbo.DatosKPIs_Historico, dbo.DatosKPIs_Live -OutputDir Models -Force

## Nuevo comando Scaffold:
Scaffold-DbContext "Server=C0K3\SQLEXPRESS;Database=dbGrupalia_aux;User ID=sa;Password=080506;TrustServerCertificate=True;Encrypt=False;" Microsoft.EntityFrameworkCore.SqlServer -Context NATCABOContext -Tables dbo.DatosKPIs_Historico, dbo.DatosKPIs_Live, dbo.T_ObjetivosConfecciones, dbo.T_Marco_Bizerba, dbo.vw_ObjetivosConfecciones, dbo.Usuarios -OutputDir Models -Force


## Para las vistas que no tienen clave foranea. Ponerlo en el dbContext.
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