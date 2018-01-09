msbuild -nologo -v:n makeit.csproj -noconlog -fl -flp:logfile=test.log;encoding=ascii;v=diag 
nuget pack C:\Users\Owner\AppData\Local\Temp\test.nuspec -verbosity detailed 
nuget push  C:\Users\Owner\source\nuget\stuff\test.1.0.3.nupkg -verbosity detailed 
