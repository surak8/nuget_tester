@echo off
setlocal
set SPEC=default.nuspec
set CMD=xt -O -v -f tester.dll -x transform\DefaultPackage.xslt xt.xml -o %SPEC% -a idValue=riktest
echo %CMD%
call %CMD%
if errorlevel 1 goto :Error

set CMD=nuget pack %SPEC% -verbosity detailed   
echo %CMD%
call %CMD%
if errorlevel 1 goto :Error

exit /b 0
:Error
echo.
echo error occurred
exit /b 1