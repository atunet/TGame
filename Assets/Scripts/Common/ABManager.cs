
using UnityEngine;
using System.IO;
using System.Collections.Generic;


public sealed class ABManager
{
    private static Dictionary<string, AssetBundle> s_abMaps = new Dictionary<string, AssetBundle>();

    public static AssetBundle get(string abName_)
    {
        AssetBundle ab = null;
        if (s_abMaps.TryGetValue(abName_, out ab)) 
			return ab;

		string filePath = AppConst.PERSISTENT_PATH + "/" + abName_ + AppConst.AB_EXT_NAME;
		if (!File.Exists (filePath))
			filePath = AppConst.STREAMING_PATH + "/" + abName_ + AppConst.AB_EXT_NAME;
		if (!File.Exists (filePath)) 
		{
			Utility.LogError ("ABManager: ab file not exist:" + filePath);
			return ab;
		}

		ab = AssetBundle.LoadFromFile(filePath);		
        if (null != ab)
        {           
            s_abMaps.Add(abName_, ab);

            string[] assetNames = ab.GetAllAssetNames();
            for (int i = 0; i < assetNames.Length; ++i)
            {
                //Debug.Log("asset name:" + assetNames[i]);
            }
			Utility.Log("ABManager: load ab file success:" + abName_ + AppConst.AB_EXT_NAME);
        }
        else
			Utility.LogError("ABManager: load ab failed,file not existed:" + abName_ + AppConst.AB_EXT_NAME);

        return ab;
    }

    public static void UnloadAB(string abName_)
    {
        AssetBundle ab = null;
        if (s_abMaps.TryGetValue(abName_, out ab))
        {
            Resources.UnloadAsset(ab);
            Resources.UnloadUnusedAssets();

            s_abMaps.Remove(abName_);
        }
    }

    public static void UnloadAll()
    {
		foreach(KeyValuePair<string, AssetBundle> kv in s_abMaps)
		{
			Resources.UnloadAsset(kv.Value);
			Utility.Log("ABManager: unload all ab resources:" + kv.Key);
		}

		s_abMaps.Clear();
		Resources.UnloadUnusedAssets();
	}
}
