-- Insertar datos de ejemplo en TMarcoBizerba
IF NOT EXISTS (SELECT 1 FROM TMarcoBizerba)
BEGIN
    INSERT INTO TMarcoBizerba (IdLineaMarco, DeviceNoBizerba, ScreeNo)
    VALUES 
    (1, 101, 1),
    (2, 102, 2),
    (3, 103, 3);
    
    PRINT 'Datos de ejemplo insertados en TMarcoBizerba';
END
ELSE
BEGIN
    PRINT 'La tabla TMarcoBizerba ya contiene datos';
END
GO

-- Insertar datos de ejemplo en DatosKpisLive
IF NOT EXISTS (SELECT 1 FROM DatosKpisLive)
BEGIN
    INSERT INTO DatosKpisLive (idLinea, nombreLinea, PpmPersonalEnBalanza, PpmObjPersonaEnBalanza, nombreCliente, nombreProducto)
    VALUES 
    (1, 'Línea 1', 150.5, 140.0, 'Cliente A', 'Producto X'),
    (2, 'Línea 2', 120.0, 130.0, 'Cliente B', 'Producto Y'),
    (3, 'Línea 3', 180.2, 160.0, 'Cliente C', 'Producto Z');
    
    PRINT 'Datos de ejemplo insertados en DatosKpisLive';
END
ELSE
BEGIN
    PRINT 'La tabla DatosKpisLive ya contiene datos';
END
GO

-- Insertar datos de ejemplo en DatosKPIs_Historico
IF NOT EXISTS (SELECT 1 FROM DatosKPIs_Historico)
BEGIN
    INSERT INTO DatosKPIs_Historico (
        Confeccion,
        costeHora,
        costeKg,
        Desecho_Kg,
        Desecho_Perc,
        Extrapeso_Bizerba,
        Extrapeso_Marco,
        Fecha,
        FTT,
        horaInicioProduccion,
        idLinea,
        [MOD],
        nombreCliente,
        nombreLinea,
        paquetesRechazados_Disc,
        paquetesTotales,
        paquetesTotales_Disc,
        paquetesValidos,
        paquetesValidos_Disc,
        pesoObjetivo,
        pesoTotalReal,
        pesoTotalReal_Disc,
        PM_Bizerba,
        PM_Bizerba_Total,
        PM_Marco,
        PPM_Bizerba,
        PPM_Marco
    ) VALUES 
    ('CONF-001', 25.50, 0.05, 10.25, 2.3, 5.75, 6.20, 
     DATEADD(DAY, -1, GETDATE()), 1, DATEADD(HOUR, -2, GETDATE()),
     1, 8.5, 'Cliente A', 'Línea 1', 5, 100, 95, 90, 85, 
     100.00, 98.75, 94.50, 99.2, 99.8, 98.5, 150.75, 145.30),
    
    ('CONF-002', 26.30, 0.06, 8.75, 1.8, 4.25, 5.10, 
     GETDATE(), 1, DATEADD(HOUR, -1, GETDATE()),
     2, 7.8, 'Cliente B', 'Línea 2', 3, 120, 115, 110, 105, 
     120.50, 118.25, 114.75, 98.7, 99.1, 97.8, 165.30, 160.15);
    
    PRINT 'Datos de ejemplo insertados en DatosKPIs_Historico';
END
ELSE
BEGIN
    PRINT 'La tabla DatosKPIs_Historico ya contiene datos';
END
GO
