USE [TempData]
GO


BEGIN


  declare @FromDate datetime2 = '2007-01-15 00:00:00.0000000'
  declare @ToDate datetime2 = '2017-01-15 00:00:00.0000000'
  declare @RandDate datetime2;

  declare @n numeric = 0;

  WHILE @n < 10000
  BEGIN

    SET @RandDate = dateadd( day,
                    RAND(checksum(newid()))*(1+datediff(day, @FromDate, @ToDate)),
                    @FromDate
                  );

    SET @RandDate = dateadd( hour,
                    (RAND(checksum(newid()))*24),
                    @RandDate
                  );

    INSERT INTO TempDaten (Zeitpunkt, Temperatur)
    VALUES (
              (
                @RandDate
              ),
              (
                FLOOR(
                        RAND(checksum(newid()))*(35-(-10))+(-10)
                      )
              )
            );
    SET @n = @n + 1;
  END

END
