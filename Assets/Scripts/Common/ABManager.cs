
using UnityEngine;
using System.Collections.Generic;


public sealed class ABManager
{
    private static Dictionary<string, AssetBundle> s_abMaps = new Dictionary<string, AssetBundle>();

    public static AssetBundle get(string abName_)
    {
        AssetBundle ab = null;
        if (s_abMaps.TryGetValue(abName_, out ab)) return ab;

    /*    AssetBundle depAB = null;
        string depABName = "sprite_logo";
        if (!s_abMaps.TryGetValue(depABName, out depAB))
        {
            depAB = AssetBundle.LoadFromFile(AppConst.PERSISTENT_PATH + "/" + depABName + AppConst.AB_EXT_NAME);
			if (null == depAB) 
				depAB = AssetBundle.LoadFromFile(AppConst.STREAMING_PATH + "/" + depABName + AppConst.AB_EXT_NAME);
				
			if (null != depAB) 
			{
				s_abMaps.Add(depABName, depAB);
			} 
			else 
				Utility.LogError("ABManager: load dep ab file failed:" + depABName + AppConst.AB_EXT_NAME);
        }
*/
        ab = AssetBundle.LoadFromFile(AppConst.PERSISTENT_PATH + "/" + abName_ + AppConst.AB_EXT_NAME);
        if (null == ab) 
            ab = AssetBundle.LoadFromFile(AppConst.STREAMING_PATH + "/" + abName_ + AppConst.AB_EXT_NAME);
		
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
