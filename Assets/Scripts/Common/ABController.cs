
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ABController : MonoBehaviour
{
    private Dictionary<string, AssetBundle> m_abMaps = new Dictionary<string, AssetBundle>();
    private ArrayList m_abNameList;

    public AssetBundle GetAB(string abName_)
    {
        AssetBundle ab = null;
        m_abMaps.TryGetValue(abName_, out ab); 
        return ab;
    }

	public IEnumerator GetAB(string abName_, System.Action<AssetBundle> cb_)
    {
        AssetBundle ab = null;
		if (!m_abMaps.TryGetValue (abName_, out ab)) 
		{
			AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(Utility.GetResourcePath (abName_));
			yield return request;

			ab = request.assetBundle;
			if (null != ab) 
			{
				Utility.Log ("ABManager load ab file ok:" + abName_);
				m_abMaps [abName_] = ab;
			}
			else
			{
				Utility.LogError ("ABManager load ab file failed:" + abName_);
				yield break;
			}
		}

		if(null != cb_) cb_ (ab);
    }

    public IEnumerator GetABList(string[] abList_, System.Action<AssetBundle> cb_)
    {
        AssetBundle ab = null;

        for(int i = 0; i < abList_.Length; ++i)
        {
            string abName = abList_[0];
            if (!m_abMaps.TryGetValue(abName, out ab))
            {
                AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(Utility.GetResourcePath (abName));
                yield return request;

                ab = request.assetBundle;
                if (null != ab)
                {
                    Utility.Log("ABManager load ab file ok:" + abName);
                    m_abMaps[abName] = ab;
                }
                else
                {
                    Utility.LogError("ABManager load ab file failed:" + abName);
                    yield break;
                }
            }
        }

        if(null != cb_) cb_ (ab);
    }

    public void UnloadAB(string abName_)
    {
        AssetBundle ab = null;
        if (m_abMaps.TryGetValue(abName_, out ab))
        {
			Resources.UnloadAsset(ab);
            Resources.UnloadUnusedAssets();

            m_abMaps.Remove(abName_);
        }
    }

    public void UnloadAll()
    {
		foreach(KeyValuePair<string, AssetBundle> kv in m_abMaps)
		{
			Resources.UnloadAsset(kv.Value);
			Utility.Log("ABManager unload all ab resources:" + kv.Key);
		}

		m_abMaps.Clear();
		Resources.UnloadUnusedAssets();
	}
}
