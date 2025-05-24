#!/bin/bash

# Script para gestionar el contenedor de SQL Server para desarrollo
# Uso: ./docker-sql.sh [start|stop|status|logs|backup|restore]

CONTAINER_NAME="sql-server-natcabo"
BACKUP_DIR="./docker/backups"

# Crear directorio de respaldos si no existe
mkdir -p "$BACKUP_DIR"

case "$1" in
    start)
        echo "Iniciando contenedor de SQL Server..."
        docker compose up -d
        ;;
    stop)
        echo "Deteniendo contenedor de SQL Server..."
        docker compose down
        ;;
    status)
        docker ps -f name=$CONTAINER_NAME
        ;;
    logs)
        docker logs $CONTAINER_NAME
        ;;
    backup)
        TIMESTAMP=$(date +%Y%m%d_%H%M%S)
        BACKUP_FILE="$BACKUP_DIR/dbGrupalia_aux_$TIMESTAMP.bak"
        echo "Creando copia de seguridad en $BACKUP_FILE..."
        docker exec -it $CONTAINER_NAME /opt/mssql-tools/bin/sqlcmd \
            -S localhost -U sa -P 'YourStrong@Passw0rd' \
            -Q "BACKUP DATABASE [dbGrupalia_aux] TO DISK = N'/var/opt/mssql/backup/dbGrupalia_aux.bak' WITH NOFORMAT, INIT, NAME = 'dbGrupalia_aux-full', SKIP, NOREWIND, NOUNLOAD, STATS = 10"
        docker cp $CONTAINER_NAME:/var/opt/mssql/backup/dbGrupalia_aux.bak "$BACKUP_FILE"
        echo "Copia de seguridad completada: $BACKUP_FILE"
        ;;
    restore)
        if [ -z "$2" ]; then
            echo "Error: Debes especificar el archivo .bak para restaurar"
            echo "Uso: $0 restore /ruta/al/archivo.bak"
            exit 1
        fi
        BACKUP_FILE="$2"
        echo "Restaurando desde $BACKUP_FILE..."
        docker cp "$BACKUP_FILE" $CONTAINER_NAME:/var/opt/mssql/backup/restore.bak
        docker exec -it $CONTAINER_NAME /opt/mssql-tools/bin/sqlcmd \
            -S localhost -U sa -P 'YourStrong@Passw0rd' \
            -Q "RESTORE DATABASE [dbGrupalia_aux] FROM DISK = N'/var/opt/mssql/backup/restore.bak' WITH FILE = 1, NOUNLOAD, REPLACE, STATS = 5"
        echo "Restauración completada"
        ;;
    *)
        echo "Uso: $0 [start|stop|status|logs|backup|restore <archivo.bak>]"
        exit 1
        ;;
esac
