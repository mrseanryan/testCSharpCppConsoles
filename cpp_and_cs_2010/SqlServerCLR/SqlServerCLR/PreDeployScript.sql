print 'enabling clr'

exec sp_configure 'clr enabled', 1
go
reconfigure
go

