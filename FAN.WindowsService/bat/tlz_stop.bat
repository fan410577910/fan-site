@echo off

color 0a

set base_dir=%~dp0%
pushd %base_dir%
echo %CD%

echo ����ֹͣ����

cd TLZ.COM.TBDress.Service.FileMonitor.Service

net stop TLZ.COM.TBDress.Service.FileMonitor.Service

cd ..\TLZ.COM.TBDress.Service.LuceneNet.Service

net stop TLZ.COM.TBDress.Service.LuceneNet.Service

cd ..\TLZ.COM.TBDress.Service.MongoDB.Service

net stop TLZ.COM.TBDress.Service.MongoDB.Service

cd ..\TLZ.COM.TBDress.Service.Redis.Service

net stop TLZ.COM.TBDress.Service.Redis.Service

cd ..\TLZ.COM.TBDress.Service.SqlServer.Service

net stop TLZ.COM.TBDress.Service.SqlServer.Service

echo ֹͣ�������

popd

@echo on

pause