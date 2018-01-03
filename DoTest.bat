bin\debug\xt -v -x fixup.xslt test.nuspec -a vOwners=blah -o zzz.nuspec 
nuget pack zzz.nuspec -verbosity detailed 
