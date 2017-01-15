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

		StartCoroutine (GlobalRef.ABController.GetAB (AppConst.AB_LOGIN, CheckUpdate));
	}


	public void CheckUpdate(AssetBundle ab_)
	{
		GameObject loginPrefab = ab_.LoadAsset ("panel_login") as GameObject;
		if (null == loginPrefab) 
        {
			Debug.LogError ("loginprefab not found");
			return;
		}			
		GameObject loginRootGo = GameObject.Instantiate (loginPrefab);
		loginRootGo.transform.SetParent (GlobalRef.UIRoot, false);
		loginRootGo.name = "panel_login";

		if (s_updateChecked) 
		{
			loginRootGo.transform.FindChild ("ResUpdate").gameObject.SetActive (false);
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
        Debug.Log("account:" + accountText.text);
       
        ulong accountId = ulong.Parse(accountText.text);
		//Utility.Log ("login btn clicked:" + accountId);

		if (NetController.Instance.Init())
		{
			HandleMgr.Init();
            NetController.Instance.LoginToLoginServer("121.199.48.63", 8888, accountId);
			//NetController.Instance.LoginToLoginServer("192.168.0.75", 4444, accoundId);
		}
	}

    public void OnDestroy()
    {
		Utility.Log("init app controller destroy");
    }
}
