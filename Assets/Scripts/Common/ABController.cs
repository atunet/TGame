
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ABController : MonoBehaviour
{
    private Dictionary<string, AssetBundle> s_abMaps = new Dictionary<string, AssetBundle>();

	public IEnumerator GetAB(string abName_, System.Action<AssetBundle> cb_)
    {
        AssetBundle ab = null;
		if (!s_abMaps.TryGetValue (abName_, out ab)) 
		{
			AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(Utility.GetResourcePath (abName_));
			yield return request;

			ab = request.assetBundle;
			if (null != ab) 
			{
				Utility.Log ("ABManager load ab file ok:" + abName_);
				s_abMaps [abName_] = ab;
			}
			else
			{
				Utility.LogError ("ABManager load ab file failed:" + abName_);
				yield break;
			}
		}

		if(null != cb_) cb_ (ab);
    }

    public void UnloadAB(string abName_)
    {
        AssetBundle ab = null;
        if (s_abMaps.TryGetValue(abName_, out ab))
        {
			Resources.UnloadAsset(ab);
            Resources.UnloadUnusedAssets();

            s_abMaps.Remove(abName_);
        }
    }

    public void UnloadAll()
    {
		foreach(KeyValuePair<string, AssetBundle> kv in s_abMaps)
		{
			Resources.UnloadAsset(kv.Value);
			Utility.Log("ABManager unload all ab resources:" + kv.Key);
		}

		s_abMaps.Clear();
		Resources.UnloadUnusedAssets();
	}
}
