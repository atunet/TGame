for %%i in (*.proto) do (
protoGen -i:%%~ni.proto -o:%%~ni.proto.cs
)


pause