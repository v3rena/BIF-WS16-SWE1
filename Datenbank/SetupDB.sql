USE Master

IF EXISTS(SELECT * FROM sysdatabases WHERE name = 'TempData')
  DROP DATABASE TempData
GO

---------------------------------------------------------------------------------------------------
--Dateipfad entspricht MSSQL12 Express Edition, bei Verwendung anderer Edition entsprechend aendern
---------------------------------------------------------------------------------------------------
CREATE DATABASE TempData

ON PRIMARY (
  Name = 'TempData_DATA',
  Filename='C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\TempData_DATA.mdf',
  --Filename='E:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\TempData_DATA.mdf',
  Size = 10MB,
  Filegrowth = 10%
)
LOG ON (
  Name = 'TempData_LOG',
  Filename='C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\TempData_LOG.ldf',
  --Filename='E:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\TempData_LOG.ldf',
  Size = 10MB,
  Filegrowth = 10%
)
