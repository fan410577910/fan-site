del /s /f /q d:\tlz_upload\*.config
del /s /f /q d:\tlz_upload\*.xml
del /s /f /q d:\tlz_upload\*.pdb
del /s /f /q d:\tlz\*.InstallLog
del /s /f /q d:\tlz\*.InstallState
del /s /f /q d:\tlz\*.txt
xcopy /s /y d:\tlz_upload\*.* d:\tlz
del /s /f /q d:\tlz_upload\*.*
pause