using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Net;
using Debug = UnityEngine.Debug;


public class Tool : MonoBehaviour 
{	
	/****************************************************************/
    /********** pack lua && art files to assetbundle files **********/
    /****************************************************************/

	// 需要打包的Lua文件目录列表
	private static string[] s_luaSrcDirs = 
	{ 
        //AppConst.LUA_TOLUA_ROOT,		// tolua自带的lua库目录 
		//AppConst.LUA_LOGIC_PATH, 		// 游戏lua逻辑脚本目录
	};
	private static List<AssetBundleBuild> s_abMaps = new List<AssetBundleBuild>();


    [MenuItem("Tool/Asset Bundle/Build Windows", false, 101)]
    public static void BuildWindowsResource() 
    {
        BuildAssetBundle(BuildTarget.StandaloneWindows);
    }

	[MenuItem("Tool/Asset Bundle/Build Android", false, 102)]
	public static void BuildAndroidResource() 
	{
		BuildAssetBundle(BuildTarget.Android);
	}
        
	[MenuItem("Tool/Asset Bundle/Build Mac", false, 103)]
	public static void BuildMacResource() 
	{
		BuildAssetBundle(BuildTarget.StandaloneOSXIntel);
	}

    [MenuItem("Tool/Asset Bundle/Build iPhone", false, 104)]
    public static void BuildiPhoneResource() 
    {
        BuildAssetBundle(BuildTarget.iOS);
    }

	/// <summary>
	/// 根据平台打包lua文件和美术资源文件
	/// </summary>
	private static void BuildAssetBundle(BuildTarget target_) 
	{
		if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (target_ == BuildTarget.iOS || target_ == BuildTarget.StandaloneOSXIntel)
            {
                Debug.Log("please build assetbundle in osx system!!!");
                return;
            }
        }
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            if (target_ == BuildTarget.StandaloneWindows)
            {
				Debug.Log("please build assetbundle in windows system!!!");
                return;
            }
        }

		s_abMaps.Clear();
		//PreprocessLuaFiles();
		PreprocessArtFiles();
        PreprocessModelFiles();

        string streamingPath = Application.streamingAssetsPath + "/" + GetTargetStr(target_);
        if (!Directory.Exists (streamingPath)) 
		{
            Directory.CreateDirectory (streamingPath);
			AssetDatabase.Refresh ();
		}
        string relativeOutPath = streamingPath.Substring(AppConst.PROJECT_PATH_LEN + 1);

		BuildAssetBundleOptions options = BuildAssetBundleOptions.DeterministicAssetBundle;
		BuildPipeline.BuildAssetBundles(relativeOutPath, s_abMaps.ToArray(), options, target_);

        string[] fileList = Directory.GetFiles(streamingPath, "*", SearchOption.TopDirectoryOnly);
		for(int i = 0; i < fileList.Length; ++i)
		{
			string filePath = fileList[i];
            filePath = filePath.Replace("\\", "/");
			string relativePath = filePath.Substring(filePath.LastIndexOf("/"));
            if(relativePath.Contains(GetTargetStr(target_)))
			{
                string newRelativePath = relativePath.Replace(GetTargetStr(target_), AppConst.VERSION_FILE_NAME);
				string newPath = filePath.Substring(0, filePath.Length-relativePath.Length) + newRelativePath;
                //Debug.Log("path:" + filePath + ",newpath:" + newPath);
                if(File.Exists(newPath)) File.Delete(newPath);
                File.Move(filePath, newPath);
			}
		}
        //BuildScenes(target_);
        AssetDatabase.Refresh();

        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayDialog("Message", "All assetBundles build ok!", "ok");
	}

	/// <summary>
	/// 预处理Lua代码文件
	/// 循环加密lua文件并输出到临时目录并追加.bytes扩展名
	/// 遍历临时目录中的子目录,添加AssetBundleBuild项,bundle命名规则: lua/lua_subdir.unityid
	/// </summary>
	static void PreprocessLuaFiles() 
	{
		string tempLuaDir = Application.dataPath + "/TempLua";
		if(Directory.Exists (tempLuaDir)) Directory.Delete (tempLuaDir, true);
		Directory.CreateDirectory(tempLuaDir);
		AssetDatabase.Refresh();

		// encode  /srcdir/xxx.lua to /dstdir/xxx.lua.bytes
		for (int i = 0; i < s_luaSrcDirs.Length; ++i) 
		{
			string thisDir = s_luaSrcDirs[i];
			if(thisDir.EndsWith("\\") || thisDir.EndsWith("/"))
			{
				thisDir = thisDir.Substring(0, thisDir.Length-1);
			}

			string[] fileList = Directory.GetFiles(thisDir, "*.lua", SearchOption.AllDirectories);
			for (int k = 0; k < fileList.Length; ++k) 
			{
				// "/xxx.lua" or "/*/xxx.lua"
				string subName = fileList[k].Substring(thisDir.Length);
				string dstName = tempLuaDir + subName + ".bytes";
				Directory.CreateDirectory(Path.GetDirectoryName(dstName));
				EncodeLuaFile(fileList[k], dstName);
                //UpdateProgress(k++, fileList.Length, "Encoding lua files");
			}
		}

		string[] subDirList = Directory.GetDirectories(tempLuaDir, "*", SearchOption.AllDirectories);
		for (int i = 0; i < subDirList.Length; i++) 
		{
			string subPart = subDirList[i].Substring(tempLuaDir.Length+1);
			subPart = subPart.Replace('\\', '_').Replace('/', '_');
			string abName = "lua/lua_" + subPart.ToLower() + AppConst.AB_EXT_NAME;

			AddBuildMap(abName, subDirList[i].Substring(AppConst.PROJECT_PATH_LEN+1), "*.bytes");
		}
		string rootABName = "lua/lua" + AppConst.AB_EXT_NAME;
		AddBuildMap(rootABName, tempLuaDir.Substring(AppConst.PROJECT_PATH_LEN+1), "*.bytes");

		AssetDatabase.Refresh();
	}

	/// <summary>
	/// 预处理美术资源文件
	/// </summary>
	static void PreprocessArtFiles() 
	{
        string resPath = Application.dataPath + "/SrcRes";
        SetSpriteTag(resPath);

        string[] dirList = Directory.GetDirectories(resPath, "*", SearchOption.AllDirectories);
        for (int i = 0; i < dirList.Length; ++i)
        {
            if (dirList[i].Contains("SrcRes") && dirList[i].Contains("Model"))
                continue;

            string relativeDir = dirList[i].Substring(resPath.Length+1);
            string abName = relativeDir.Replace("\\", "_").Replace("/", "_") + AppConst.AB_EXT_NAME;

            string[] fileList = Directory.GetFiles(dirList[i], "*", SearchOption.TopDirectoryOnly);

            int realLength = 0; // 过滤xxx.meta之后的文件数量
            for(int j = 0; j < fileList.Length; ++j)
            {
                if (fileList[j].EndsWith(".meta") || 
					fileList[j].Contains("ds_store") ||
					fileList[j].Contains(".DS_Store")) continue;
                realLength++;
            }
            if (0 == realLength) continue;

            AssetBundleBuild build = new AssetBundleBuild();
            build.assetNames = new string[realLength];
            build.assetBundleName = abName;

            int index = 0;
            for (int k = 0; k < fileList.Length; ++k)
            {             
                if (fileList[k].EndsWith(".meta") || 
					fileList[k].Contains("ds_store") ||
					fileList[k].Contains(".DS_Store")) continue;
                             
                string relativeFilePath = fileList[k].Replace("\\", "/").Substring(AppConst.PROJECT_PATH_LEN+1);
                build.assetNames[index++] = relativeFilePath;

                string barInfo = "Build ab file:" + relativeFilePath + " to " + abName + " ... (" + k + "/" + fileList.Length + ")";
                EditorUtility.DisplayProgressBar("Build assetbundle map", barInfo, (float)k / (float)fileList.Length);

                Debug.Log(barInfo);
            }

            s_abMaps.Add(build);
        }

        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();

        /*
        AddBuildMap("role_login" + AppConst.AB_EXT_NAME, relativePath, "*.prefab");
        AddBuildMap("prompt" + AppConst.AB_EXT_NAME, "*.prefab", "Assets/LuaFramework/Examples/Builds/Prompt");
		AddBuildMap("message" + AppConst.AB_EXT_NAME, "*.prefab", "Assets/LuaFramework/Examples/Builds/Message");
		AddBuildMap("prompt_asset" + AppConst.AB_EXT_NAME, "*.png", "Assets/LuaFramework/Examples/Textures/Prompt");
		AddBuildMap("shared_asset" + AppConst.AB_EXT_NAME, "*.png", "Assets/LuaFramework/Examples/Textures/Shared");
	    */
    }

    static void PreprocessModelFiles()
    {
        string modelPath = Application.dataPath + "/SrcRes/Model";
        string[] subPathList = Directory.GetDirectories(modelPath, "*", SearchOption.TopDirectoryOnly);

        for (int n = 0; n < subPathList.Length; ++n)
        {
            string[] dirList = Directory.GetDirectories(subPathList[n], "*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < dirList.Length; ++i)
            {
                string relativeDir = dirList[i].Substring(modelPath.Length + 1);
                string abName = relativeDir.Replace("\\", "_").Replace("/", "_") + AppConst.AB_EXT_NAME;

                string[] fileList = Directory.GetFiles(dirList[i], "*", SearchOption.AllDirectories);
                
                int realLength = 0; // 过滤xxx.meta之后的文件数量
                for (int j = 0; j < fileList.Length; ++j)
                {
                    if (fileList[j].EndsWith(".meta") || 
						fileList[j].Contains("ds_store") ||
						fileList[j].Contains(".DS_Store")) continue;
                    realLength++;
                }
                if (0 == realLength) continue;

                AssetBundleBuild build = new AssetBundleBuild();
                build.assetNames = new string[realLength];
                build.assetBundleName = abName;

                int index = 0;
                for (int k = 0; k < fileList.Length; ++k)
                {             
                    if (fileList[k].EndsWith(".meta") || 
						fileList[k].Contains("ds_store") ||
						fileList[k].Contains(".DS_Store")) continue;

                    string relativeFilePath = fileList[k].Replace("\\", "/").Substring(AppConst.PROJECT_PATH_LEN + 1);
                    build.assetNames[index++] = relativeFilePath;

                    string barInfo = "Build ab file:" + relativeFilePath + " to " + abName + " ... (" + k + "/" + fileList.Length + ")";
                    EditorUtility.DisplayProgressBar("Build assetbundle map", barInfo, (float)k / (float)fileList.Length);

                    Debug.Log(barInfo);
                }

                s_abMaps.Add(build);
            }
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    static void SetSpriteTag(string path_)
    {
        string[] fileList = Directory.GetFiles(path_, "*.png", SearchOption.AllDirectories);
        for (int i = 0; i < fileList.Length; ++i)
        {
            string filePath = fileList[i].Replace("\\", "/");
            string relativePath = filePath.Substring(AppConst.PROJECT_PATH_LEN + 1);
            string packTagName = Path.GetDirectoryName(filePath).Substring(path_.Length+1).Replace("/", "_");

            string barInfo = "Setting sprite tag:" + relativePath + "(" + i + "/" + fileList.Length + ")";
            EditorUtility.DisplayProgressBar("Set sprite tag", barInfo, (float)i / (float)fileList.Length);

            TextureImporter importer = TextureImporter.GetAtPath(relativePath) as TextureImporter;
            if (null == importer)
            {
                Debug.LogError("set sprite tag failed: " + filePath + ", relative:" + relativePath + ", tagname:" + packTagName);
                EditorUtility.ClearProgressBar();
                return;
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spritePackingTag = packTagName;
            importer.mipmapEnabled = false;
            importer.SaveAndReimport();
        }

        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }


    [MenuItem("Tool/Asset Bundle/Build Scenes Windows", false, 201)]
    public static void BuildScenesWindows() 
    {
        BuildScenes(BuildTarget.StandaloneWindows);
    }

    [MenuItem("Tool/Asset Bundle/Build Scenes Android", false, 202)]
    public static void BuildScenesAndroid() 
    {
        BuildScenes(BuildTarget.Android);
    }

    [MenuItem("Tool/Asset Bundle/Build Scenes Mac", false, 203)]
    public static void BuildScenesMac() 
    {
        BuildScenes(BuildTarget.StandaloneOSXIntel);
    }

    [MenuItem("Tool/Asset Bundle/Build Scenes iPhone", false, 204)]
    public static void BuildScenesiPhone() 
    {
        BuildScenes(BuildTarget.iOS);
    }

    static void BuildScenes(BuildTarget target_)
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (target_ == BuildTarget.iOS || target_ == BuildTarget.StandaloneOSXIntel)
            {
                Debug.Log("please build scene assetbundle in osx system!!!");
                return;
            }
        }
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            if (target_ == BuildTarget.StandaloneWindows ||
                target_ == BuildTarget.StandaloneWindows64)
            {
                Debug.Log("please build scene assetbundle in windows system!!!");
                return;
            }
        }
            
        string scenePath = Application.dataPath + "/Scenes";
        string streamingPath = Application.streamingAssetsPath + "/" + GetTargetStr(target_);
        string relativeOutPath = streamingPath.Substring(AppConst.PROJECT_PATH_LEN + 1);

        string[] fileList = Directory.GetFiles(scenePath, "*.unity", SearchOption.TopDirectoryOnly);
        for(int i = 0; i < fileList.Length; ++i)
        {
            Debug.Log(fileList[i]);
        }
        //for (int i = 0; i < fileList.Length; ++i)
        {
            BuildPipeline.BuildPlayer(fileList, relativeOutPath + "/scenes.unity3d", target_, BuildOptions.BuildAdditionalStreamedScenes);
        }

    }

	static void AddBuildMap(string abName_, string relativePath_, string pattern_) 
	{
		string[] files = Directory.GetFiles(relativePath_, pattern_);
		if (0 == files.Length) return;

		for (int i = 0; i < files.Length; ++i) 
		{
			files[i] = files[i].Replace('\\', '/');
			//Debug.Log("AddMap:" + abName_ + "," + files[i]);
		}

		AssetBundleBuild build = new AssetBundleBuild();
		build.assetBundleName = abName_;
		build.assetNames = files;
		s_abMaps.Add(build);
	}

	public static void EncodeLuaFile(string srcFile_, string outFile_) 
	{
		if (!srcFile_.ToLower().EndsWith(".lua")) 
		{
			File.Copy(srcFile_, outFile_, true);
			return;
		}

		//string currDir = Directory.GetCurrentDirectory();
		//string exeDir = string.Empty;

		ProcessStartInfo processInfo = new ProcessStartInfo();	
		processInfo.WindowStyle = ProcessWindowStyle.Hidden;
		processInfo.ErrorDialog = true;

		if (Application.platform == RuntimePlatform.WindowsEditor) 
		{
            processInfo.FileName = AppConst.PROJECT_PATH + "/LuaEncoder/luajit/luajit.exe";
			processInfo.Arguments = "-b " + srcFile_ + " " + outFile_;
			processInfo.UseShellExecute = true;
			//exeDir = AppConst.PROJECT_PATH + "/LuaEncoder/luajit/";
		}
		else if (Application.platform == RuntimePlatform.OSXEditor) 
		{
            processInfo.FileName = AppConst.PROJECT_PATH + "/LuaEncoder/luavm/luac";
			processInfo.Arguments = "-o " + outFile_ + " " + srcFile_;
			processInfo.UseShellExecute = false;
			//exeDir = AppConst.PROJECT_PATH + "/LuaEncoder/luavm/";
		}

		Debug.Log(processInfo.FileName + " " + processInfo.Arguments);
		//Directory.SetCurrentDirectory(exeDir);
		Process.Start(processInfo).WaitForExit();
		//Directory.SetCurrentDirectory(currDir);
	}

    static void UpdateProgress(int currentNum_, int maxNum_, string title_) 
	{
        string desc = "Processing...[" + currentNum_ + " - " + maxNum_ + "]";
        float value = (float)currentNum_ / (float)maxNum_;
        EditorUtility.DisplayProgressBar(title_, desc, value);
    }

	/****************************************************************/
    /************* upload assetbundle files to web server ***********/
    /****************************************************************/
    /*
    [MenuItem("Tool/Upload Resource/Upload Windows", false, 200)]
    public static void UploadWinResource()
    {
        UploadResource(BuildTarget.StandaloneWindows);
    }
      */  
    [MenuItem("Tool/Upload Resource/Upload Android", false, 201)]
    public static void UploadAndroidResource()
    {
        UploadResource(BuildTarget.Android);
    }

    [MenuItem("Tool/Upload Resource/Upload Mac", false, 202)]
    public static void UploadMacResource()
    {
        UploadResource(BuildTarget.StandaloneOSXIntel);
    }

    [MenuItem("Tool/Upload Resource/Upload iPhone", false, 203)]
    public static void UploadIosResource()
    {
        UploadResource(BuildTarget.iOS);
    }

    private static string GetTargetStr(BuildTarget target_)
    {
        if (target_ == BuildTarget.StandaloneWindows ||
            target_ == BuildTarget.StandaloneWindows64)
            return "Windows";
        else if (target_ == BuildTarget.StandaloneOSXIntel)
            return "Mac";
        else if (target_ == BuildTarget.iOS)
            return "iOS";
        else if (target_ == BuildTarget.Android)
            return "Android";
        else
            return "";
    }

    private static void UploadResource (BuildTarget target_)
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (target_ == BuildTarget.iOS || target_ == BuildTarget.StandaloneOSXIntel)
            {
				EditorUtility.DisplayDialog("Warnning", "Please upload ios/osx files in osx system!", "ok");
                return;
            }
        }
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            if (target_ == BuildTarget.StandaloneWindows)
            {
				EditorUtility.DisplayDialog("Warnning", "Please upload windows files in windows system!", "ok");
                return;
            }
        }

        // step 1: create new dirs 
		/*string[] dirList = Directory.GetDirectories(AppConst.STREAMING_PATH);
        for (int i = 0; i < dirList.Length; ++i)
        {
			string relativePath = dirList[i].Substring(AppConst.STREAMING_PATH.Length);
            string remotePath = AppConst.RES_SERVER_PATH + relativePath;
            remotePath = remotePath.Replace("\\", "/");
            Debug.Log("Upload dir: " + dirList[i] + " => " + remotePath);

            ProcessStartInfo pInfo = new ProcessStartInfo();  
            pInfo.WindowStyle = ProcessWindowStyle.Hidden;
            pInfo.ErrorDialog = true;
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                pInfo.FileName = "plink.exe";
                pInfo.Arguments = "-l tfx -pw sunrise 121.199.48.63 mkdir -p " + remotePath;
                pInfo.UseShellExecute = true;

                string currDir = Directory.GetCurrentDirectory();
                string exeDir = AppConst.PROJECT_PATH + "/putty/";

                Directory.SetCurrentDirectory(exeDir);
                Process.Start(pInfo).WaitForExit();
                Directory.SetCurrentDirectory(currDir);
            }
            else if (Application.platform == RuntimePlatform.OSXEditor)
            {
                pInfo.FileName = "sshpass";
                pInfo.Arguments = "-p sunrise ssh tfx@" + AppConst.RES_SERVER_IP + " \"mkdir -p " + remotePath + "\"";
                pInfo.UseShellExecute = false;
                Process.Start(pInfo).WaitForExit();
            }
        }*/

        string streamingPath = Application.streamingAssetsPath + "/" + GetTargetStr(target_);

        // step 2: upload all resource files to server
        string[] allList = Directory.GetFiles(streamingPath, "*.unity3d", SearchOption.AllDirectories);
		string[] fileList = new string[allList.Length+1];
		allList.CopyTo(fileList, 0);
        fileList[fileList.Length-1] = streamingPath + "/" + AppConst.VERSION_FILE_NAME;
        for(int i = 0; i < fileList.Length; ++i)
        {			
			fileList[i] = fileList[i].Replace("/", "\\");
            string relativePath = fileList[i].Substring(streamingPath.Length);
            string remotePath = AppConst.RES_SERVER_PATH + GetTargetStr(target_) + "/" + relativePath;
            remotePath = remotePath.Replace("\\", "/");

			string barInfo = "Uploading file:" + relativePath.Substring(1) + "(" + (i+1) + "/" + fileList.Length + ")";
			EditorUtility.DisplayProgressBar("Uploading files to server", barInfo, (float)(i+1) / (float)fileList.Length);

           	ProcessStartInfo processInfo = new ProcessStartInfo();  
	        processInfo.WindowStyle = ProcessWindowStyle.Hidden;
	        processInfo.ErrorDialog = true;
	      /*  if (Application.platform == RuntimePlatform.WindowsEditor) 
	        {
                processInfo.FileName = AppConst.PROJECT_PATH + "/putty/pscp.exe";
	            processInfo.Arguments = "-pw sunrise -r " + fileList[i] + " " + "   tfx@" + AppConst.RES_SERVER_IP + ":" + remotePath;
	            processInfo.UseShellExecute = true;
				Debug.Log(processInfo.FileName + " " + processInfo.Arguments);

	            string currDir = Directory.GetCurrentDirectory();
	            string exeDir = AppConst.PROJECT_PATH + "/putty/";

			    //Directory.SetCurrentDirectory(exeDir);
				Process.Start(processInfo).WaitForExit();
				//Directory.SetCurrentDirectory(currDir);
	        }
	        else if (Application.platform == RuntimePlatform.OSXEditor) 
	        */
            {
	            processInfo.FileName = "scp";
                processInfo.Arguments = "-r " + fileList[i].Replace("\\", "/") + "   tfx@" + AppConst.RES_SERVER_IP + ":" + remotePath;
	            processInfo.UseShellExecute = false;
	            Process.Start(processInfo).WaitForExit();
	        }
			Debug.Log(processInfo.FileName + " " + processInfo.Arguments);
	    }

        EditorUtility.ClearProgressBar();
		EditorUtility.DisplayDialog("Message", "All resource files upload finished!", "ok");
    }



    /****************************************************************/
    /**************** CSV config file convert to lua ****************/
    /****************************************************************/

	static string s_inputDir = Application.dataPath + "/Config/";
	static string s_outputDir = Application.dataPath + "/Scripts/LuaLogic/config/";
	
	[MenuItem("Tool/Csv To Lua", false, 1)]
	public static void csv2Lua()
	{
		DirectoryInfo dirInfo = new DirectoryInfo(s_inputDir);
		if(!dirInfo.Exists)
		{
			Debug.LogError("Config dir not exists: " + s_inputDir);
			return;
		}		
		
		FileInfo[] fileList = dirInfo.GetFiles("*.csv");
		for(int i = 0; i < fileList.Length; ++i)
		{
			if(fileList[i].Name == "gameOption.csv")
			{
				parseCsvHorizontal(fileList[i].Name);
			}
			else
			{
				parseCsvVertical(fileList[i].Name);
			}
		}
	}
	
	private static void parseCsvHorizontal(string fileName_)
	{
        string inputPath = s_inputDir + fileName_;
        string outputPath = s_outputDir + fileName_.Replace("csv", "lua");

        Debug.Log("input path:" + inputPath);
        Debug.Log("output path:" + outputPath);

        StreamWriter writer = new StreamWriter(outputPath);
        writer.WriteLine(fileName_.Split('.')[0] + " = ");
		writer.WriteLine("{");
		
		string keyName = "id", valueName = "value";
		int keyIndex = 0, valueIndex = 0;
		
        StreamReader reader = new StreamReader(inputPath);		
		
		string line;
		while(null != (line = reader.ReadLine()))
		{
			int offset = 0;
			while(offset < line.Length)
			{
				int startIndex = line.IndexOf('"', offset);
				if(-1 == startIndex) break;
				int endIndex = line.IndexOf('"', startIndex+1);
				if(-1 == endIndex) break;

				//Debug.Log("start:" + startIndex + ",end:" + endIndex);

				string oldStr = line.Substring(startIndex, endIndex-startIndex);
                string newStr = oldStr.Replace(",", ";");
                line = line.Replace(oldStr, newStr);   

                offset = endIndex+1;
			}

            //Debug.Log("line:" + line);

			string[] allFields = line.Split(',');						
			if("" == allFields[0])
			{
				if(allFields.Length < valueIndex+1) 
				{
					Debug.LogWarning("the column count of line not fix the count of key list");
					return;
				}

				string key = allFields[keyIndex];
				string value = allFields[valueIndex];

                value = value.Replace("\"", "");
                value = value.Replace('[', '{');
                value = value.Replace(']', '}');
                string destValue = value;
				
                int round = 0;
                Match m = Regex.Match(value, "\\d{1,2}:\\d{1,2}:\\d{1,2}");
                while(m.Success)
				{
                   // Debug.Log("match value:" + value.Substring(m.Index, m.Length) + "," + m.Index + "," + (m.Index+m.Length));

                    destValue = destValue.Insert(2*round+m.Index, "\"");
                    destValue = destValue.Insert(2*round+m.Index+m.Length+1, "\"");
                    round++;

                    m = m.NextMatch();					
                }

				//if(round > 0) Debug.Log("after deal date:" + destValue);

                destValue = destValue.Replace(";", ",");
                writer.WriteLine("\t" + key + " = " + destValue + ",");
			}	
			else if("!" == allFields[0])
            {
				for(int i = 0; i < allFields.Length; ++i)
                {
					if(allFields[i] == keyName) keyIndex = i;
					else if(allFields[i] == valueName) valueIndex = i;
                }   
                //Debug.Log("keyindex:" + keyIndex + ",valueindex:" + valueIndex);
            }
		}
		
		reader.Close();
		reader = null;
		
		writer.WriteLine("}");
		writer.Close();
		writer = null;
	}
	
	private static void parseCsvVertical(string fileName_)
	{
		string inputPath = s_inputDir + fileName_;
        string outputPath = s_outputDir + fileName_.Replace("csv", "lua");

        Debug.Log("input path:" + inputPath);
        Debug.Log("output path:" + outputPath);

        StreamWriter writer = new StreamWriter(outputPath);
        writer.WriteLine(fileName_.Split('.')[0] + " = ");
		writer.WriteLine("{");
		
		ArrayList keyList = new ArrayList();

        StreamReader reader = new StreamReader(inputPath);		
		
		string line;
		while(null != (line = reader.ReadLine()))
		{
			int offset = 0;
			while(offset < line.Length)
			{
				int startIndex = line.IndexOf('"', offset);
				if(-1 == startIndex) break;
				int endIndex = line.IndexOf('"', startIndex+1);
				if(-1 == endIndex) break;

				//Debug.Log("start:" + startIndex + ",end:" + endIndex);
				string oldStr = line.Substring(startIndex, endIndex-startIndex);
                string newStr = oldStr.Replace(",", ";");
                line = line.Replace(oldStr, newStr);   

                offset = endIndex+1;
			}

           // Debug.Log("line:" + line);

			string[] allFields = line.Split(',');						
			if("" == allFields[0])
			{
				if(allFields.Length != keyList.Count) 
				{
					Debug.LogWarning("the column count of line not fix the count of key list");
					return;
				}

	            string destLine = "\t{ ";
				for(int j = 1; j < allFields.Length; ++j)
	            {
	            	if(keyList[j].Equals("Skip")) continue;

					string field = allFields[j];
					field = field.Replace("\"", "");
					field = field.Replace('[', '{');
					field = field.Replace(']', '}');
	                string destField = field;
					
	                int round = 0;
					Match m = Regex.Match(field, "\\d{1,2}:\\d{1,2}:\\d{1,2}");
	                while(m.Success)
					{
						//Debug.Log("match value:" + field.Substring(m.Index, m.Length) + "," + m.Index + "," + (m.Index+m.Length));

						destField = destField.Insert(2*round+m.Index, "\"");
						destField = destField.Insert(2*round+m.Index+m.Length+1, "\"");
	                    round++;

	                    m = m.NextMatch();					
	                }

					//if(round > 0) Debug.Log("after deal date:" + destField);

					destField = destField.Replace(";", ",");
					if(1 == j)
					{
						destLine += (keyList[j] + " = " + destField);
					}
					else
						destLine += (", " + keyList[j] + " = " + destField);
	            }

                writer.WriteLine(destLine + " },");
			}	
            else if("!" == allFields[0])
            {
				for(int i = 0; i < allFields.Length; ++i)
                {
					keyList.Add(allFields[i]);
					//Debug.Log("key:" + keyList[i]);
                }   
            }
		}
		
		reader.Close();
		reader = null;
		
		writer.WriteLine("}");
		writer.Close();
		writer = null;
	}
}
