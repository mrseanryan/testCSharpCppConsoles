use licensing_dev

-- in case you need to manually deploy the assembly, then try this SQL:

/*
note:
I got this project to debug + deploy from within Visual Studio (even debugging the SQL + the C# within the db !)
by doing the following:

1. run VS2010 as Administrator
2. VS2010 was on same box as database
3. database login was db administrator account (sa)
4. just hit F5 to deploy + debug

*/

CREATE ASSEMBLY uspSplitString from 'C:\sourceroot\local\SqlServerCLR\SqlServerCLR\bin\Release\SqlServerCLR.dll' --WITH PERMISSION_SET = SAFE
--with execute as 'sryan'
GO

CREATE PROCEDURE uspSplitString
AS
EXTERNAL NAME SqlServerCLR.uspSplitString

GO
