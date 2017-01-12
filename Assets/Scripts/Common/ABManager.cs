
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

public class ABManager : MonoBehaviour
{
    private Dictionary<string, AssetBundle> s_abMaps = new Dictionary<string, AssetBundle>();

	public IEnumerator get(string abName_, Action<AssetBundle> cb_)
    {
        AssetBundle ab = null;
		if (!s_abMaps.TryGetValue (abName_, out ab)) 
		{
			WWW loadWWW = new WWW (Utility.GetResourcePath (abName_));
			yield return loadWWW;
			if (string.IsNullOrEmpty (loadWWW.error))
			{
				Utility.Log ("abmanager: www load ok" + abName_);

				ab = loadWWW.assetBundle;
				s_abMaps [abName_] = ab;
			}
			else
			{
				Utility.LogError ("abmanager: www load failed" + abName_ + ",error:" + loadWWW.error);
				yield return 0;
			}
		}

		cb_ (ab);
    }

    public void UnloadAB(string abName_)
    {
        AssetBundle ab = null;
        if (s_abMaps.TryGetValue(abName_, out ab))
        {
            Resource.UnloadAsset(ab);
            Resources.UnloadUnusedAssets();

            s_abMaps.Remove(abName_);
        }
    }

    public void UnloadAll()
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
