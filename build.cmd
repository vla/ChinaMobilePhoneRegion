set artifacts=%~dp0artifacts

if exist %artifacts%  rd /q /s %artifacts%

call dotnet restore src/MobilePhoneRegion

call dotnet build src/MobilePhoneRegion -f net462 -c Release -o %artifacts%\net462
call dotnet build src/MobilePhoneRegion -f netstandard2.0 -c Release -o %artifacts%\netstandard2.0

call dotnet pack src/MobilePhoneRegion -c release -o %artifacts%
