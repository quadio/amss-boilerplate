@echo off
Migrate -db SqlServer2008 -conn "Data Source=(local);Initial Catalog=boilerplatedb;Integrated Security=True" -a Amss.Boilerplate.Migrations.dll