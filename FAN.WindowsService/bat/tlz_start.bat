@echo off

color 0a

set base_dir=%~dp0%
pushd %base_dir%
echo %CD%

echo 正在启动服务

cd TLZ.COM.TBDress.Service.FileMonitor.Service

net start TLZ.COM.TBDress.Service.FileMonitor.Service

cd ..\TLZ.COM.TBDress.Service.LuceneNet.Service

net start TLZ.COM.TBDress.Service.LuceneNet.Service

cd ..\TLZ.COM.TBDress.Service.MongoDB.Service

net start TLZ.COM.TBDress.Service.MongoDB.Service

cd ..\TLZ.COM.TBDress.Service.Redis.Service

net start TLZ.COM.TBDress.Service.Redis.Service

cd ..\TLZ.COM.TBDress.Service.SqlServer.Service

net start TLZ.COM.TBDress.Service.SqlServer.Service

echo 启动服务完成

popd

@echo on

pause
