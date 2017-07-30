
using UnityEngine;

public static class GlobalRef 
{
	private static Transform s_uiRoot = null; 
	private static Transform s_sceneRoot = null;
	  
	public static Transform UIRoot
    {
		get { return s_uiRoot; }
		set { s_uiRoot = value; }
    }

	public static Transform SceneRoot
    {
		get { return s_sceneRoot; }
        set { s_sceneRoot = value; }
    }


	private static AssetMgr s_AssetMgr = null;
	public static AssetMgr AssetMgr 
	{
		get { return s_AssetMgr; }
	}


	public static bool Init()
	{
		s_uiRoot = GameObject.Find("UIRoot/UICanvas").transform;
		if (null == s_uiRoot)
			return false;
		
		s_sceneRoot = GameObject.Find("SceneRoot/SceneCanvas").transform;
		if(null == s_sceneRoot)
			return false;

		GameObject abGo = GameObject.Find ("AssetMgr").gameObject;
		if (null == abGo)
			return false;
	
		s_AssetMgr = abGo.GetComponent<AssetMgr>();
		if (null == s_AssetMgr)
			return false;

		GameObject.DontDestroyOnLoad (abGo);

		return true;
	}

    /*
	private void InitRoleData()
	{
		if(HasKey ("AccountId"))
		{
			SceneWelcome.s_accoutId = GetInt ("AccountId");
		}
		else
		{
			SceneWelcome.s_accoutId = NGUITools.RandomRange (10000, 20000);
			SetInt("AccountId", SceneWelcome.s_accoutId);
		}

		if(HasKey ("CreateTime_" + SceneWelcome.s_accoutId))
		{
			s_ri.createTime = GetInt("CreateTime_" + SceneWelcome.s_accoutId);
		}
		else
		{
			s_ri.createTime = SceneLevel.TotalSeconds();
			SetInt("CreateTime_" + SceneWelcome.s_accoutId, s_ri.createTime);
		}

		if(HasKey("CurrentExp_" + SceneWelcome.s_accoutId))
		{
			s_ri.currentExp = GetInt("CurrentExp_" + SceneWelcome.s_accoutId);
		}
		else
		{
			s_ri.currentExp = 0;
			SetInt("CurrentExp_" + SceneWelcome.s_accoutId, s_ri.currentExp);
		}

		if(HasKey("CurrentGold_" + SceneWelcome.s_accoutId))
		{
			s_ri.currentGold = GetInt("CurrentGold_" + SceneWelcome.s_accoutId);
		}
		else
		{
			s_ri.currentGold = 1000;
			SetInt("CurrentGold_" + SceneWelcome.s_accoutId, s_ri.currentGold);
		}

		if(HasKey("CurrentDiamond_" + SceneWelcome.s_accoutId))
		{
			s_ri.currentDiamond = GetInt("CurrentDiamond_" + SceneWelcome.s_accoutId);
		}
		else
		{
			s_ri.currentDiamond = 0;
			SetInt("CurrentDiamond_" + SceneWelcome.s_accoutId, s_ri.currentDiamond);
		}

		if(HasKey("CurrentPower_" + SceneWelcome.s_accoutId))
		{
			s_ri.currentPower = GetInt("CurrentPower_" + SceneWelcome.s_accoutId);
		}
		else
		{
			s_ri.currentPower = 5;
			SetInt("CurrentPower_" + SceneWelcome.s_accoutId, s_ri.currentPower);
		}

		if(HasKey("PowerCountdownTime_" + SceneWelcome.s_accoutId))
		{
			s_ri.powerCountdownTime = GetInt("PowerCountdownTime_" + SceneWelcome.s_accoutId);
		}
		else
		{
			s_ri.powerCountdownTime = 0;
			SetInt("PowerCountdownTime_" + SceneWelcome.s_accoutId, s_ri.powerCountdownTime);
		}

		if(HasKey("CurrentAnimal_" + SceneWelcome.s_accoutId))
		{
			s_ri.currentAnimal = (EAnimal)GetInt("CurrentAnimal_" + SceneWelcome.s_accoutId);
			s_ri.currentAnimal = EAnimal.ANIMAL_CAT;
		}
		else
		{
			s_ri.currentAnimal = EAnimal.ANIMAL_CAT;
			SetInt("CurrentAnimal_" + SceneWelcome.s_accoutId, (byte)s_ri.currentAnimal);
		}
	}

	public static string AbsFilePath(string fileName_)
	{
		return Application.streamingAssetsPath + "/Config/" + fileName_;
	}
	
	void Update()
	{
		if(Application.platform == RuntimePlatform.Android)
		{
			if(Input.GetKey(KeyCode.Escape))
			{
				Utility.Log("Escape key clicked, app exit!");
				Application.Quit();
			}
		}
	}
    */
	public static bool HasKey(string key_)
	{
		return PlayerPrefs.HasKey (key_);
	}

	public static string GetString(string key_)
	{
		return PlayerPrefs.GetString (key_);
	}
	public static int GetInt(string key_)
	{
		return PlayerPrefs.GetInt(key_);
	}
	public static float GetFloat(string key_)
	{
		return PlayerPrefs.GetFloat(key_);
	}

	public static void SetString(string key_, string value_)
	{
		PlayerPrefs.SetString(key_, value_);
	}

	public static void SetInt(string key_, int value_)
	{
		PlayerPrefs.SetInt(key_, value_);
	}

	public static void SetFloat(string key_, float value_)
	{
		PlayerPrefs.SetFloat(key_, value_);
	}
}
