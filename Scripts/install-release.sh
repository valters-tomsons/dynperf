#!/bin/bash

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

cd bin || exit

TARGET=/usr/bin/dynperf
cp dynperf "$TARGET" || { echo -e "\e[31mFailed to copy binary"; exit 1; }
echo "Installed dynperf release binary to $TARGET"

cp "dynperf.1.gz" "/usr/share/man/man1/dynperf.1.gz" || { echo -e "\e[31mFailed to copy man page"; exit 1; }
echo "man page installed"