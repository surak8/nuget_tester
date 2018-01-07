@echo off
setlocal
csc -nologo ..\..\stuff\test.cs -t:library -out:test.dll 
msbuild -nologo NugetTask.csproj 
msbuild -nologo tester.csproj -v:diag 
