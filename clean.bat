setlocal
set NAME=test
set NAME2=tester
rd /q/s lib bin obj *.nuspec
del /q/s lib%NAME%.dll  lib%NAME%.nuspec *.nupkg %NAME%.dll %NAME%.nuspec *.dll.config %NAME2%.nuspec
