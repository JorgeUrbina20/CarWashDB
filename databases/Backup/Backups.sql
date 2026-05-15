USE CarWashDB;
GO

/*
1 vez al día → Backup FULL

Cada 4-6 horas → Backup DIFERENCIAL

Cada 30 minutos → Backup de LOG
*/

-- ==========================================
-- BACKUP FULL DIARIO DE CarWashDB
-- ==========================================
BACKUP DATABASE CarWashDB 
TO DISK = 'C:\Program Files\Microsoft SQL Server\MSSQL17.MSSQLSERVER\MSSQL\Backup\CarwashDB_Full.bak'
WITH INIT, CHECKSUM;

PRINT 'Backup FULL completado.';


-- ==========================================
-- BACKUP DIFERENCIAL DE CarWashDB
-- ==========================================
BACKUP DATABASE CarWashDB 
TO DISK = 'C:\Program Files\Microsoft SQL Server\MSSQL17.MSSQLSERVER\MSSQL\Backup\CarWashDB_DIFF.bak'
WITH DIFFERENTIAL, INIT, CHECKSUM;

PRINT 'Backup DIFERENCIAL completado.';

-- ==========================================
-- BACKUP DE LOG DE CarWashDB
-- ==========================================
BACKUP LOG CarWashDB 
TO DISK = 'C:\Backups\CarWashDB\LOG\CarWashDB_LOG.trn'
WITH INIT, CHECKSUM;

PRINT 'Backup de LOG completado.';




--Si falla la base de datos restaurarla 

RESTORE DATABASE CarWashDB 
FROM DISK = 'C:\Backups\CarWashDB\FULL\CarWashDB_FULL.bak'
WITH NORECOVERY;

RESTORE DATABASE CarWashDB 
FROM DISK = 'C:\Backups\CarWashDB\DIFF\CarWashDB_DIFF.bak'
WITH NORECOVERY;

RESTORE LOG CarWashDB 
FROM DISK = 'C:\Backups\CarWashDB\LOG\CarWashDB_LOG.trn'
WITH RECOVERY;