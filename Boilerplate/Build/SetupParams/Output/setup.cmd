@echo off
copy /b/y NUL %WINDIR%\06CF2EB6-94E6-4a60-91D8-AB945AE8CF38 >NUL 2>&1
if errorlevel 1 goto:nonadmin
del %WINDIR%\06CF2EB6-94E6-4a60-91D8-AB945AE8CF38 >NUL 2>&1
:admin

set msdeploy="C:\Program Files (x86)\IIS\Microsoft Web Deploy\\msdeploy.exe"

set deploymentTransport=http://${DeploymentServer}/MSDeployAgentService
set packageDest=/M:%deploymentTransport%

if "${DeploymentServer}" EQU "local" (
    set packageDest=
)

set error=0
set errorMessage=

echo Setup Started...................................................

    echo        DB Install Started.......................................
    Database\Migrate -db SqlServer2008 -conn "${DatabaseConnection}" -a Database\Amss.Boilerplate.Migrations.dll
        if %ERRORLEVEL% NEQ 0 (
            set /A error=%error%+1
            set errorMessage=%errorMessage% Cannot update database structure.
        )
    echo        .........................................DB Install Ended

    echo        Site Install Started.....................................
    cmd /C Packages\Amss.Boilerplate.Web.deploy.cmd /Y %packageDest%
        if %ERRORLEVEL% NEQ 0 (
            set /A error=%error%+2
            set errorMessage=%errorMessage% Cannot install Amss.Boilerplate.Web.
        )
    cmd /C Packages\Amss.Boilerplate.Api.deploy.cmd /Y %packageDest%
        if %ERRORLEVEL% NEQ 0 (
            set /A error=%error%+4
            set errorMessage=%errorMessage% Cannot install Amss.Boilerplate.Api.
        )
    echo        .......................................Site Install Ended
    
echo Setup Ended...................................................

if %error% NEQ 0 (
    echo Setup Failure Reason is [%error%]:%errorMessage%
    exit /b %error%
)

goto:end
:nonadmin
    echo Setup Failure Reason is [16384]: You need administration privileges to run this script
    exit /b 16384
:end