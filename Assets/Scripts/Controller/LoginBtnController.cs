using UnityEngine;
using System.Collections;

public class LoginBtnController : MonoBehaviour 
{
    void Start()
    {
        Debug.Log("login controller start");
    }

	public void OnLoginClick()
	{   
     /*   GameObject loadingPrefab = ABManager.get(AppConst.AB_LOGIN).LoadAsset ("LoadingPrefab") as GameObject;
		if (null == loadingPrefab)
		{
            Debug.LogError("load loadingprefab failed");
            //return;
		}
*/
		//GameObject loadingGo = GameObject.Instantiate (loadingPrefab);
		//loadingGo.transform.SetParent (this.transform.parent);

		// lua init: load the config data and cached role data ...
	//	if(MsgHandler.Instance.Init())
		{
            if (NetController.Instance.Init())
            {
                HandleMgr.Init();
                NetController.Instance.LoginToLoginServer("121.199.48.63", 8888);
                //NetController.Instance.LoginToLoginServer("192.168.0.75", 4444);
            }
		}
	}

    public void OnDestroy()
    {
        Debug.Log("login controller destroy");
    }
}
