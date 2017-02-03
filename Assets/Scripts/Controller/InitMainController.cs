using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InitMainController : MonoBehaviour 
{
	void Start () 
    {
        if (!GlobalRef.Init())
            return;

        CreateCommonUI();

        StartCoroutine (GlobalRef.ABController.GetAB (AppConst.AB_MAIN, CreateMainUI));
	}

    public void CreateCommonUI()
    {
        AssetBundle commonAB = GlobalRef.ABController.GetAB(AppConst.AB_COMMON);
        GameObject reconnectPrefab = commonAB.LoadAsset("panel_reconnecting") as GameObject;
        if (null == reconnectPrefab)
        {
            Utility.LogError("reconnectprefab not found");
            return;
        }
        GameObject reconnectGo = GameObject.Instantiate(reconnectPrefab);
        reconnectGo.transform.SetParent(GlobalRef.UIRoot, false);
        reconnectGo.name = "panel_reconnecting";
        reconnectGo.SetActive(false);
    }

    public void CreateMainUI(AssetBundle ab_)
    {
        GameObject mainuiPrefab = ab_.LoadAsset("panel_main_ui") as GameObject;
        if (null == mainuiPrefab)
        {
            Utility.LogError("mainuiprefab not found");
            return;
        }
        GameObject mainuiGo = GameObject.Instantiate(mainuiPrefab);
        mainuiGo.transform.SetParent(GlobalRef.UIRoot, false);
        mainuiGo.name = "panel_mainui";
        //mainuiGo.SetActive(true);

        GameObject pveBtnGo = mainuiGo.transform.FindChild ("btn_pve").gameObject;
        Button pveBtn = pveBtnGo.GetComponent<Button> ();
        pveBtn.onClick.AddListener (OnPveClick);

        GameObject pvpBtnGo = mainuiGo.transform.FindChild ("btn_pvp").gameObject;
        Button pvpBtn = pvpBtnGo.GetComponent<Button> ();
        pvpBtn.onClick.AddListener (OnPvpClick);

        //StartCoroutine (GlobalRef.ABController.GetAB (AppConst.AB_MAIN, CreateMainUI));
    }

    public void OnPveClick()
    {
        Utility.Log("OnPveClicked");
        Utility.LoadingScene("BattleScene");
    }

    public void OnPvpClick()
    {
        Utility.Log("OnPvpClicked");
        Utility.LoadingScene("BattleScene");
    }
}
