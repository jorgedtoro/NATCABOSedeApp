
## Linea comando Scaffold las dos tablas:
Scaffold-DbContext "Server=C0K3\SQLEXPRESS;Database=NATCABO;User ID=sa;Password=080506;TrustServerCertificate=True;Encrypt=False;" Microsoft.EntityFrameworkCore.SqlServer -Context NATCABOContext -Tables dbo.KPIsHistorico, dbo.datosKPIs -OutputDir Models -Force
