for %%i in (*.proto) do (
protogen -i:%%~ni.proto -o:%%~ni.proto.cs
)


pause