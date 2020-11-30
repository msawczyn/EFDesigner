use [master];
EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'Test'
ALTER DATABASE [Test] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [Test];
CREATE DATABASE [Test];
