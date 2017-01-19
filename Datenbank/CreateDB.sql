USE [TempData]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TempDaten]') AND TYPE IN (N'U'))
DROP TABLE [dbo].[TempDaten]
GO

CREATE TABLE TempDaten
(
  ID numeric IDENTITY(1,1) PRIMARY KEY,
  Zeitpunkt datetime2 not null,
  Temperatur float not null,
);
