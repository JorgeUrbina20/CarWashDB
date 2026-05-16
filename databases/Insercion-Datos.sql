USE CarWashDB;
GO

-- ============================================================
-- 1. ROLES
-- ============================================================
INSERT INTO ROLES (NombreRol, Descripcion) VALUES 
('Owner', 'Dueño, acceso total al sistema'),
('RecepcionistaCajero', 'Atención al cliente, creación de órdenes y cobros'),
('Empleado', 'Lavador, solo ve y actualiza sus trabajos asignados');

-- ============================================================
-- 2. EMPLEADOS (incluye Owner y demás puestos)
-- ============================================================
INSERT INTO EMPLEADOS (Nombre, Apellido_Paterno, Apellido_Materno, Direccion, Telefono, Sueldo, Turno, Cargo) VALUES
('Admin', 'Sistema', 'Principal', 'Oficina Central', '555-0000', 0, 'Completo', 'Owner'),
('Laura', 'Ramírez', 'López', 'Calle 10 #45', '555-1001', 25000, 'Matutino', 'Recepcionista'),
('José', 'Martínez', NULL, 'Av. Siempre Viva 742', '555-1002', 25000, 'Vespertino', 'Recepcionista'),
('Miguel', 'García', 'Ortiz', 'Calle 20 #30', '555-2001', 15000, 'Matutino', 'Lavador'),
('Ana', 'Sánchez', 'Pérez', 'Calle 21 #31', '555-2002', 15000, 'Matutino', 'Lavador'),
('Luis', 'Fernández', 'Díaz', 'Calle 22 #32', '555-2003', 15000, 'Vespertino', 'Lavador'),
('Carmen', 'López', 'Ruiz', 'Calle 23 #33', '555-2004', 15000, 'Nocturno', 'Lavador');

-- ============================================================
-- 3. USUARIOS (contraseñas temporales, el backend aplicará bcrypt)
-- ============================================================
-- IMPORTANTE: La contraseña 'Admin123!' será reemplazada por el backend con bcrypt.
-- Se usa el hash de ejemplo SOLO para que el login funcione con 'Admin123!'.
-- Los demás usuarios usarán 'temporal' y deberán cambiarse en el primer inicio.
INSERT INTO USUARIOS (NombreUsuario, Contrasena, IdRol, IdEmpleado) VALUES
('admin', '$2a$12$LJ3m4yG5Z6v6Qe9F8v0wFeK1bG3c0hJpXnR0oT7uWsUfYzBvI8m2W', 1, 1), -- hash de 'Admin123!'
('laura.r', 'temporal', 2, 2),
('jose.m', 'temporal', 2, 3),
('miguel.g', 'temporal', 3, 4),
('ana.s', 'temporal', 3, 5),
('luis.f', 'temporal', 3, 6);

UPDATE USUARIOS SET Contrasena = 'temporal' WHERE NombreUsuario = 'admin';
select * from USUARIOS;

-- ============================================================
-- 4. SUCURSALES (CARWASH)
-- ============================================================
INSERT INTO CARWASH (Nombre, Direccion, Email, Telefono) VALUES
('CarWash Express Central', 'Av. Principal 123', 'central@carwash.com', '555-3001'),
('CarWash Express Norte', 'Calle 50 #10-20', 'norte@carwash.com', '555-3002');

-- ============================================================
-- 5. ASIGNACIÓN EMPLEADOS → SUCURSALES
-- ============================================================
INSERT INTO CARWASH_HAS_EMPLEADOS (IdCarwash, IdEmpleado) VALUES
(1,1), (2,1),   -- Owner en ambas
(1,2), (2,3),   -- Recepcionistas: Laura en Central, José en Norte
(1,4), (1,5),   -- Lavadores: Miguel y Ana en Central
(2,6), (2,7);   -- Lavadores: Luis y Carmen en Norte

-- ============================================================
-- 6. CLIENTES
-- ============================================================
INSERT INTO CLIENTES (Nombre, Apellido_Paterno, Apellido_Materno, Direccion, Telefono, Email) VALUES
('Juan', 'Pérez', 'Gómez', 'Calle 1 #100', '555-4001', 'juan.perez@email.com'),
('María', 'López', 'Hernández', 'Carrera 2 #200', '555-4002', 'maria.lopez@email.com'),
('Carlos', 'Sánchez', 'Ramírez', 'Transversal 3 #300', '555-4003', 'carlos.sanchez@email.com'),
('Sofía', 'García', 'Martínez', 'Diagonal 4 #400', '555-4004', 'sofia.garcia@email.com'),
('Pedro', 'Rodríguez', 'Fernández', 'Avenida 5 #500', '555-4005', 'pedro.rodriguez@email.com');

-- ============================================================
-- 7. MARCAS DE VEHÍCULOS
-- ============================================================
INSERT INTO MARCAS (NombreMarca) VALUES 
('Toyota'), ('Honda'), ('Nissan'), ('Ford'), ('Chevrolet'), ('Hyundai'), ('Mazda'), ('Kia');

-- ============================================================
-- 8. MODELOS DE VEHÍCULOS
-- ============================================================
INSERT INTO MODELOS (NombreModelo, IdMarca) VALUES
('Corolla', 1), ('Camry', 1), ('Hilux', 1),
('Civic', 2), ('CR-V', 2), ('Accord', 2),
('Sentra', 3), ('Altima', 3), ('Kicks', 3),
('F-150', 4), ('Mustang', 4), ('Escape', 4),
('Onix', 5), ('Tracker', 5), ('Spin', 5),
('Elantra', 6), ('Tucson', 6), ('Santa Fe', 6),
('Mazda3', 7), ('CX-5', 7), ('MX-5', 7),
('Rio', 8), ('Sportage', 8), ('Sorento', 8);

-- ============================================================
-- 9. VEHÍCULOS DE CLIENTES
-- ============================================================
INSERT INTO VEHICULOS (IdCliente, IdModelo, Tipo, Placa) VALUES
(1, 1, 'Sedán', 'ABC-123'),     -- Juan -> Toyota Corolla
(1, 4, 'Sedán', 'ABC-124'),     -- Juan -> Honda Civic
(2, 10, 'Camioneta', 'DEF-456'),-- María -> Ford F-150
(3, 2, 'Sedán', 'GHI-789'),     -- Carlos -> Toyota Camry
(4, 16, 'Sedán', 'JKL-012'),    -- Sofía -> Hyundai Elantra
(5, 19, 'Sedán', 'MNO-345');    -- Pedro -> Mazda3

-- ============================================================
-- 10. TIPOS DE SERVICIO
-- ============================================================
INSERT INTO TIPOS_SERVICIO (Nombre, Descripcion, PrecioBase) VALUES
('Lavado Exterior', 'Lavado de carrocería y llantas', 150.00),
('Lavado Completo', 'Exterior + interior (aspirado y tablero)', 200.00),
('Lavado y Encerado', 'Lavado completo más cera protectora', 250.00),
('Lavado Rápido', 'Solo enjuague y secado exterior', 100.00);

-- ============================================================
-- 11. ÓRDENES DE TRABAJO (flujo real)
-- ============================================================
INSERT INTO ORDENES (IdCliente, IdVehiculo, IdTipoServicio, IdEmpleado, Estado, InstruccionesEspeciales) VALUES
(1, 1, 1, 4, 'pendiente', 'Quitar manchas del capó'),           -- Corolla de Juan, Lavado Exterior, asignado a Miguel
(2, 3, 2, 5, 'pendiente', NULL),                                 -- F-150 de María, Lavado Completo, asignado a Ana
(3, 4, 3, NULL, 'pendiente', 'Cera especial en techo'),          -- Camry de Carlos, Lavado y Encerado, sin asignar aún
(4, 5, 1, 6, 'pendiente', NULL),                                 -- Elantra de Sofía, asignado a Luis (Norte)
(5, 6, 2, 7, 'pendiente', 'Interior con aroma a vainilla');     -- Mazda3 de Pedro, asignado a Carmen (Norte)

-- ============================================================
-- 12. SERVICIOS (facturación) y PAGOS (caja) con transacción
-- ============================================================
DECLARE @IdServicio INT;

-- Orden 1 completada y pagada
BEGIN TRY
    BEGIN TRANSACTION;
    
    INSERT INTO SERVICIOS (IdCliente, IdVehiculo, IdTipoServicio, Costo, Fecha) VALUES
    (1, 1, 1, 150.00, '2026-05-04');
    SET @IdServicio = SCOPE_IDENTITY();

    INSERT INTO CAJA (IdServicio, Precio, Tipo_Pago, Fecha) VALUES
    (@IdServicio, 150.00, 'Efectivo', '2026-05-04');

    UPDATE ORDENES SET IdServicio = @IdServicio, Estado = 'terminado' WHERE IdOrden = 1;
    
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;
END CATCH

-- Orden 2 completada y pagada
BEGIN TRY
    BEGIN TRANSACTION;
    
    INSERT INTO SERVICIOS (IdCliente, IdVehiculo, IdTipoServicio, Costo, Fecha) VALUES
    (2, 3, 2, 200.00, '2026-05-04');
    SET @IdServicio = SCOPE_IDENTITY();

    INSERT INTO CAJA (IdServicio, Precio, Tipo_Pago, Fecha) VALUES
    (@IdServicio, 200.00, 'Tarjeta', '2026-05-04');

    UPDATE ORDENES SET IdServicio = @IdServicio, Estado = 'terminado', IdEmpleado = 5 WHERE IdOrden = 2;
    
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;
END CATCH

-- ============================================================
-- 13. ALMACEN (productos)
-- ============================================================
INSERT INTO ALMACEN (Nombre, Descripcion, Precio) VALUES
('Shampoo para autos 5L', 'Líquido concentrado', 150.00),
('Cera líquida 1L', 'Cera protectora', 200.00),
('Toallas microfibra x10', 'Paquete de 10 unidades', 80.00),
('Aromatizante spray 500ml', 'Fragancia dulce', 60.00),
('Desengrasante 5L', 'Multiusos', 120.00),
('Guantes de lavado', 'Caja con 50 pares', 90.00),
('Esponja profesional', 'Esponja de doble cara', 35.00);

-- ============================================================
-- 14. INVENTARIO
-- ============================================================
INSERT INTO INVENTARIO (IdProducto, Cantidad, CostoUnitario) VALUES
(1, 20, 100.00),
(2, 15, 150.00),
(3, 50, 50.00),
(4, 30, 40.00),
(5, 25, 90.00),
(6, 40, 60.00),
(7, 60, 20.00);

-- ============================================================
-- 15. PROVEEDORES
-- ============================================================
INSERT INTO PROVEEDORES (Nombre, Direccion, Telefono, Email) VALUES
('Distribuidora LimpioMax', 'Calle Industria 1', '555-5001', 'ventas@limpiomax.com'),
('Químicos Profesionales S.A.', 'Av. Materias Primas 200', '555-5002', 'info@quimicospro.com'),
('Insumos y Más', 'Boulevard Comercio 300', '555-5003', 'contacto@insumosymas.com');

-- ============================================================
-- 16. PROVEEDORES_HAS_INVENTARIO
-- ============================================================
INSERT INTO PROVEEDORES_HAS_INVENTARIO (IdProveedor, IdInventario) VALUES
(1,1), (1,2), (1,3),
(2,4), (2,5), (2,6),
(3,7), (3,2), (3,5);
GO