
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class AssetMgr : SingletonMB<AssetMgr>
{
    private bool abMode = false;

    private Dictionary<string, AssetBundle> m_abMaps = new Dictionary<string, AssetBundle>();
    private ArrayList m_abNameList;

    public AssetBundle TryGetAB(string abName_)
    {
        AssetBundle ab = null;
        m_abMaps.TryGetValue(abName_, out ab); 
        return ab;
    }

    public void GetAsset<T>(string abName, string assetName, System.Action<T> callback) where T : Object
    {
#if UNITY_EDITOR
        if (abMode)
        {
            StartCoroutine(LoadAssetBundleAsync(abName, assetName, callback));
        }
        else
        {
            LoadLocalAsset(assetName, callback);
        }
#else
        StartCoroutine(LoadAssetBundleAsync(abName, assetName, callback));
#endif
    }

    public void GetAsset<T>(string[] abList, string assetName, System.Action<T> callback) where T : Object
    {
#if UNITY_EDITOR
        if (abMode)
        {
            StartCoroutine(LoadAssetBundleAsync(abList, assetName, callback));
        }
        else
        {
            LoadLocalAsset(assetName, callback);
        }
#else
        StartCoroutine(LoadAssetBundleAsync(abList, assetName, callback));
#endif
    }

    public IEnumerator LoadAssetBundleAsync<T>(string abName, string assetName, System.Action<T> callback) where T : Object
    {
        AssetBundle ab = null;
        if (!m_abMaps.TryGetValue(abName, out ab))
        {
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(Utility.GetResourcePath(abName));
            yield return request;

            ab = request.assetBundle;
            if (null != ab)
            {
                Utility.Log("[AssetMgr] load ab file ok:" + abName);
                m_abMaps[abName] = ab;
            }
            else
            {
                Utility.LogError("[AssetMgr] load ab file failed:" + abName);
                yield return false;
            }
        }

        if (assetName.Length > 0 && callback != null)
        {
            T assetObject = ab.LoadAsset<T>(assetName);
            if (null != assetObject)
            {
                callback.DynamicInvoke(assetObject);
            }
            else
            {
                Utility.LogError("[AssetMgr] load asset failed:" + assetName);
            }
        }
    }

    public IEnumerator LoadAssetBundleAsync<T>(string[] abList, string assetName, System.Action<T> callback) where T : Object
    {
        AssetBundle ab = null;
        for(int i = 0; i < abList.Length; ++i)
        {
            string abName = abList[i];
            if (!m_abMaps.TryGetValue(abName, out ab))
            {
                AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(Utility.GetResourcePath (abName));
                yield return request;

                ab = request.assetBundle;
                if (null != ab)
                {
                    Utility.Log("[AssetMgr] load ab file ok:" + abName);
                    m_abMaps[abName] = ab;
                }
                else
                {
                    Utility.LogError("[AssetMgr] load ab file failed:" + abName);
                    yield return false;
                }
            }
        }
       
        if (assetName.Length > 0 && callback != null)
        {
            T assetObject = ab.LoadAsset<T>(assetName);
            if (null != assetObject)
            {
                callback.DynamicInvoke(assetObject);
            }
            else
            {
                Utility.LogError("[AssetMgr] load asset failed:" + assetName);
            }
        }
    }

    public void LoadLocalAsset<T>(string assetName, System.Action<T> callback) where T : Object
    {
        if (assetName.Length > 0 && callback != null)
        {
            T assetObject = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetName);
            if (null != assetObject)
            {
                callback.DynamicInvoke(assetObject);
            }
            else
            {
                Utility.LogError("[AssetMgr] load local asset failed:" + assetName);
            }
        }
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
            string abName = abList_[i];
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
                    yield return false;
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
