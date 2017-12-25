@echo off
setlocal
set NAME=test
set DEST=%~dp0bin\debug
set DLL=%DEST%\%NAME%.dll 

if not exist "%DEST%" md "%DEST%"
csc -nologo -D:DEBUG -debug:full -t:library %NAME%.cs -out:%DLL% 
set CMD=nuget spec -force -a %DLL% -verbosity detailed
echo %CMD%
call %CMD%
if errorlevel 1 goto :UGH

set CMD=msbuild -nologo -p:configuration=debug -v:m test_driver.csproj
echo %CMD%
call %CMD%
if errorlevel 1 goto :UGH

set CMD=binx\debug\test_driver -d -description "desc" -releasenotes "rnotes" test.nuspec
echo %CMD%
call %CMD%
if errorlevel 1 goto :UGH

REM REM nuget pack  -verbosity detailed %NAME%.nuspec 	
REM nuget pack  -verbosity detailed  test.nuspec -properties owners=janedoe,harikm,kimo,xiaop;desc="Awesome app logger utility";tags=riktest 
set CMD=nuget pack  -verbosity detailed  test.fix.nuspec -properties owners=janedoe,harikm,kimo,xiaop;desc="Awesome app logger utility";tags=riktest 
echo %CMD%
call %CMD%
if errorlevel 1 goto :UGH

REM nuget push test.1.0.2.0.nupkg -source "\\appdeploy\appdeploy\Colt Software\NUGET"  -apikey 3cf63582-b815-4cd4-82ec-4b269fbc614b 
set CMD=nuget push test.1.0.3.nupkg
echo %CMD%
call %CMD%
if errorlevel 1 goto :UGH
exit /b 0
:UGH
echo.
echo error occurred
exit /b 1
