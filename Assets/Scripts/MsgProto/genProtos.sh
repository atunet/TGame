#!/bin/sh


for file in ./*.proto
do
	echo convert proto to lua file: $file ...
	protoc --lua_out=../LuaScripts/Protol/ $file

	echo convert proto to binary file: $file ...
	protoc -o ../LuaScripts/Protol/${file/proto/pb} $file 
done
