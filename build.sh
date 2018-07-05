#!/usr/bin/env bash
set -e
basepath=$(cd `dirname $0`; pwd)
artifacts=${basepath}/artifacts

if [[ -d ${artifacts} ]]; then
   rm -rf ${artifacts}
fi

mkdir -p ${artifacts}

dotnet restore src/MobilePhoneRegion

dotnet build src/MobilePhoneRegion -f net40 -c Release -o ${artifacts}/net40
dotnet build src/MobilePhoneRegion -f netstandard2.0 -c Release -o ${artifacts}/netstandard2.0

