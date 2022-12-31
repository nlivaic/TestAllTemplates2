USE master
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TestAllPipelines2Db')
BEGIN
  CREATE DATABASE TestAllPipelines2Db;
END;
GO

USE TestAllPipelines2Db;
GO

IF NOT EXISTS (SELECT 1
                 FROM sys.server_principals
                WHERE [name] = N'TestAllPipelines2Db_Login' 
                  AND [type] IN ('C','E', 'G', 'K', 'S', 'U'))
BEGIN
    CREATE LOGIN TestAllPipelines2Db_Login
        WITH PASSWORD = '<DB_PASSWORD>';
END;
GO  

IF NOT EXISTS (select * from sys.database_principals where name = 'TestAllPipelines2Db_User')
BEGIN
    CREATE USER TestAllPipelines2Db_User FOR LOGIN TestAllPipelines2Db_Login;
END;
GO  


EXEC sp_addrolemember N'db_datareader', N'TestAllPipelines2Db_User';
GO

EXEC sp_addrolemember N'db_datawriter', N'TestAllPipelines2Db_User';
GO

EXEC sp_addrolemember N'db_ddladmin', N'TestAllPipelines2Db_User';
GO
