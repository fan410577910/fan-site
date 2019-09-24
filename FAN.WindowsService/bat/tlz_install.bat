@echo off

color 0a

set base_dir=%~dp0%
pushd %base_dir%
echo %CD%

echo 正在安装服务TLZ.COM.TBDress.Service.FileMonitor.Service

cd TLZ.COM.TBDress.Service.FileMonitor.Service
TLZ.COM.TBDress.Service.FileMonitor.Service.exe -i

echo 安装服务TLZ.COM.TBDress.Service.FileMonitor.Service完成

echo 正在安装服务TLZ.COM.TBDress.Service.LuceneNet.Service

cd ..\TLZ.COM.TBDress.Service.LuceneNet.Service
TLZ.COM.TBDress.Service.LuceneNet.Service.exe -i

echo 安装服务TLZ.COM.TBDress.Service.LuceneNet.Service完成

echo 正在安装服务TLZ.COM.TBDress.Service.MongoDB.Service

cd ..\TLZ.COM.TBDress.Service.MongoDB.Service
TLZ.COM.TBDress.Service.MongoDB.Service.exe -i

echo 安装服务TLZ.COM.TBDress.Service.MongoDB.Service完成

echo 正在安装服务TLZ.COM.TBDress.Service.Redis.Service

cd ..\TLZ.COM.TBDress.Service.Redis.Service
TLZ.COM.TBDress.Service.Redis.Service.exe -i

echo 安装服务TLZ.COM.TBDress.Service.Redis.Service完成

echo 正在安装服务TLZ.COM.TBDress.Service.SqlServer.Service

cd ..\TLZ.COM.TBDress.Service.SqlServer.Service
TLZ.COM.TBDress.Service.SqlServer.Service.exe -i

echo 安装服务TLZ.COM.TBDress.Service.SqlServer.Service完成

popd

@echo on

pause
