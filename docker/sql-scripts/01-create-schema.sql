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

-- Crear la tabla DatosKPIs_Historico
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DatosKPIs_Historico')
BEGIN
    CREATE TABLE DatosKPIs_Historico (
        Confeccion NVARCHAR(255) NULL,
        costeHora DECIMAL(18, 2) NULL,
        costeKg DECIMAL(18, 2) NULL,
        Desecho_Kg DECIMAL(18, 2) NULL,
        Desecho_Perc DECIMAL(18, 2) NULL,
        Extrapeso_Bizerba DECIMAL(18, 2) NULL,
        Extrapeso_Marco DECIMAL(18, 2) NULL,
        Fecha DATETIME NULL,
        FTT BIT NULL,
        horaInicioProduccion DATETIME NULL,
        idLinea INT NULL,
        MOD DECIMAL(18, 2) NULL,
        nombreCliente NVARCHAR(255) NULL,
        nombreLinea NVARCHAR(255) NULL,
        paquetesRechazados_Disc INT NULL,
        paquetesTotales INT NULL,
        paquetesTotales_Disc INT NULL,
        paquetesValidos INT NULL,
        paquetesValidos_Disc INT NULL,
        pesoObjetivo DECIMAL(18, 2) NULL,
        pesoTotalReal DECIMAL(18, 2) NULL,
        pesoTotalReal_Disc DECIMAL(18, 2) NULL,
        PM_Bizerba DECIMAL(18, 2) NULL,
        PM_Bizerba_Total DECIMAL(18, 2) NULL,
        PM_Marco DECIMAL(18, 2) NULL,
        PPM_Bizerba DECIMAL(18, 2) NULL,
        PPM_Marco DECIMAL(18, 2) NULL
    );
    PRINT 'Tabla DatosKPIs_Historico creada correctamente.';
END
ELSE
BEGIN
    PRINT 'La tabla DatosKPIs_Historico ya existe.';
END
GO

-- Crear la tabla DatosKpisLive
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DatosKpisLive')
BEGIN
    CREATE TABLE DatosKpisLive (
        IdLinea SMALLINT NULL,
        NombreLinea NVARCHAR(255) NULL,
        NombreLote NVARCHAR(255) NULL,
        NombreProducto NVARCHAR(255) NULL,
        NombreCliente NVARCHAR(255) NULL,
        HoraInicioLote DATETIME NULL,
        PaquetesValidos INT NULL,
        NumeroOperadores INT NULL,
        PesoObjetivo FLOAT NULL,
        PaquetesRequeridos INT NULL,
        PpmObjetivo FLOAT NULL,
        FInputWeight FLOAT NULL,
        PersonalCorrecto INT NULL,
        DiscriminadorEnUso INT NULL,
        PpmPersonalEnBalanza FLOAT NULL,
        PpmObjPersonaEnBalanza FLOAT NULL,
        PpmLinea FLOAT NULL,
        PpmObj FLOAT NULL,
        ModObj FLOAT NULL,
        FttObj FLOAT NULL,
        PpmBalanzasOk INT NULL,
        PpmLineaOk INT NULL,
        Mod FLOAT NULL,
        Ftt INT NULL,
        RangosOk INT NULL,
        ExpulsionAireOk INT NULL
    );
    PRINT 'Tabla DatosKpisLive creada correctamente.';
END
ELSE
BEGIN
    PRINT 'La tabla DatosKpisLive ya existe.';
END
GO

-- Crear la tabla TMarcoBizerba
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TMarcoBizerba')
BEGIN
    CREATE TABLE TMarcoBizerba (
        IdLineaMarco SMALLINT NULL,
        DeviceNoBizerba SMALLINT NULL,
        ScreeNo SMALLINT NULL
    );
    PRINT 'Tabla TMarcoBizerba creada correctamente.';
END
ELSE
BEGIN
    PRINT 'La tabla TMarcoBizerba ya existe.';
END
GO

-- Crear la tabla TObjetivosConfecciones
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TObjetivosConfecciones')
BEGIN
    CREATE TABLE TObjetivosConfecciones (
        SCode NVARCHAR(255) PRIMARY KEY,
        PercExtraOper SMALLINT NULL,
        PpmObj FLOAT NULL,
        ModObj FLOAT NULL,
        FttObj FLOAT NULL
    );
    PRINT 'Tabla TObjetivosConfecciones creada correctamente.';
END
ELSE
BEGIN
    PRINT 'La tabla TObjetivosConfecciones ya existe.';
END
GO

-- Crear la tabla Usuarios
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuarios')
BEGIN
    CREATE TABLE Usuarios (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(255) NOT NULL,
        Email NVARCHAR(255) NULL,
        PasswordHash NVARCHAR(255) NOT NULL,
        Rol NVARCHAR(50) NOT NULL,
        FechaRegistro DATETIME NULL
    );
    PRINT 'Tabla Usuarios creada correctamente.';
    
    -- Insertar un usuario administrador por defecto (contraseña: admin123)
    INSERT INTO Usuarios (Nombre, Email, PasswordHash, Rol, FechaRegistro)
    VALUES ('admin', 'admin@natcabo.com', '$2a$11$2X3q7v8x/A1qGHR/pL3XeOaGdc1xWdbN9x8Xj1zJvFvJY5aZ8KJXW', 'Administrador', GETDATE());
    
    PRINT 'Usuario administrador por defecto creado (usuario: admin, contraseña: admin123).';
END
ELSE
BEGIN
    PRINT 'La tabla Usuarios ya existe.';
END
GO

-- Crear la vista VwObjetivosConfecciones
IF NOT EXISTS (SELECT * FROM sys.views WHERE name = 'VwObjetivosConfecciones')
BEGIN
    EXEC('CREATE VIEW VwObjetivosConfecciones AS 
        SELECT 
            SCode,
            SCode AS SName,  -- Usamos SCode como SName por defecto, ajustar según sea necesario
            PercExtraOper,
            PpmObj,
            ModObj,
            FttObj
        FROM TObjetivosConfecciones');
    PRINT 'Vista VwObjetivosConfecciones creada correctamente.';
    
    -- Insertar datos de ejemplo si es necesario
    -- INSERT INTO TObjetivosConfecciones (SCode, PercExtraOper, PpmObj, ModObj, FttObj)
    -- VALUES 
    --     ('CONF001', 10, 100.0, 1.5, 95.0),
    --     ('CONF002', 15, 120.0, 1.6, 96.0);
END
ELSE
BEGIN
    PRINT 'La vista VwObjetivosConfecciones ya existe.';
END
GO

PRINT 'Esquema de base de datos creado correctamente.';
GO
