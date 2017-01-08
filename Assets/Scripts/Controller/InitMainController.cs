using UnityEngine;
using System.Collections;

public class InitMainController : MonoBehaviour 
{
	void Start () 
    {
		GlobalRef.UIRoot = GameObject.Find("UIRoot/UICanvas").transform;
		GlobalRef.SceneRoot = GameObject.Find("SceneRoot/SceneCanvas").transform;
		if (null == GlobalRef.UIRoot || null == GlobalRef.SceneRoot)
        {
            Debug.LogError("uiRoot or sceneRoot not found!!!");
            return;
        }

        NetController.Instance.ReconnectingUI = GameObject.Find("UIRoot/UICanvas/panel_reconnecting").gameObject;

        //LuaBehaviour.LuaFileName = "InitMainBehaviour";
        //gameObject.AddComponent<LuaBehaviour>();
	}
}
