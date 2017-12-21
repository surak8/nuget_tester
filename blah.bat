setlocal
set NAME=test
set DEST=%~dp0lib\net4.5
set DLL=%DEST%\%NAME%.dll 

if not exist "%DEST%" md "%DEST%"
csc -nologo %NAME%.cs -out:%DLL% 
nuget spec -force -a %DLL% -verbosity detailed
call test_driver -d -description "desc" -releasenotes "rnotes" test.nuspec
if errorlevel 1 goto :UGH
REM REM nuget pack  -verbosity detailed %NAME%.nuspec 	
 nuget pack  -verbosity detailed  test.nuspec -properties owners=janedoe,harikm,kimo,xiaop;desc="Awesome app logger utility";tags=riktest 
REM nuget push test.1.0.2.0.nupkg -source "\\appdeploy\appdeploy\Colt Software\NUGET"  -apikey 3cf63582-b815-4cd4-82ec-4b269fbc614b 
nuget push test.1.0.2.nupkg
exit /b 0
:UGH
echo.
echo error occurred
exit /b 1
