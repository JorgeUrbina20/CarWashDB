-- ============================================================
-- TRIGGER: trg_AuditarEmpleado
-- Descripción: Registra en la tabla Auditoria cualquier operación
--              (INSERT, UPDATE, DELETE) realizada sobre EMPLEADOS.
-- ============================================================
USE CarWashDB;
GO

CREATE OR ALTER TRIGGER trg_AuditarEmpleado
ON EMPLEADOS
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Determinar el tipo de operación
    DECLARE @Operacion VARCHAR(10) = 
        CASE 
            WHEN EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted) THEN 'UPDATE'
            WHEN EXISTS (SELECT 1 FROM inserted) THEN 'INSERT'
            ELSE 'DELETE'
        END;

    -- Insertar el registro de auditoría
    INSERT INTO Auditoria (TablaAfectada, Operacion, IdRegistro, DatosAnteriores, DatosNuevos, Usuario)
    SELECT 
        'EMPLEADOS'                     AS TablaAfectada,
        @Operacion                      AS Operacion,
        ISNULL(i.IdEmpleado, d.IdEmpleado) AS IdRegistro,
        (SELECT d2.* FROM deleted d2 WHERE d2.IdEmpleado = d.IdEmpleado FOR JSON PATH, WITHOUT_ARRAY_WRAPPER) AS DatosAnteriores,
        (SELECT i2.* FROM inserted i2 WHERE i2.IdEmpleado = i.IdEmpleado FOR JSON PATH, WITHOUT_ARRAY_WRAPPER) AS DatosNuevos,
        SYSTEM_USER                     AS Usuario
    FROM inserted i
    FULL OUTER JOIN deleted d ON i.IdEmpleado = d.IdEmpleado;
END
GO