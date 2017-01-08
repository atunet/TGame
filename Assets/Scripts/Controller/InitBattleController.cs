using UnityEngine;
using System.Collections;

public class InitBattleController : MonoBehaviour 
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

        //LuaBehaviour.LuaFileName = "InitBattleBehaviour";
        //gameObject.AddComponent<LuaBehaviour>();
    }
}
