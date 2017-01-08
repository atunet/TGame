using UnityEngine;
using System.Collections;
using System.IO;

public class InitAppController : MonoBehaviour 
{
	private static bool s_resUpdateChecked = false;
	public  static void setResChecked(bool checked_) { s_resUpdateChecked = checked_; }

	void Start () 
	{
        AppConst.PrintPath();

		if (!GlobalRef.Init ())
			return;

	/*	if(!Directory.Exists(AppConst.PERSISTENT_PATH))
        {
			Directory.CreateDirectory(AppConst.PERSISTENT_PATH);
        }

		if(File.Exists(AppConst.PERSISTENT_VERSION_FILE_PATH))
        {
            CheckResUpdate();
        } 
*/

		AssetBundle loginAB = ABManager.get(AppConst.AB_LOGIN);
		if (null == loginAB) 
			return;

		if (s_resUpdateChecked) 
		{
		} 
		else 
		{
			GameObject resUpdatePrefab = loginAB.LoadAsset ("res_update") as GameObject;
			if (null == resUpdatePrefab)
			{
				Debug.LogError("CheckResUpdate failed,resourceupdateprefab not found");
				return;
			}			
			GameObject resUpdateGo = GameObject.Instantiate (resUpdatePrefab);
			resUpdateGo.transform.SetParent(GlobalRef.UIRoot, false);
		}

	}
		
    public void OnDestroy()
    {
        Debug.Log("app start controller destroy");
    }
}
