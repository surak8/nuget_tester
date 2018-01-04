@echo off
setlocal
set DLL=%~dp0test.dll
set IN_SPEC=test.nuspec
set OUT_SPEC=blah.nuspec
set EXEC1=%~dp0bin\debug\xt.exe

if not exist "%DLL%" (
echo BLAH
set CMD=csc -nologo %~dp0stuff\test.cs -out:"%DLL%" -t:library -D:DEBUG -debug:full
echo.
echo CMD=%CMD%
call %CMD%
if errorlevel 1 (
echo.
echo error building %DLL%!
exit /b 1
)
)

if not exist "%IN_SPEC%" (
set CMD=nuget spec -force -a "%DLL%"
echo %CMD%
call %CMD%
if errorlevel 1 (
echo.
echo error creating spec-file :  %IN_SPEC%!
exit /b 1
)
)

if not exist "%EXEC1%" (
echo NEED XT
set CMD=msbuild -nologo
echo %CMD%
call %CMD%
if errorlevel 1 (
echo.
echo error creating executable: %EXEC1%
exit /b 1
)
)

set CMD=%EXEC1% -v -x fixup.xslt %IN_SPEC% -a vOwners=rik  -o %OUT_SPEC%
echo %CMD%
call %CMD%
if errorlevel 1 (
echo.
echo transform-error!
exit /b 1
)

nuget pack %OUT_SPEC% -verbosity detailed 
