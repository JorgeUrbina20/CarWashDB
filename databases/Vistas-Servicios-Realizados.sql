-- ============================================================
-- VISTAS PARA CarWashDB
-- ============================================================
USE CarWashDB;
GO

-- 1. Servicios realizados hoy (MEJORADA)
--    Incluye tipo de servicio, cliente completo y fecha.
CREATE OR ALTER VIEW Vista_Servicios_Hoy AS
SELECT 
    s.IdServicio,
    c.IdCliente,
    c.Nombre + ' ' + ISNULL(c.Apellido_Paterno, '') + ' ' + ISNULL(c.Apellido_Materno, '') AS Cliente,
    v.Placa,
    ts.Nombre AS TipoServicio,
    s.Costo,
    s.Fecha
FROM SERVICIOS s
INNER JOIN CLIENTES c ON s.IdCliente = c.IdCliente
LEFT JOIN VEHICULOS v ON s.IdVehiculo = v.IdVehiculo
INNER JOIN TIPOS_SERVICIO ts ON s.IdTipoServicio = ts.IdTipoServicio
WHERE s.Fecha = CAST(GETDATE() AS DATE);
GO

-- 2. Vehículos por cliente (MEJORADA)
--    Agrega tipo del vehículo y estado del cliente.
CREATE OR ALTER VIEW Vista_Vehiculos_Por_Cliente AS
SELECT 
    c.IdCliente,
    c.Nombre + ' ' + ISNULL(c.Apellido_Paterno, '') + ' ' + ISNULL(c.Apellido_Materno, '') AS Cliente,
    c.Activo AS ClienteActivo,
    v.Placa,
    v.Tipo AS TipoVehiculo,
    ma.NombreMarca AS Marca,
    mo.NombreModelo AS Modelo
FROM CLIENTES c
INNER JOIN VEHICULOS v ON c.IdCliente = v.IdCliente
INNER JOIN MODELOS mo ON v.IdModelo = mo.IdModelo
INNER JOIN MARCAS ma ON mo.IdMarca = ma.IdMarca;
GO

-- 3. Reporte de caja por día (MEJORADA)
--    Agrega conteo de transacciones y promedio.
CREATE OR ALTER VIEW Vista_Caja_Diaria AS
SELECT 
    Fecha,
    SUM(Precio) AS Total,
    COUNT(*) AS NumeroTransacciones,
    AVG(Precio) AS PromedioPorTransaccion,
    Tipo_Pago
FROM CAJA
GROUP BY Fecha, Tipo_Pago;
GO

-- ============================================================
-- VISTAS ADICIONALES RECOMENDADAS
-- ============================================================

-- 4. Órdenes pendientes y en proceso (útil para lavadores y recepcionistas)
CREATE OR ALTER VIEW Vista_Ordenes_Pendientes AS
SELECT 
    o.IdOrden,
    o.Estado,
    o.FechaCreacion,
    o.InstruccionesEspeciales,
    c.Nombre + ' ' + ISNULL(c.Apellido_Paterno, '') AS Cliente,
    v.Placa,
    ts.Nombre AS TipoServicio,
    e.Nombre + ' ' + e.Apellido_Paterno AS EmpleadoAsignado
FROM ORDENES o
INNER JOIN CLIENTES c ON o.IdCliente = c.IdCliente
INNER JOIN VEHICULOS v ON o.IdVehiculo = v.IdVehiculo
INNER JOIN TIPOS_SERVICIO ts ON o.IdTipoServicio = ts.IdTipoServicio
LEFT JOIN EMPLEADOS e ON o.IdEmpleado = e.IdEmpleado
WHERE o.Estado IN ('pendiente', 'lavando');
GO

-- 5. Inventario actual (stock de productos)
CREATE OR ALTER VIEW Vista_Inventario_Actual AS
SELECT 
    a.IdProducto,
    a.Nombre AS Producto,
    a.Descripcion,
    i.Cantidad,
    i.CostoUnitario,
    (i.Cantidad * i.CostoUnitario) AS ValorInventario
FROM ALMACEN a
INNER JOIN INVENTARIO i ON a.IdProducto = i.IdProducto
WHERE i.Cantidad > 0;
GO