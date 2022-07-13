C:\Windows\System32\inetsrv\appcmd.exe stop sites "MicroServices"
 
dotnet build "C:\Users\Podo\Desktop\MicroServices\PodoMicroServices\MicroServices.LogService\MicroServices.LogService.sln" --configuration Release

Xcopy /S /I /E /Y  "C:\Users\Podo\Desktop\MicroServices\PodoMicroServices\MicroServices.LogService\MicroServices.LogService.Client\bin\Release\*.nupkg" "C:\Nugets\MicroServices.LogService"

Remove-Item "C:\Nugets\MicroServices.LogService\net6.0"

dotnet publish "C:\Users\Podo\Desktop\MicroServices\PodoMicroServices\MicroServices.LogService\MicroServices.LogService\MicroServices.LogService.csproj" -c Release
Xcopy /S /I /E /Y  "C:\www\MicroServices.LogService\Logger.db" "C:\Users\Podo\Desktop\MicroServices\PodoMicroServices\MicroServices.LogService\MicroServices.LogService\bin\Release\net6.0\publish"
Xcopy /S /I /E /Y  "C:\Users\Podo\Desktop\MicroServices\PodoMicroServices\MicroServices.LogService\MicroServices.LogService\bin\Release\net6.0\publish\*" "C:\www\MicroServices.LogService"


C:\Windows\System32\inetsrv\appcmd.exe start sites "MicroServices"

dotnet test "C:\Users\Podo\Desktop\MicroServices\PodoMicroServices\MicroServices.LogService\MicroServices.LogService.Test\MicroServices.LogService.Test.csproj"
