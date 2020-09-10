sqlcmd -S .\sqlexpress -Q "EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'Test'"
sqlcmd -S .\sqlexpress -Q "ALTER DATABASE [Test] SET SINGLE_USER WITH ROLLBACK IMMEDIATE"
sqlcmd -S .\sqlexpress -Q "DROP DATABASE [Test]"
sqlcmd -S .\sqlexpress -Q "CREATE DATABASE [Test]"
