# Configuración de SQL Server en Docker

Este directorio contiene la configuración necesaria para ejecutar SQL Server en un contenedor Docker, lo que es especialmente útil para desarrollo en entornos Mac o Linux.

## Requisitos previos

- Docker Desktop instalado y en ejecución
- Docker Compose (generalmente incluido con Docker Desktop)

## Cómo usar

### 1. Iniciar el contenedor de SQL Server

```bash
# Dar permisos de ejecución al script (solo la primera vez)
chmod +x ../docker-sql.sh

# Iniciar el contenedor
./../docker-sql.sh start
```

### 2. Detener el contenedor

```bash
./../docker-sql.sh stop
```

### 3. Ver el estado del contenedor

```bash
./../docker-sql.sh status
```

### 4. Ver los logs del contenedor

```bash
./../docker-sql.sh logs
```

### 5. Crear una copia de seguridad de la base de datos

```bash
./../docker-sql.sh backup
```

Las copias de seguridad se guardan en `./docker/backups/`

### 6. Restaurar una base de datos desde una copia de seguridad

```bash
./../docker-sql.sh restore /ruta/a/tu/archivo.bak
```

## Configuración de la aplicación

Para que tu aplicación se conecte a la base de datos en Docker, usa la siguiente cadena de conexión en tu `appsettings.json`:

```json
"DockerConnection": "Server=localhost,1433;Database=dbGrupalia_aux;User ID=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;Trusted_Connection=False;MultipleActiveResultSets=true;"
```

## Inicialización de la base de datos

Puedes agregar scripts SQL en el directorio `./docker/sql-scripts/` y se ejecutarán automáticamente al iniciar el contenedor.

## Solución de problemas

- **Error de puerto en uso**: Asegúrate de que el puerto 1433 no esté siendo utilizado por otra instancia de SQL Server.
- **Error de contraseña**: Verifica que la contraseña en el archivo `docker-compose.yml` coincida con la de tu cadena de conexión.
- **Problemas de permisos**: Si tienes problemas con los permisos, intenta ejecutar los comandos con `sudo`.

## Seguridad

⚠️ **ADVERTENCIA**: Esta configuración es solo para desarrollo. No uses contraseñas débiles en producción y nunca expongas el puerto 1433 a Internet.
