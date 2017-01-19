using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class InitLoadingController : MonoBehaviour 
{
    public static string s_sceneName;
    public static void setNextScene(string name_)
    {
        s_sceneName = name_;
    }

    private Slider m_slider;
    private AsyncOperation m_asyc;
   
	// Use this for initialization
	void Start () 
    {
	    // TODO do scene init in lua script ...
        Debug.Log("Loading Controller started");
        // TODO find the slider gameobject
        m_slider = GameObject.Find("Slider").GetComponent<Slider>();
        // start coroutine to load scene
        StartCoroutine(LoadScene());
	}
	
    IEnumerator LoadScene()
    {
        m_asyc = SceneManager.LoadSceneAsync(s_sceneName);
        m_asyc.allowSceneActivation = true;
        yield return m_asyc;
    }

	// Update is called once per frame
	void Update () 
    {
        if (m_slider)
        {
            m_slider.value = m_asyc.progress;
        }
	}
}
