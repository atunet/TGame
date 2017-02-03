using UnityEngine;
using System.Collections;

public class AccountController : MonoBehaviour 
{
	private static string s_userName = "abc";
	private static string s_userPswd = "123456";

	// Use this for initialization
	void Start () 
	{
        Utility.Log("account controller start");

		StartCoroutine(AccountLogin());
	}

	IEnumerator AccountLogin()
	{
		// TODO ... account reg or account login to other platform ...
        yield return new WaitForSeconds(2f);

		// after account login, load the zone list UI ...
		AssetBundle loginAB = null; //ABManager.get(AppConst.AB_LOGIN);
        if (null != loginAB)
        {
            GameObject zongListPrefab = loginAB.LoadAsset("zone_list") as GameObject;
            if (null != zongListPrefab)
            {
                GameObject zoneListGo = GameObject.Instantiate(zongListPrefab);
				zoneListGo.transform.SetParent(GlobalRef.UIRoot, false);
                Destroy(this.gameObject);   // destroy account controller instance
            }
            else
                Utility.LogError("load asset from login ab failed: zone_list");
        }
	}

    public void OnDestroy()
    {
        Utility.Log("account controller destroy");
    }
}
