USE CarWashDB;
GO

-- ============================================================
-- 1. LOGINS (a nivel de servidor)
-- ============================================================
CREATE LOGIN UserMaster WITH PASSWORD = 'OwnerSeguro123!';
CREATE LOGIN EmpleadoL WITH PASSWORD = 'EmpSeguro123!';
CREATE LOGIN InvitadoL WITH PASSWORD = 'Invitado123!';

-- ============================================================
-- 2. USUARIOS DE BASE DE DATOS
-- ============================================================
CREATE USER AdminMaster FOR LOGIN UserMaster;
CREATE USER EmpleadoUser FOR LOGIN EmpleadoL;
CREATE USER InvitadoU FOR LOGIN InvitadoL;

-- ============================================================
-- 3. ROLES DE BASE DE DATOS
-- ============================================================
CREATE ROLE Rol_Owner;
CREATE ROLE Rol_Empleado;
CREATE ROLE Rol_Invitado;

-- Asignar usuarios a los roles
ALTER ROLE Rol_Owner ADD MEMBER AdminMaster;
ALTER ROLE Rol_Empleado ADD MEMBER EmpleadoUser;
ALTER ROLE Rol_Invitado ADD MEMBER InvitadoU;

-- ============================================================
-- 4. PERMISOS PARA ROL_OWNER
-- ============================================================
ALTER ROLE db_owner ADD MEMBER Rol_Owner;   -- control total

-- ============================================================
-- 5. PERMISOS PARA ROL_EMPLEADO (Recepcionista/Cajero + Lavador)
--    Como en el sistema un empleado puede hacer ambas funciones,
--    otorgamos permisos amplios sobre las tablas operativas.
-- ============================================================
-- Clientes y vehículos
GRANT SELECT, INSERT, UPDATE ON CLIENTES TO Rol_Empleado;
GRANT SELECT, INSERT, UPDATE ON VEHICULOS TO Rol_Empleado;
GRANT SELECT ON MARCAS TO Rol_Empleado;
GRANT SELECT ON MODELOS TO Rol_Empleado;

-- Servicios y pagos
GRANT SELECT ON TIPOS_SERVICIO TO Rol_Empleado;
GRANT SELECT, INSERT ON ORDENES TO Rol_Empleado;
GRANT SELECT, INSERT, UPDATE ON SERVICIOS TO Rol_Empleado;
GRANT SELECT, INSERT ON CAJA TO Rol_Empleado;
GRANT SELECT ON SERVICIO_DELIVERY TO Rol_Empleado;

-- Inventario y proveedores (solo lectura para poder ver productos)
GRANT SELECT ON ALMACEN TO Rol_Empleado;
GRANT SELECT ON INVENTARIO TO Rol_Empleado;
GRANT SELECT ON PROVEEDORES TO Rol_Empleado;
GRANT SELECT ON PROVEEDORES_HAS_INVENTARIO TO Rol_Empleado;

-- No se permite modificar empleados ni roles.

-- ============================================================
-- 6. PERMISOS PARA ROL_INVITADO
-- ============================================================
GRANT SELECT ON dbo.Vista_Servicios_Hoy TO Rol_Invitado;
GRANT SELECT ON dbo.Vista_Vehiculos_Por_Cliente TO Rol_Invitado;
GO