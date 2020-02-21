#!/bin/sh
dotnet publish -c Release --self-contained -o dynperf-release
cp ./dynperf-server/Defaults ./dynperf-release/ -r