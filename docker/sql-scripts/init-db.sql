-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'dbGrupalia_aux')
BEGIN
    CREATE DATABASE dbGrupalia_aux;
    PRINT 'Base de datos dbGrupalia_aux creada correctamente.';
END
ELSE
BEGIN
    PRINT 'La base de datos dbGrupalia_aux ya existe.';
END
GO

-- Usar la base de datos
USE dbGrupalia_aux;
GO

-- Aquí puedes agregar la estructura de tus tablas, vistas, procedimientos almacenados, etc.
-- Por ejemplo:
-- CREATE TABLE TuTabla (...);
-- INSERT INTO TuTabla (...) VALUES (...);

PRINT 'Base de datos configurada correctamente.';
GO
