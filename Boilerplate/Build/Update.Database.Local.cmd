@echo off
c:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild ..\Source\Amss.Boilerplate.Migrations\Amss.Boilerplate.Migrations.csproj /nologo /v:m
        if %ERRORLEVEL% NEQ 0 (
            goto:end
        )

PUSHD ..\Source\Amss.Boilerplate.Migrations\bin\
    Migrate -db SqlServer2008 -conn "Data Source=(local);Initial Catalog=boilerplatedb;Integrated Security=True" -a Amss.Boilerplate.Migrations.dll
POPD
:end