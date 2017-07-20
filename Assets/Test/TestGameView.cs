using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TestGameView : MonoBehaviour {

    private Button m_btn;
    private InputField m_ipt;
	// Use this for initialization

    void Awake()
    {
        m_btn = transform.FindChild("Button").GetComponent<Button>();
        m_ipt = transform.FindChild("InputField").GetComponent<InputField>();

    }

	void Start () {
if (null == m_btn)
            Utility.LogWarning("btn is null");

        if (null == m_ipt)
            Utility.LogWarning("ipt is null");
	}
	
    public void UpdateEvent(ref TestGameData data)
    {
        m_ipt.text = "Name:" + data.playerName +
        ",level:" + data.playerLevel +
        ",exp:" + data.playerExp;
    }

    public void AddBtnEvent(UnityAction action)
    {
        m_btn.onClick.AddListener(action);
    }
}
