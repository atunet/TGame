using UnityEngine;

public static class AppConst
{
#if UNITY_EDITOR
	public static string ASSETS_DIR_NAME = "/Assets";
	public static string PROJECT_PATH = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf(ASSETS_DIR_NAME));
	public static int PROJECT_PATH_LEN = PROJECT_PATH.Length;
#endif
    public static string PLATFORM = GetPlatformStr(Application.platform);
	public static string VERSION_FILE_NAME = "version_file";

	public static string STREAMING_PATH = Application.streamingAssetsPath + "/" + PLATFORM;
    public static string STREAMING_VERSION_FILE_PATH = STREAMING_PATH + "/" + VERSION_FILE_NAME;

	public static string PERSISTENT_PATH = Application.persistentDataPath + "/" + PLATFORM;
    public static string PERSISTENT_VERSION_FILE_PATH = PERSISTENT_PATH + "/" + VERSION_FILE_NAME;

	public static string REMOTE_URL = "http://121.199.48.63/res/firework";
    public static string REMOTE_ASSET_URL = REMOTE_URL + "/" + PLATFORM;						// do not use Path.Combine()
    public static string REMOTE_VERSION_FILE_URL = REMOTE_ASSET_URL + "/" + VERSION_FILE_NAME;	// do not use Path.Combine()

    public static string RES_SERVER_IP = "121.199.48.63";
    public static string RES_SERVER_PATH = "/var/www/html/res/firework/";

	public static string LUA_TEMP_PATH = "Assets/StreamingAssets/Temp/Lua";
	public static string LUA_TOLUA_PATH = Application.dataPath + "/ToLua/Lua/protobuf";
	public static string LUA_LOGIC_PATH = Application.dataPath + "/Scripts/LuaScripts";
    public static string LUA_TOLUA_ROOT = Application.dataPath + "/ToLua/Lua";

	public static string AB_EXT_NAME = ".unity3d";

    public static string GetPlatformStr(RuntimePlatform platform_)
    {
        if (platform_ == RuntimePlatform.WindowsEditor)
            return "Windows";
        else if (platform_ == RuntimePlatform.OSXEditor)
            return "Mac";
        else if (platform_ == RuntimePlatform.IPhonePlayer)
            return "iOS";
        else if (platform_ == RuntimePlatform.Android)
            return "Android";
        else
            return "";
    }
	public static void PrintPath()
	{
   		Utility.Log("[AppConst]PLATFROM:" + PLATFORM);
        //Utility.Log("VERSION_FILE_NAME:" + VERSION_FILE_NAME);

		//Utility.Log("STREAMING_PATH:" + STREAMING_PATH);
		Utility.Log("[AppConst]STREAMING_VERSION_FILE:" + STREAMING_VERSION_FILE_PATH);

		//Utility.Log("PERSISTENT_PATH:" + PERSISTENT_PATH);
        Utility.Log("[AppConst]PERSISTENT_VERSION_FILE:" + PERSISTENT_VERSION_FILE_PATH);

		//Utility.Log("REMOTE_URL:" + REMOTE_URL);
		//Utility.Log("REMOTE_ASSET_URL:" + REMOTE_ASSET_URL);
        Utility.Log("[AppConst]REMOTE_VERSION_FILE_URL:" + REMOTE_VERSION_FILE_URL);

		//Utility.Log("UPLOAD_URL:" + UPLOAD_URL);
		//Utility.Log("UPLOAD_ASSET_URL:" + UPLOAD_ASSET_URL);
    }	 

    // 所有assetbundle的名字统一定义列表
    public static string AB_VERSION = "versionfile";
    public static string AB_COMMON = "prefab_common";
    public static string AB_LOGIN = "prefab_login";
    public static string AB_CREATE_ROLE = "create_role";
    public static string AB_MAIN = "prefab_mainui";
    public static string AB_BATTLE = "prefab_battleui";
}
