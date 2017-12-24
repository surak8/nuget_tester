@echo off
setlocal
set args=-properties owners=janedoe,harikm,kimo,xiaop;desc="Awesome app logger utility";tags=riktest 
if {%1x}=={x} goto :NoArgs
set SPEC=%1
set CMD=nuget pack  -verbosity detailed  %1 %ARGS%
echo %CMD%
call %CMD%
if errorlevel 1 goto :Error
exit /b 0
:Error
echo.
echo error occurred.
exit /b 1
