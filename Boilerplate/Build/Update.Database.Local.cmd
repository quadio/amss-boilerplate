@echo off
c:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild ..\Source\Amss.Boilerplate.Migrations\Amss.Boilerplate.Migrations.csproj /nologo /v:m
        if %ERRORLEVEL% NEQ 0 (
            goto:end
        )

PUSHD ..\Source\Amss.Boilerplate.Migrations\bin\
    ECM7.Migrator.Console SqlServer "Data Source=(local);Initial Catalog=boilerplatedb;Integrated Security=True" Amss.Boilerplate.Migrations.dll
POPD
:end