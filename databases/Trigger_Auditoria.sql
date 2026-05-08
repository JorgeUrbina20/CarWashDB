USE CarWashDB;
GO

CREATE TRIGGER trg_AuditarEmpleado
ON EMPLEADOS
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Operacion VARCHAR(10);
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
        SET @Operacion = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted)
        SET @Operacion = 'INSERT';
    ELSE
        SET @Operacion = 'DELETE';

    INSERT INTO Auditoria (TablaAfectada, Operacion, IdRegistro, DatosAnteriores, DatosNuevos, Usuario)
    SELECT 'EMPLEADOS', @Operacion,
           ISNULL(i.IdEmpleado, d.IdEmpleado),
           (SELECT * FROM deleted d2 WHERE d2.IdEmpleado = d.IdEmpleado FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
           (SELECT * FROM inserted i2 WHERE i2.IdEmpleado = i.IdEmpleado FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
           SYSTEM_USER
    FROM inserted i
    FULL OUTER JOIN deleted d ON i.IdEmpleado = d.IdEmpleado;
END
GO