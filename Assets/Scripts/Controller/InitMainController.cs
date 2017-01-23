using UnityEngine;
using System.Collections;

public class InitMainController : MonoBehaviour 
{
	void Start () 
    {
        if (!GlobalRef.Init())
            return;

        StartCoroutine(GlobalRef.ABController.GetAB(AppConst.AB_COMMON, CreateCommonUI));
	}

    public void CreateCommonUI(AssetBundle ab_)
    {
        GameObject reconnectPrefab = ab_.LoadAsset("panel_reconnecting") as GameObject;
        if (null == reconnectPrefab)
        {
            Debug.LogError("reconnectprefab not found");
            return;
        }
        GameObject reconnectGo = GameObject.Instantiate(reconnectPrefab);
        reconnectGo.transform.SetParent(GlobalRef.UIRoot, false);
        reconnectGo.name = "panel_reconnecting";
        reconnectGo.SetActive(false);

        StartCoroutine (GlobalRef.ABController.GetAB (AppConst.AB_MAIN, CreateMainUI));
    }

    public void CreateMainUI(AssetBundle ab_)
    {
        GameObject mainuiPrefab = ab_.LoadAsset("panel_main_ui") as GameObject;
        if (null == mainuiPrefab)
        {
            Debug.LogError("mainuiprefab not found");
            return;
        }
        GameObject reconnectGo = GameObject.Instantiate(mainuiPrefab);
        reconnectGo.transform.SetParent(GlobalRef.UIRoot, false);
        reconnectGo.name = "panel_mainui";
        reconnectGo.SetActive(true);

        //StartCoroutine (GlobalRef.ABController.GetAB (AppConst.AB_MAIN, CreateMainUI));
    }

}
