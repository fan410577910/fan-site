@echo off

color 0a

set base_dir=%~dp0%
pushd %base_dir%
echo %CD%

echo ����ж�ط���TLZ.COM.TBDress.Service.FileMonitor.Service

cd TLZ.COM.TBDress.Service.FileMonitor.Service
TLZ.COM.TBDress.Service.FileMonitor.Service.exe -u

echo ж�ط���TLZ.COM.TBDress.Service.FileMonitor.Service���

echo ����ж�ط���TLZ.COM.TBDress.Service.LuceneNet.Service

cd ..\TLZ.COM.TBDress.Service.LuceneNet.Service
TLZ.COM.TBDress.Service.LuceneNet.Service.exe -u

echo ж�ط���TLZ.COM.TBDress.Service.LuceneNet.Service���

echo ����ж�ط���TLZ.COM.TBDress.Service.MongoDB.Service

cd ..\TLZ.COM.TBDress.Service.MongoDB.Service
TLZ.COM.TBDress.Service.MongoDB.Service.exe -u

echo ж�ط���TLZ.COM.TBDress.Service.MongoDB.Service���

echo ����ж�ط���TLZ.COM.TBDress.Service.Redis.Service

cd ..\TLZ.COM.TBDress.Service.Redis.Service
TLZ.COM.TBDress.Service.Redis.Service.exe -u

echo ж�ط���TLZ.COM.TBDress.Service.Redis.Service���

echo ����ж�ط���TLZ.COM.TBDress.Service.SqlServer.Service

cd ..\TLZ.COM.TBDress.Service.SqlServer.Service
TLZ.COM.TBDress.Service.SqlServer.Service.exe -u

echo ж�ط���TLZ.COM.TBDress.Service.SqlServer.Service���

popd

@echo on

pause