# NATCABO Sede App

Aplicación web para la gestión y monitoreo de indicadores clave de rendimiento (KPIs) en líneas de producción.

## 🚀 Tecnologías

- **Backend**: .NET 7.0
- **Frontend**: ASP.NET Core MVC, JavaScript, Chart.js
- **Base de datos**: SQL Server
- **Autenticación**: Cookies
- **Librerías principales**:
  - Entity Framework Core 7.0
  - ClosedXML para generación de informes Excel
  - BCrypt.Net-Next para hashing de contraseñas
  - Chart.js para visualización de datos

## 📋 Requisitos previos

- .NET 7.0 SDK o superior
- SQL Server 2019 o superior
- Visual Studio 2022 o Visual Studio Code
- Node.js (para gestión de paquetes frontend)

## 🛠️ Configuración

1. **Clonar el repositorio**
   ```bash
   git clone [URL_DEL_REPOSITORIO]
   cd NATCABOSedeApp
   ```

2. **Configuración de la base de datos**
   - Crear una base de datos SQL Server
   - Actualizar la cadena de conexión en `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "NATCABOConnection": "Server=TU_SERVIDOR;Database=NATCABO;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```

3. **Aplicar migraciones**
   ```bash
   cd NATCABOSede/NATCABOSede
   dotnet ef database update
   ```

4. **Configuración de entorno**
   - Copiar `.env.example` a `.env` y configurar las variables necesarias

5. **Ejecutar la aplicación**
   ```bash
   dotnet run
   ```
   La aplicación estará disponible en: `https://localhost:5001`

## 📂 Estructura del proyecto

```
NATCABOSede/
├── Areas/
│   └── KPIS/               # Módulo de gestión de KPIs
│       ├── Controllers/     # Controladores del área
│       ├── Models/          # Modelos específicos del área
│       └── Views/           # Vistas del área
├── Controllers/             # Controladores principales
├── Models/                  # Modelos de datos
├── Services/                # Lógica de negocio
├── Utilities/               # Clases de utilidad
├── ViewModels/              # Modelos de vista
└── wwwroot/                 # Archivos estáticos
```

## 🔒 Autenticación

La aplicación utiliza autenticación basada en cookies. Los usuarios deben iniciar sesión para acceder a las funcionalidades.

## 📊 Características principales

- Monitoreo en tiempo real de KPIs de producción
- Gestión de líneas de producción
- Generación de informes
- Configuración de objetivos
- Histórico de datos

## 🧪 Pruebas

El proyecto incluye pruebas unitarias que se pueden ejecutar con:

```bash
dotnet test
```

## 🚀 Despliegue

La aplicación está configurada para desplegarse en entornos Windows con IIS o en contenedores Docker.

## 📝 Licencia

[Especificar licencia]

## 🤝 Contribución

1. Hacer fork del proyecto
2. Crear una rama para tu característica (`git checkout -b feature/AmazingFeature`)
3. Hacer commit de tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Hacer push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir un Pull Request
