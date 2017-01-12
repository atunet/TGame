using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class InitAppController : MonoBehaviour 
{
	private static bool s_updateChecked = false;
	public  static void setResChecked(bool checked_) { s_updateChecked = checked_; }

	private InputField m_accountField = null;

	void Start () 
	{
        AppConst.PrintPath();

		if (!GlobalRef.Init ()) 
		{
			Utility.LogError ("Globalref init failed");
			return;
		}

		StartCoroutine (LoadLoginAB ());
	}


	public IEnumerator LoadLoginAB()
	{
		WWW loginWWW = new WWW(Utility.GetResourcePath (AppConst.AB_LOGIN));
		yield return loginWWW;

		if (string.IsNullOrEmpty (loginWWW.error)) 
		{
			AssetBundle loginAB = loginWWW.assetBundle;

			GameObject loginPrefab = loginAB.LoadAsset ("panel_login") as GameObject;
			if (null == loginPrefab) {
				Debug.LogError ("loginprefab not found");
				return;
			}			
			GameObject loginRootGo = GameObject.Instantiate (loginPrefab);
			loginRootGo.transform.SetParent (GlobalRef.UIRoot, false);
			loginRootGo.name = "panel_login";

			if (s_updateChecked) {
				loginRootGo.transform.FindChild ("ResUpdate").gameObject.SetActive (false);

				GameObject accountInputGo = loginRootGo.transform.FindChild ("IptAccount").gameObject;
				m_accountField = accountInputGo.GetComponent<InputField> ();
				accountInputGo.SetActive (true);

				GameObject loginBtnGo = loginRootGo.transform.FindChild ("BtnLogin").gameObject;
				Button loginBtn = loginBtnGo.GetComponent<Button> ();
				loginBtn.onClick.AddListener (OnLoginClick);
				loginBtnGo.SetActive (true);
			} else {
				GameObject resUpdateGo = new GameObject ("ResUpdate");
				resUpdateGo.transform.SetParent (GlobalRef.UIRoot, false);
				resUpdateGo.AddComponent<ResUpdateController> ().m_slider = null;
			}
		} 
		else 
		{
		}
	}

	public void OnLoginClick()
	{
		ulong accountId = ulong.Parse(m_accountField.text);
		Utility.Log ("login btn clicked:" + accountId);

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
