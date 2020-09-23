#!/bin/sh

if [ "$EUID" -ne 0 ]
  then echo -e "\e[33m Run as root!"
  exit
fi

cd ..

FILE=bin/dynperf
if ! test -f "$FILE"; then
    echo -e "> Release binary not found"
    echo -e "\e[33m Run ./build-release.sh first"
    exit
fi

# Copy binary
cd ./bin
cp dynperf /usr/bin/dynperf