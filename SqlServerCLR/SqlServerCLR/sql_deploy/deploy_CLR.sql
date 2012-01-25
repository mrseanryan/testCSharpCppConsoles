use licensing_dev



CREATE ASSEMBLY uspSplitString from 'C:\sourceroot\local\SqlServerCLR\SqlServerCLR\bin\Release\SqlServerCLR.dll' --WITH PERMISSION_SET = SAFE
--with execute as 'sryan'
GO

CREATE PROCEDURE uspSplitString
AS
EXTERNAL NAME SqlServerCLR.uspSplitString

GO

exec uspSplitString 'test ;string',  ';'
