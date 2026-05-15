-- ============================================================
-- SCRIPT DE SEGURIDAD: Logins, Usuarios, Roles y Permisos
-- Base de datos: CarWashDB
-- ============================================================
USE CarWashDB;
GO

-- ============================================================
-- 1. LOGINS A NIVEL DE SERVIDOR
-- ============================================================
-- Nota: Las contraseñas deben cumplir la política de seguridad del servidor.
--       Cambiar las contraseñas en producción inmediatamente.
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'UserMaster')
    CREATE LOGIN UserMaster WITH PASSWORD = 'Own3r_S3gur0#2026!';
    
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'EmpleadoL')
    CREATE LOGIN EmpleadoL WITH PASSWORD = 'Emp_S3gur0#2026!';
    
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'InvitadoL')
    CREATE LOGIN InvitadoL WITH PASSWORD = 'Invitado_S3gur0#2026!';

-- ============================================================
-- 2. USUARIOS DE BASE DE DATOS
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'AdminMaster')
    CREATE USER AdminMaster FOR LOGIN UserMaster;

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'EmpleadoUser')
    CREATE USER EmpleadoUser FOR LOGIN EmpleadoL;

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'InvitadoU')
    CREATE USER InvitadoU FOR LOGIN InvitadoL;

-- ============================================================
-- 3. ROLES DE BASE DE DATOS
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'Rol_Owner' AND type = 'R')
    CREATE ROLE Rol_Owner;

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'Rol_Empleado' AND type = 'R')
    CREATE ROLE Rol_Empleado;

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'Rol_Invitado' AND type = 'R')
    CREATE ROLE Rol_Invitado;

-- Asignar usuarios a los roles
ALTER ROLE Rol_Owner ADD MEMBER AdminMaster;
ALTER ROLE Rol_Empleado ADD MEMBER EmpleadoUser;
ALTER ROLE Rol_Invitado ADD MEMBER InvitadoU;

-- ============================================================
-- 4. PERMISOS PARA ROL_OWNER (Control total)
-- ============================================================
ALTER ROLE db_owner ADD MEMBER Rol_Owner;

-- ============================================================
-- 5. PERMISOS PARA ROL_EMPLEADO
--    (Recepcionista/Cajero + Lavador)
-- ============================================================
-- Clientes y vehículos
GRANT SELECT, INSERT, UPDATE ON CLIENTES TO Rol_Empleado;
GRANT SELECT, INSERT, UPDATE ON VEHICULOS TO Rol_Empleado;
GRANT SELECT ON MARCAS TO Rol_Empleado;
GRANT SELECT ON MODELOS TO Rol_Empleado;

-- Servicios y pagos
GRANT SELECT ON TIPOS_SERVICIO TO Rol_Empleado;
GRANT SELECT, INSERT, UPDATE ON ORDENES TO Rol_Empleado;
GRANT SELECT, INSERT ON SERVICIOS TO Rol_Empleado;
GRANT SELECT, INSERT ON CAJA TO Rol_Empleado;

-- Inventario y proveedores (solo lectura)
GRANT SELECT ON ALMACEN TO Rol_Empleado;
GRANT SELECT ON INVENTARIO TO Rol_Empleado;
GRANT SELECT ON PROVEEDORES TO Rol_Empleado;
GRANT SELECT ON PROVEEDORES_HAS_INVENTARIO TO Rol_Empleado;

-- ============================================================
-- 6. PERMISOS PARA ROL_INVITADO
-- ============================================================
GRANT SELECT ON dbo.Vista_Servicios_Hoy TO Rol_Invitado;
GRANT SELECT ON dbo.Vista_Vehiculos_Por_Cliente TO Rol_Invitado;
GO