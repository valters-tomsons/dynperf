#!/bin/sh
dotnet publish -r linux-x64 -c Release /p:PublishSingleFile=true -o dynperf-release
cp ./dynperf-server/Defaults ./dynperf-release/ -r