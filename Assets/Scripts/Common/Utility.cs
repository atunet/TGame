using System;
using System.IO;

public static class Utility
{
    public static void Log(string text_)
    {
        UnityEngine.Debug.Log(DateTime.Now.ToString() + " : " + text_);
    }

    public static void LogWarning(string text_)
    {
        UnityEngine.Debug.LogWarning(DateTime.Now.ToString() + " : " + text_);
    }

    public static void LogError(string text_)
    {
        UnityEngine.Debug.LogError(DateTime.Now.ToString() + " : " + text_);
    }

	public static string GetResourcePath(string fileName_)
	{
		string filePath = AppConst.PERSISTENT_PATH + "/" + fileName_ + AppConst.AB_EXT_NAME;
		if (!File.Exists (filePath)) 
		{
			filePath = AppConst.STREAMING_PATH + "/" + fileName_ + AppConst.AB_EXT_NAME;
		}
		return filePath;
	}
}
