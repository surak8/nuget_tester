setlocal
set NAME=test
set NAME2=tester
set NAME3=test_driver
set DF1=%NAME%.fix.nuspec
set DF2=%NAME2%.dll %NAME2%.pdb %NAME2%.xml
set DF3=%NAME3%.exe %NAME3%.exe.config %NAME3%.pdb %NAME3%.xml
set DEL_FILES=*.nuspec %DF1% %DF2% %DF3%
rd /q/s lib bin obj 
del /q/s %DEL_FILES%
