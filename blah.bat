setlocal
set NAME=test
set DEST=%~dp0lib
set DLL=%DEST%\%NAME%.dll 

if not exist "%DEST%" md "%DEST%"
csc -nologo %NAME%.cs -out:%DLL% 
nuget spec -force -a %DLL% -verbosity detailed
REM REM nuget pack  -verbosity detailed %NAME%.nuspec 	
