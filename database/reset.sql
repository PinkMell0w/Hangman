USE master;
GO

IF EXISTS (
    SELECT name FROM sys.databases
    WHERE name = N'hangmanDB'
)
BEGIN
    ALTER DATABASE hangmanDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE hangmanDB;
END
GO

CREATE DATABASE hangmanDB;
GO

-- Recreate the user mapping after DB is dropped and recreated
USE hangmanDB;
GO

CREATE USER hangmanAdmin FOR LOGIN hangmanAdmin;
GO

ALTER ROLE db_datareader ADD MEMBER hangmanAdmin;
ALTER ROLE db_datawriter ADD MEMBER hangmanAdmin;
GO

GRANT CREATE TABLE TO hangmanAdmin;
GRANT ALTER        TO hangmanAdmin;
GRANT REFERENCES   TO hangmanAdmin;
GO

PRINT 'Database reset complete.';
GO
