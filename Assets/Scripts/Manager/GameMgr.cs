using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    public bool AssetBundleMode = false;

    // Use this for initialization
    void Start () 
    {
        string abName = "";
        string assetName = "";
        AssetMgr.Instance.GetAsset(abName, assetName, ShowLoginBG);

        abName = "";
        assetName = "";
        AssetMgr.Instance.GetAsset(abName, assetName, ShowPatchUI);
    }
	
    public void ShowLoginBG(GameObject prefab)
    {
    }

    public void ShowPatchUI(GameObject prefab)
    {
        // ...
        PatchMgr.Instance;
    }
}
