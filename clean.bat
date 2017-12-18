setlocal
set NAME=test
rd /q/s lib
del /q/s lib%NAME%.dll  lib%NAME%.nuspec *.nupkg %NAME%.dll %NAME%.nuspec
