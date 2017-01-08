using UnityEngine;
using System.Collections;

public class ZoneListController : MonoBehaviour 
{
	public static string s_zoneListURL = "";

	// Use this for initialization
	void Start () 
	{
        Debug.Log("zone list controller start");
		StartCoroutine(LoadZoneList());
	}

	private IEnumerator LoadZoneList()
	{
		/*WWW w = new WWW(s_zoneListURL);
        yield return w;

        if(!string.IsNullOrEmpty(w.error))
        {
            Debug.LogError("www load zone list failed:" + w.error);
		}
        w.Dispose();
        w = null;
        */
        yield return new WaitForSeconds(0.5f);
		//NetController.Instance.ServerIP = "121.199.48.63";
		//NetController.Instance.ServerPort = 8888;

		//NetController.Instance.ServerIP = "119.15.139.149";
		//NetController.Instance.ServerPort = 4444;

		ShowLoginBtn();
	}

	private void ShowLoginBtn()
	{
        GameObject loginBtnPrefab = ABManager.get(AppConst.AB_LOGIN).LoadAsset ("login_btn") as GameObject;
		if (null == loginBtnPrefab)
		{
            Debug.LogError("ShowLoginBtn failed,loginBtnPrefab load failed");
            return;
		}

		GameObject loginBtnGo = GameObject.Instantiate (loginBtnPrefab);
		loginBtnGo.transform.SetParent(GlobalRef.UIRoot, false);
        //loginBtnGo.transform.localPosition = new Vector3(0f, -425f, 0f);
        //loginBtnGo.transform.localScale = new Vector3(1f, 1f, 1f);

        Destroy(this.gameObject);
		//BGMController btm = BGMController.Instance;
	}

    public void OnDestroy()
    {
        Debug.Log("zone list controller destroy");
    }
}
