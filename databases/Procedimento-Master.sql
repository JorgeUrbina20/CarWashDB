USE CarWashDB;
GO

-- ============================================================
-- MARCAS Y MODELOS
-- ============================================================
CREATE OR ALTER PROC sp_ListarMarcas AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdMarca, NombreMarca FROM MARCAS ORDER BY NombreMarca;
END
GO

CREATE OR ALTER PROC sp_ListarModelosPorMarca
    @IdMarca INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdModelo, NombreModelo FROM MODELOS WHERE IdMarca = @IdMarca ORDER BY NombreModelo;
END
GO

-- ============================================================
-- TIPOS DE SERVICIO
-- ============================================================
CREATE OR ALTER PROC sp_ListarTiposServicio AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdTipoServicio, Nombre, Descripcion, PrecioBase
    FROM TIPOS_SERVICIO
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROC sp_ObtenerTipoServicioPorId
    @IdTipoServicio INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdTipoServicio, Nombre, Descripcion, PrecioBase
    FROM TIPOS_SERVICIO
    WHERE IdTipoServicio = @IdTipoServicio;
END
GO

-- ============================================================
-- VEHÍCULOS
-- ============================================================
CREATE OR ALTER PROC sp_InsertarVehiculo
    @IdCliente INT,
    @IdModelo INT,
    @Tipo VARCHAR(30),
    @Placa VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM CLIENTES WHERE IdCliente = @IdCliente)
    BEGIN
        RAISERROR('Cliente no encontrado.', 16, 1);
        RETURN;
    END
    IF EXISTS (SELECT 1 FROM VEHICULOS WHERE Placa = @Placa)
    BEGIN
        RAISERROR('Ya existe un vehículo con esa placa.', 16, 1);
        RETURN;
    END
    INSERT INTO VEHICULOS (IdCliente, IdModelo, Tipo, Placa)
    VALUES (@IdCliente, @IdModelo, @Tipo, @Placa);
    SELECT SCOPE_IDENTITY() AS NuevoId;
END
GO

CREATE OR ALTER PROC sp_ActualizarVehiculo
    @IdVehiculo INT,
    @IdCliente INT,
    @IdModelo INT,
    @Tipo VARCHAR(30),
    @Placa VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM VEHICULOS WHERE IdVehiculo = @IdVehiculo)
    BEGIN
        RAISERROR('Vehículo no encontrado.', 16, 1);
        RETURN;
    END
    IF EXISTS (SELECT 1 FROM VEHICULOS WHERE Placa = @Placa AND IdVehiculo <> @IdVehiculo)
    BEGIN
        RAISERROR('La placa ya está en uso por otro vehículo.', 16, 1);
        RETURN;
    END
    UPDATE VEHICULOS SET
        IdCliente = @IdCliente,
        IdModelo = @IdModelo,
        Tipo = @Tipo,
        Placa = @Placa
    WHERE IdVehiculo = @IdVehiculo;
END
GO

CREATE OR ALTER PROC sp_EliminarVehiculo
    @IdVehiculo INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM VEHICULOS WHERE IdVehiculo = @IdVehiculo)
    BEGIN
        RAISERROR('Vehículo no encontrado.', 16, 1);
        RETURN;
    END
    DELETE FROM VEHICULOS WHERE IdVehiculo = @IdVehiculo;
END
GO

CREATE OR ALTER PROC sp_LeerVehiculosPorCliente
    @IdCliente INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT v.IdVehiculo, v.Placa, v.Tipo,
           mo.NombreModelo AS Modelo,
           ma.NombreMarca AS Marca
    FROM VEHICULOS v
    INNER JOIN MODELOS mo ON v.IdModelo = mo.IdModelo
    INNER JOIN MARCAS ma ON mo.IdMarca = ma.IdMarca
    WHERE v.IdCliente = @IdCliente;
END
GO

CREATE OR ALTER PROC sp_BuscarVehiculoPorPlaca
    @Placa VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT v.IdVehiculo, v.Placa, v.Tipo,
           mo.NombreModelo AS Modelo,
           ma.NombreMarca AS Marca,
           c.Nombre + ' ' + c.Apellido_Paterno AS Cliente,
           c.IdCliente
    FROM VEHICULOS v
    INNER JOIN MODELOS mo ON v.IdModelo = mo.IdModelo
    INNER JOIN MARCAS ma ON mo.IdMarca = ma.IdMarca
    INNER JOIN CLIENTES c ON v.IdCliente = c.IdCliente
    WHERE v.Placa = @Placa;
END
GO

-- ============================================================
-- ÓRDENES
-- ============================================================
CREATE OR ALTER PROC sp_CrearOrden
    @IdCliente INT,
    @IdVehiculo INT,
    @IdTipoServicio INT,
    @Instrucciones VARCHAR(200) = NULL,
    @IdEmpleado INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO ORDENES (IdCliente, IdVehiculo, IdTipoServicio, InstruccionesEspeciales, IdEmpleado)
        VALUES (@IdCliente, @IdVehiculo, @IdTipoServicio, @Instrucciones, @IdEmpleado);
        SELECT SCOPE_IDENTITY() AS NuevaOrdenId;
    END TRY
    BEGIN CATCH
        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrMsg, 16, 1);
    END CATCH
END
GO

CREATE OR ALTER PROC sp_AsignarEmpleadoOrden
    @IdOrden INT,
    @IdEmpleado INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM ORDENES WHERE IdOrden = @IdOrden)
    BEGIN
        RAISERROR('Orden no encontrada.', 16, 1);
        RETURN;
    END
    UPDATE ORDENES SET IdEmpleado = @IdEmpleado WHERE IdOrden = @IdOrden;
END
GO

CREATE OR ALTER PROC sp_CambiarEstadoOrden
    @IdOrden INT,
    @NuevoEstado VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM ORDENES WHERE IdOrden = @IdOrden)
    BEGIN
        RAISERROR('Orden no encontrada.', 16, 1);
        RETURN;
    END
    IF @NuevoEstado NOT IN ('pendiente', 'lavando', 'terminado', 'entregado')
    BEGIN
        RAISERROR('Estado no válido.', 16, 1);
        RETURN;
    END
    UPDATE ORDENES SET Estado = @NuevoEstado WHERE IdOrden = @IdOrden;
END
GO

CREATE OR ALTER PROC sp_ListarOrdenesPendientes AS
BEGIN
    SET NOCOUNT ON;
    SELECT o.IdOrden, o.FechaCreacion, o.Estado, o.InstruccionesEspeciales,
           c.Nombre + ' ' + c.Apellido_Paterno AS Cliente,
           v.Placa,
           ts.Nombre AS TipoServicio,
           e.Nombre + ' ' + e.Apellido_Paterno AS EmpleadoAsignado
    FROM ORDENES o
    INNER JOIN CLIENTES c ON o.IdCliente = c.IdCliente
    INNER JOIN VEHICULOS v ON o.IdVehiculo = v.IdVehiculo
    INNER JOIN TIPOS_SERVICIO ts ON o.IdTipoServicio = ts.IdTipoServicio
    LEFT JOIN EMPLEADOS e ON o.IdEmpleado = e.IdEmpleado
    WHERE o.Estado IN ('pendiente', 'lavando')
    ORDER BY o.FechaCreacion;
END
GO

CREATE OR ALTER PROC sp_ListarOrdenesPorEmpleado
    @IdEmpleado INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT o.IdOrden, o.FechaCreacion, o.Estado, o.InstruccionesEspeciales,
           c.Nombre + ' ' + c.Apellido_Paterno AS Cliente,
           v.Placa,
           ts.Nombre AS TipoServicio
    FROM ORDENES o
    INNER JOIN CLIENTES c ON o.IdCliente = c.IdCliente
    INNER JOIN VEHICULOS v ON o.IdVehiculo = v.IdVehiculo
    INNER JOIN TIPOS_SERVICIO ts ON o.IdTipoServicio = ts.IdTipoServicio
    WHERE o.IdEmpleado = @IdEmpleado
    ORDER BY o.FechaCreacion DESC;
END
GO

-- ============================================================
-- ASEGURAR LA COLUMNA IdServicio EN ORDENES
-- ============================================================
IF COL_LENGTH('ORDENES', 'IdServicio') IS NULL
BEGIN
    ALTER TABLE ORDENES ADD IdServicio INT NULL;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Ordenes_Servicios')
    AND COL_LENGTH('ORDENES', 'IdServicio') IS NOT NULL
BEGIN
    ALTER TABLE ORDENES
    ADD CONSTRAINT FK_Ordenes_Servicios
    FOREIGN KEY (IdServicio) REFERENCES SERVICIOS(IdServicio);
END
GO

-- ============================================================
-- SERVICIOS (facturación)
-- ============================================================
CREATE OR ALTER PROC sp_CrearServicioDesdeOrden
    @IdOrden INT,
    @Costo DECIMAL(10,2),
    @TipoPago VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdCliente INT, @IdVehiculo INT, @IdTipoServicio INT, @IdServicio INT;

    SELECT @IdCliente = IdCliente,
           @IdVehiculo = IdVehiculo,
           @IdTipoServicio = IdTipoServicio
    FROM ORDENES
    WHERE IdOrden = @IdOrden;

    IF @IdCliente IS NULL
    BEGIN
        RAISERROR('Orden no encontrada.', 16, 1);
        RETURN;
    END

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO SERVICIOS (IdCliente, IdVehiculo, IdTipoServicio, Costo)
        VALUES (@IdCliente, @IdVehiculo, @IdTipoServicio, @Costo);

        SET @IdServicio = SCOPE_IDENTITY();

        INSERT INTO CAJA (IdServicio, Precio, Tipo_Pago)
        VALUES (@IdServicio, @Costo, @TipoPago);

        UPDATE ORDENES
        SET IdServicio = @IdServicio,
            Estado = 'entregado'
        WHERE IdOrden = @IdOrden;

        COMMIT;
        SELECT @IdServicio AS IdServicio;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrMsg, 16, 1);
    END CATCH
END
GO

-- ============================================================
-- PAGOS / CAJA
-- ============================================================
CREATE OR ALTER PROC sp_RegistrarPago
    @IdServicio INT,
    @Monto DECIMAL(10,2),
    @TipoPago VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO CAJA (IdServicio, Precio, Tipo_Pago)
    VALUES (@IdServicio, @Monto, @TipoPago);
END
GO

CREATE OR ALTER PROC sp_ReporteCajaDiario
    @Fecha DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @Fecha IS NULL SET @Fecha = CAST(GETDATE() AS DATE);
    SELECT Tipo_Pago, SUM(Precio) AS Total
    FROM CAJA
    WHERE Fecha = @Fecha
    GROUP BY Tipo_Pago;
END
GO

-- ============================================================
-- ALMACEN / INVENTARIO
-- ============================================================
CREATE OR ALTER PROC sp_ListarProductos AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdProducto, Nombre, Descripcion, Precio
    FROM ALMACEN
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROC sp_InsertarProducto
    @Nombre VARCHAR(50),
    @Descripcion VARCHAR(150) = NULL,
    @Precio DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ALMACEN (Nombre, Descripcion, Precio)
    VALUES (@Nombre, @Descripcion, @Precio);
    SELECT SCOPE_IDENTITY() AS NuevoId;
END
GO

CREATE OR ALTER PROC sp_ActualizarProducto
    @IdProducto INT,
    @Nombre VARCHAR(50),
    @Descripcion VARCHAR(150) = NULL,
    @Precio DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ALMACEN SET Nombre = @Nombre, Descripcion = @Descripcion, Precio = @Precio
    WHERE IdProducto = @IdProducto;
END
GO

CREATE OR ALTER PROC sp_ActualizarInventario
    @IdProducto INT,
    @Cantidad INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM INVENTARIO WHERE IdProducto = @IdProducto)
        INSERT INTO INVENTARIO (IdProducto, Cantidad, CostoUnitario)
        VALUES (@IdProducto, @Cantidad, 0);
    ELSE
        UPDATE INVENTARIO SET Cantidad = Cantidad + @Cantidad
        WHERE IdProducto = @IdProducto;
END
GO

CREATE OR ALTER PROC sp_ObtenerInventario AS
BEGIN
    SET NOCOUNT ON;
    SELECT a.IdProducto, a.Nombre, a.Descripcion,
           i.Cantidad, i.CostoUnitario
    FROM ALMACEN a
    LEFT JOIN INVENTARIO i ON a.IdProducto = i.IdProducto
    ORDER BY a.Nombre;
END
GO

-- ============================================================
-- PROVEEDORES
-- ============================================================
CREATE OR ALTER PROC sp_ListarProveedores AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdProveedor, Nombre, Direccion, Telefono, Email
    FROM PROVEEDORES
    ORDER BY Nombre;
END
GO

CREATE OR ALTER PROC sp_InsertarProveedor
    @Nombre VARCHAR(50),
    @Direccion VARCHAR(100) = NULL,
    @Telefono VARCHAR(15) = NULL,
    @Email VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO PROVEEDORES (Nombre, Direccion, Telefono, Email)
    VALUES (@Nombre, @Direccion, @Telefono, @Email);
    SELECT SCOPE_IDENTITY() AS NuevoId;
END
GO

CREATE OR ALTER PROC sp_ActualizarProveedor
    @IdProveedor INT,
    @Nombre VARCHAR(50),
    @Direccion VARCHAR(100) = NULL,
    @Telefono VARCHAR(15) = NULL,
    @Email VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE PROVEEDORES SET
        Nombre = @Nombre,
        Direccion = @Direccion,
        Telefono = @Telefono,
        Email = @Email
    WHERE IdProveedor = @IdProveedor;
END
GO

------------------------------------------------------------------
-------------------------Empleado Procedure-----------------------
------------------------------------------------------------------
USE CarWashDB;
GO

CREATE OR ALTER PROC sp_LeerEmpleados AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdEmpleado, Nombre, Apellido_Paterno, Apellido_Materno, Direccion, Telefono, Sueldo, Turno, Cargo, FechaIngreso, Activo
    FROM EMPLEADOS
    WHERE Activo = 1
    ORDER BY Apellido_Paterno, Apellido_Materno;
END
GO