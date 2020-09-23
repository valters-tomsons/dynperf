#!/bin/sh

if [ "$EUID" -ne 0 ]
  then echo "Please run as root"
  exit
fi

# Copy binary
cd ./release
cp dynperf /usr/bin/dynperf