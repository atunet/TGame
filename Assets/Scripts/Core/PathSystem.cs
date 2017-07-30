using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class PathSystem 
{
    private static string s_rtPlatform = string.Empty;
    public static string GetPlatformStr()
    {
        if (string.IsNullOrEmpty(s_rtPlatform))
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
                s_rtPlatform = "WindowsEditor";
            else if (Application.platform == RuntimePlatform.OSXEditor)
                s_rtPlatform = "MacEditor";
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
                s_rtPlatform = "iOS";
            else if (Application.platform == RuntimePlatform.Android)
                s_rtPlatform = "Android";
            else
                s_rtPlatform = "unknown";
        }
        return s_rtPlatform;
    }


    private static string s_streamingPath = string.Empty;
    public static string StreamingPath()
    {
        if (string.IsNullOrEmpty(s_streamingPath))
        {
            s_streamingPath = string.Format("{0}/{1}/", Application.streamingAssetsPath, GetPlatformStr());
        }
        return s_streamingPath;
    }



    private static string s_persistentPath = string.Empty;
    public static string PersistentPath()
    {
        if (string.IsNullOrEmpty(s_persistentPath))
        {
            s_persistentPath = string.Format("{0}/{1}/", Application.persistentDataPath, GetPlatformStr());
        }
        return s_persistentPath;
    }


    public static string AssetBundlePath(string abName)
    {
        string filePath = string.Format("{0}/{1}.{2}", PersistentPath(), abName, AppConst.AB_EXT_NAME);
        if (!File.Exists (filePath)) 
        {
            filePath = string.Format("{0}/{1}.{2}", StreamingPath(), abName, AppConst.AB_EXT_NAME);
        }
        return filePath;
    }


    private static string s_localAssetPath = "ResSet/Prefab/";
    public static string LocalAssetPath(string prefabName)
    {
        return string.Format("{0}{1}", s_localAssetPath, prefabName);
    }
}

