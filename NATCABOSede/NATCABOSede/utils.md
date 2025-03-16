
## Linea comando Scaffold las dos tablas:
Scaffold-DbContext "Server=C0K3\SQLEXPRESS;Database=dbGrupalia_aux;User ID=sa;Password=080506;TrustServerCertificate=True;Encrypt=False;" Microsoft.EntityFrameworkCore.SqlServer -Context NATCABOContext -Tables dbo.DatosKPIs_Historico, dbo.DatosKPIs_Live -OutputDir Models -Force
