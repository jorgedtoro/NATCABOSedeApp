
## Linea comando Scaffold una tabla:
Scaffold-DbContext "Server=C0K3\SQLEXPRESS;Database=NATCABO;User ID=sa;Password=080506;TrustServerCertificate=True;Encrypt=False;" Microsoft.EntityFrameworkCore.SqlServer -Context NATCABOContext -Tables dbo.datosKPIS -OutputDir Models -Force
Scaffold-DbContext "Server=C0K3\SQLEXPRESS;Database=NATCABO;User ID=sa;Password=080506;TrustServerCertificate=True;Encrypt=False;" Microsoft.EntityFrameworkCore.SqlServer -Context NATCABOContext -Tables dbo.KPIsHistorico -OutputDir Models -Force