pushd "%~dp0"
start "" "%~dp0BuildCopy.exe" -l -o -s "%~dp0lib" -d "%~dp0bin\Debug" %1
popd