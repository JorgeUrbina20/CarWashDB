-- ============================================================
-- BASE DE DATOS: CarWashDB 
-- ============================================================
CREATE DATABASE CarWashDB;
GO
USE CarWashDB;
GO

-- 1. ROLES
CREATE TABLE ROLES (
    IdRol INT PRIMARY KEY IDENTITY(1,1),
    NombreRol VARCHAR(50) NOT NULL UNIQUE,
    Descripcion VARCHAR(150),
    Estado BIT DEFAULT 1
);

-- 2. EMPLEADOS
CREATE TABLE EMPLEADOS (
    IdEmpleado INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Apellido_Paterno VARCHAR(50) NOT NULL,
    Apellido_Materno VARCHAR(50) NULL,
    Direccion VARCHAR(100),
    Telefono VARCHAR(15),
    Sueldo DECIMAL(10,2),
    Turno VARCHAR(20),
    Cargo VARCHAR(50) NOT NULL,
    FechaIngreso DATE DEFAULT GETDATE(),
    Activo BIT DEFAULT 1
);

-- 3. USUARIOS
CREATE TABLE USUARIOS (
    IdUsuario INT PRIMARY KEY IDENTITY(1,1),
    NombreUsuario VARCHAR(50) NOT NULL UNIQUE,
    Contrasena VARCHAR(255) NOT NULL,
    IdRol INT NOT NULL,
    IdEmpleado INT NULL UNIQUE,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1,
    FOREIGN KEY (IdRol) REFERENCES ROLES(IdRol),
    FOREIGN KEY (IdEmpleado) REFERENCES EMPLEADOS(IdEmpleado)
);

-- 4. CARWASH (SUCURSAL)
CREATE TABLE CARWASH (
    IdCarwash INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Direccion VARCHAR(100),
    Email VARCHAR(100),
    Telefono VARCHAR(15),
    Activo BIT DEFAULT 1
);

-- 5. CARWASH_HAS_EMPLEADOS
CREATE TABLE CARWASH_HAS_EMPLEADOS (
    IdCarwash INT NOT NULL,
    IdEmpleado INT NOT NULL,
    PRIMARY KEY (IdCarwash, IdEmpleado),
    FOREIGN KEY (IdCarwash) REFERENCES CARWASH(IdCarwash),
    FOREIGN KEY (IdEmpleado) REFERENCES EMPLEADOS(IdEmpleado)
);

-- 6. CLIENTES
CREATE TABLE CLIENTES (
    IdCliente INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Apellido_Paterno VARCHAR(50),
    Apellido_Materno VARCHAR(50),
    Direccion VARCHAR(100),
    Telefono VARCHAR(15),
    Email VARCHAR(100),
    Activo BIT DEFAULT 1
);

-- 7. MARCAS
CREATE TABLE MARCAS (
    IdMarca INT PRIMARY KEY IDENTITY(1,1),
    NombreMarca VARCHAR(50) NOT NULL UNIQUE
);

-- 8. MODELOS
CREATE TABLE MODELOS (
    IdModelo INT PRIMARY KEY IDENTITY(1,1),
    NombreModelo VARCHAR(50) NOT NULL,
    IdMarca INT NOT NULL,
    FOREIGN KEY (IdMarca) REFERENCES MARCAS(IdMarca)
);

-- 9. VEHÍCULOS
CREATE TABLE VEHICULOS (
    IdVehiculo INT PRIMARY KEY IDENTITY(1,1),
    IdCliente INT NOT NULL,
    IdModelo INT NOT NULL,
    Tipo VARCHAR(30),
    Placa VARCHAR(20) UNIQUE NOT NULL,
    FOREIGN KEY (IdCliente) REFERENCES CLIENTES(IdCliente),
    FOREIGN KEY (IdModelo) REFERENCES MODELOS(IdModelo)
);

-- 10. TIPOS DE SERVICIO
CREATE TABLE TIPOS_SERVICIO (
    IdTipoServicio INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(150),
    PrecioBase DECIMAL(10,2) NOT NULL
);

-- 11. SERVICIOS (sin delivery)
CREATE TABLE SERVICIOS (
    IdServicio INT PRIMARY KEY IDENTITY(1,1),
    IdCliente INT NOT NULL,
    IdVehiculo INT NULL,
    IdTipoServicio INT NOT NULL,
    Costo DECIMAL(10,2) NOT NULL,
    Fecha DATE DEFAULT GETDATE(),
    FOREIGN KEY (IdCliente) REFERENCES CLIENTES(IdCliente),
    FOREIGN KEY (IdVehiculo) REFERENCES VEHICULOS(IdVehiculo),
    FOREIGN KEY (IdTipoServicio) REFERENCES TIPOS_SERVICIO(IdTipoServicio)
);

-- 12. CAJA (PAGOS)
CREATE TABLE CAJA (
    IdCaja INT PRIMARY KEY IDENTITY(1,1),
    IdServicio INT NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Tipo_Pago VARCHAR(50),
    Fecha DATE DEFAULT GETDATE(),
    FOREIGN KEY (IdServicio) REFERENCES SERVICIOS(IdServicio)
);

-- 13. ÓRDENES DE TRABAJO
CREATE TABLE ORDENES (
    IdOrden INT PRIMARY KEY IDENTITY(1,1),
    IdCliente INT NOT NULL,
    IdVehiculo INT NOT NULL,
    IdTipoServicio INT NOT NULL,
    IdEmpleado INT NULL,
    IdServicio INT NULL,                 -- se asigna cuando se factura
    Estado VARCHAR(20) DEFAULT 'pendiente',
    FechaCreacion DATETIME DEFAULT GETDATE(),
    InstruccionesEspeciales VARCHAR(200),
    FOREIGN KEY (IdCliente) REFERENCES CLIENTES(IdCliente),
    FOREIGN KEY (IdVehiculo) REFERENCES VEHICULOS(IdVehiculo),
    FOREIGN KEY (IdTipoServicio) REFERENCES TIPOS_SERVICIO(IdTipoServicio),
    FOREIGN KEY (IdEmpleado) REFERENCES EMPLEADOS(IdEmpleado),
    FOREIGN KEY (IdServicio) REFERENCES SERVICIOS(IdServicio)
);

-- 14. ALMACEN (PRODUCTOS)
CREATE TABLE ALMACEN (
    IdProducto INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(150),
    Precio DECIMAL(10,2)
);

-- 15. INVENTARIO
CREATE TABLE INVENTARIO (
    IdInventario INT PRIMARY KEY IDENTITY(1,1),
    IdProducto INT NOT NULL,
    Cantidad INT NOT NULL,
    CostoUnitario DECIMAL(10,2),
    FOREIGN KEY (IdProducto) REFERENCES ALMACEN(IdProducto)
);

-- 16. PROVEEDORES
CREATE TABLE PROVEEDORES (
    IdProveedor INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL,
    Direccion VARCHAR(100),
    Telefono VARCHAR(15),
    Email VARCHAR(100)
);

-- 17. PROVEEDORES_HAS_INVENTARIO
CREATE TABLE PROVEEDORES_HAS_INVENTARIO (
    IdProveedor INT NOT NULL,
    IdInventario INT NOT NULL,
    PRIMARY KEY (IdProveedor, IdInventario),
    FOREIGN KEY (IdProveedor) REFERENCES PROVEEDORES(IdProveedor),
    FOREIGN KEY (IdInventario) REFERENCES INVENTARIO(IdInventario)
);

-- 18. AUDITORÍA
CREATE TABLE Auditoria (
    IdAuditoria INT PRIMARY KEY IDENTITY(1,1),
    TablaAfectada VARCHAR(50),
    Operacion VARCHAR(10),
    IdRegistro INT,
    DatosAnteriores NVARCHAR(MAX),
    DatosNuevos NVARCHAR(MAX),
    Usuario VARCHAR(50),
    Fecha DATETIME DEFAULT GETDATE()
);
GO