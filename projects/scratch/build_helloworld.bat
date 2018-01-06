@echo off
REM https://msdn.microsoft.com/en-us/library/dd576348.aspx
setlocal
msbuild helloworld.csproj /t:Build
