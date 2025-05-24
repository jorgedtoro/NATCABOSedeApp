# Configuración del Entorno de Desarrollo

Esta guía te ayudará a configurar la aplicación para que funcione en diferentes entornos (Windows, macOS, Linux).

## Requisitos Previos

- .NET 6.0 SDK o superior
- Docker Desktop (para ejecutar SQL Server en contenedores)
- SQL Server Management Studio (opcional, para administrar la base de datos)

## Configuración de la Base de Datos

### 1. Configuración para Docker (Recomendado para macOS/Linux)

1. Asegúrate de que Docker Desktop esté en ejecución
2. Ejecuta el siguiente comando para iniciar un contenedor de SQL Server:

```bash
cd /ruta/al/proyecto/docker
docker-compose up -d
```

3. Verifica que el contenedor esté en ejecución:
   ```bash
   docker ps
   ```

### 2. Configuración para Windows (SQL Server local)

1. Asegúrate de tener SQL Server instalado y en ejecución
2. Actualiza la cadena de conexión en `appsettings.json` con tus credenciales

## Configuración de la Aplicación

### 1. Configuración de Entorno

La aplicación usa diferentes configuraciones según el entorno:

- `appsettings.json` - Configuración base (no modificar directamente)
- `appsettings.Development.json` - Configuración para desarrollo local (crear basado en el ejemplo)
- `appsettings.Development.Mac.json` - Configuración específica para macOS

### 2. Configuración Inicial

1. Copia el archivo de ejemplo de configuración:
   ```bash
   cp appsettings.Development.example.json appsettings.Development.json
   ```

2. Edita el archivo `appsettings.Development.json` con tus credenciales locales

## Ejecución de la Aplicación

### Desarrollo Local

```bash
dotnet run --environment=Development
```

### Producción

```bash
dotnet run --environment=Production
```

## Solución de Problemas Comunes

### Error de conexión a la base de datos

1. Verifica que el servicio de SQL Server esté en ejecución
2. Comprueba que el puerto 1433 esté accesible
3. Asegúrate de que las credenciales en `appsettings.json` sean correctas

### Problemas con Docker

1. Verifica que Docker Desktop esté en ejecución
2. Intenta reiniciar los contenedores:
   ```bash
   docker-compose down
   docker-compose up -d
   ```

## Variables de Entorno

Puedes sobrescribir cualquier configuración usando variables de entorno. Por ejemplo:

```bash
export ConnectionStrings__NATCABOConnection="Server=localhost,1433;Database=dbGrupalia_aux;User ID=sa;Password=tu_contraseña;"
```

## Notas Importantes

- Nunca subas archivos de configuración con credenciales al control de versiones
- Usa contraseñas seguras en producción
- Mantén actualizadas las dependencias de seguridad
