@echo off
setlocal
set DLL=test.dll
set SPEC=blah.nuspec
set CMD=csc -nologo stuff\test.cs -t:library -d:debug -out:%DLL%
echo %CMD%
call %CMD%
if errorlevel 1 goto :Error

set CMD=bin\debug\xt -v -x transform\DefaultPackage.xslt test.xml -a idValue=zzz;descValue=desc;authorsValue=a;versionValue=1.2.3.4 -o %SPEC%
echo %CMD%
call %CMD%
if errorlevel 1 goto :Error

set CMD=nuget pack %SPEC% -verbosity d 
echo %CMD%
call %CMD%
if errorlevel 1 goto :Error

exit /b 0
:error
echo.
echo error here
exit /b 1