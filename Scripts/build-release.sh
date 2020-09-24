#!/bin/sh

cd ..
dotnet publish -c Release -r linux-x64 -o ./bin

gzip dynperf.1 -k
mv dynperf.1.gz "./bin/dynperf.1.gz"