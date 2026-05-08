USE CarWashDB;
GO

-- 1. Servicios realizados hoy (sin cambios, funciona)
CREATE VIEW Vista_Servicios_Hoy AS
SELECT s.IdServicio, c.Nombre + ' ' + c.Apellido_Paterno AS Cliente,
       v.Placa, s.Costo, s.Fecha
FROM SERVICIOS s
JOIN CLIENTES c ON s.IdCliente = c.IdCliente
LEFT JOIN VEHICULOS v ON s.IdVehiculo = v.IdVehiculo
WHERE s.Fecha = CAST(GETDATE() AS DATE);
GO

-- 2. Vehículos por cliente (AJUSTADA a la estructura normalizada)
CREATE VIEW Vista_Vehiculos_Por_Cliente AS
SELECT 
    c.IdCliente,
    c.Nombre,
    c.Apellido_Paterno,
    v.Placa,
    ma.NombreMarca AS Marca,
    mo.NombreModelo AS Modelo
FROM CLIENTES c
JOIN VEHICULOS v ON c.IdCliente = v.IdCliente
JOIN MODELOS mo ON v.IdModelo = mo.IdModelo
JOIN MARCAS ma ON mo.IdMarca = ma.IdMarca;
GO

-- 3. Reporte de caja por día (sin cambios, funciona)
CREATE VIEW Vista_Caja_Diaria AS
SELECT Fecha, SUM(Precio) AS Total, Tipo_Pago
FROM CAJA
GROUP BY Fecha, Tipo_Pago;
GO