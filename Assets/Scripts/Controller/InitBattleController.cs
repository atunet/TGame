using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InitBattleController : MonoBehaviour 
{
    void Start () 
    {
        if (!GlobalRef.Init())
            return;

        CreateCommonUI();

        StartCoroutine (GlobalRef.ABController.GetAB (AppConst.AB_MAIN, CreateBattleUI));
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

    public void CreateBattleUI(AssetBundle ab_)
    {
        GameObject battleuiPrefab = ab_.LoadAsset("panel_battle_ui") as GameObject;
        if (null == battleuiPrefab)
        {
            Utility.LogError("battleuiprefab not found");
            return;
        }
        GameObject battleuiGo = GameObject.Instantiate(battleuiPrefab);
        battleuiGo.transform.SetParent(GlobalRef.UIRoot, false);
        battleuiGo.name = "panel_battleui";
        //battleuiGo.SetActive(true);

        GameObject quitBtnGo = battleuiGo.transform.FindChild ("btn_pause").gameObject;
        Button quitBtn = quitBtnGo.GetComponent<Button> ();
        quitBtn.onClick.AddListener (OnQuitBattleClick);
    }


    public void OnQuitBattleClick()
    {
        Utility.Log("OnQuitBattleClicked");
        Utility.LoadingScene("MainScene");
    }
}
