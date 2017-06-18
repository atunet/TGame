for file in ./*.proto
do
	mono /Users/aTunet/Desktop/repoGit/protogen/bin/protogen.exe  -i:$file -o:$file.pb.cs
done
