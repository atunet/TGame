using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class InitAppController : MonoBehaviour 
{
	private static bool s_updateChecked = false;
	public  static void setResChecked(bool checked_) 
    { 
        s_updateChecked = checked_; 
    }

	void Start () 
	{
        AppConst.PrintPath();

		if (!GlobalRef.Init ()) 
		{
			Utility.LogError ("Globalref init failed");
			return;
		}

        StartCoroutine (GlobalRef.ABController.GetAB (AppConst.AB_COMMON, CreateCommonUI));
	}

    public void CreateCommonUI(AssetBundle ab_)
    {
        GameObject reconnectPrefab = ab_.LoadAsset("panel_reconnecting") as GameObject;
        if (null == reconnectPrefab)
        {
            Utility.LogError("reconnectprefab not found");
            return;
        }
        GameObject reconnectGo = GameObject.Instantiate(reconnectPrefab);
        reconnectGo.transform.SetParent(GlobalRef.UIRoot, false);
        reconnectGo.name = "panel_reconnecting";
        reconnectGo.SetActive(false);

        StartCoroutine (GlobalRef.ABController.GetAB (AppConst.AB_LOGIN, CheckUpdate));
    }

	public void CheckUpdate(AssetBundle ab_)
	{
		GameObject loginPrefab = ab_.LoadAsset ("panel_login") as GameObject;
		if (null == loginPrefab) 
        {
            Utility.LogError ("loginprefab not found");
			return;
		}			
		GameObject loginRootGo = GameObject.Instantiate (loginPrefab);
		loginRootGo.transform.SetParent (GlobalRef.UIRoot, false);
		loginRootGo.name = "panel_login";

		if (s_updateChecked) 
		{
			loginRootGo.transform.FindChild ("ResUpdate").gameObject.SetActive(false);
            loginRootGo.transform.FindChild ("IptAccount").gameObject.SetActive(true);

			GameObject loginBtnGo = loginRootGo.transform.FindChild ("BtnLogin").gameObject;
			Button loginBtn = loginBtnGo.GetComponent<Button> ();
			loginBtn.onClick.AddListener (OnLoginClick);
			loginBtnGo.SetActive (true);
		} 
	}

	public void OnLoginClick()
	{
        GameObject inputGo = GlobalRef.UIRoot.FindChild("panel_login/IptAccount/Text").gameObject;
        Text accountText = inputGo.GetComponent<Text> ();
        Utility.Log("account:" + accountText.text);
       
		if (NetController.Instance.Init())
		{
			HandleMgr.Init();
            NetController.Instance.LoginToLoginServer("121.199.48.63", 4444, accountText.text, 3999);
            //NetController.Instance.LoginToLoginServer("192.168.0.75", 4444, accountText.text, 3999);
		}
	}

    public void OnDestroy()
    {
		Utility.Log("init app controller destroy");
    }
}
