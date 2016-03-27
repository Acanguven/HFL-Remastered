@echo off
title Resetting ACLs…

setlocal

echo.
echo Determine whether we are on an 32 or 64 bit machine
echo.

if “%PROCESSOR_ARCHITECTURE%”==”x86″ if “%PROCESSOR_ARCHITEW6432%”==”” goto x86

set ProgramFilesPath=%ProgramFiles(x86)%

goto startResetting

:x86

set ProgramFilesPath=%ProgramFiles%

:startResetting

echo.

if exist “%ProgramFilesPath%\Windows Resource Kits\Tools\subinacl.exe” goto filesExist

echo ***ERROR*** – Could not find file %ProgramFilesPath%\Windows Resource Kits\Tools\subinacl.exe. Double-check that SubInAcl is correctly installed and re-run this script.
goto END

:filesExist

pushd “%ProgramFilesPath%\Windows Resource Kits\Tools”

echo. 
echo Resetting ACLs…
echo (this may take several minutes to complete)
echo. 
echo IMPORTANT NOTE: For this script to run correctly, you must change
echo the values named YOURUSERNAME to be the Windows user account that
echo you are logged in with.
echo.
echo ==========================================================================
echo. 
echo. 
subinacl.exe /subkeyreg HKEY_CURRENT_USER /grant=administrators=f /grant=system=f /grant=restricted=r /grant=Zikenzie=f /setowner=administrators > %temp%\subinacl_output.txt
echo. 
echo. 
subinacl.exe /keyreg HKEY_CURRENT_USER /grant=administrators=f /grant=system=f /grant=restricted=r /grant=Zikenzie=f /setowner=administrators >> %temp%\subinacl_output.txt
echo. 
echo. 
subinacl.exe /subkeyreg HKEY_LOCAL_MACHINE /grant=administrators=f /grant=system=f /grant=users=r /grant=everyone=r /grant=restricted=r /setowner=administrators >> %temp%\subinacl_output.txt
echo. 
echo. 
subinacl.exe /keyreg HKEY_LOCAL_MACHINE /grant=administrators=f /grant=system=f /grant=users=r /grant=everyone=r /grant=restricted=r /setowner=administrators >> %temp%\subinacl_output.txt
echo. 
echo. 
subinacl.exe /subkeyreg HKEY_CLASSES_ROOT /grant=administrators=f /grant=system=f /grant=users=r /setowner=administrators >> %temp%\subinacl_output.txt
echo. 
echo. 
subinacl.exe /keyreg HKEY_CLASSES_ROOT /grant=administrators=f /grant=system=f /grant=users=r /setowner=administrators >> %temp%\subinacl_output.txt
echo. 
echo. 
echo System Drive…
subinacl.exe /subdirectories %ProgramFilesPath%\ /grant=administrators=f /grant=system=f /grant=users=e >> %temp%\subinacl_output.txt
echo. 
echo. 
echo Windows Directory…
subinacl.exe /subdirectories %windir%\ /grant=administrators=f /grant=system=f /grant=users=e >> %temp%\subinacl_output.txt
echo. 
echo. 
echo ==========================================================================
echo. 
echo FINISHED.
echo. 
echo Press any key to exit . . .
pause >NUL

popd

:END

endlocal