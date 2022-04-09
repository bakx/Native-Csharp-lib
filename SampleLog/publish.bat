call "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\VC\Auxiliary\Build\vcvarsall.bat" x64
dotnet publish /p:NativeLib=Shared --self-contained -r win-x64 -c release