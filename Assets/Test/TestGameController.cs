using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameController : MonoBehaviour {

    private TestGameModel m_model;
    private TestGameView m_view;

    void Awake()
    {
        m_view = GetComponent<TestGameView>();
        m_model = GetComponent<TestGameModel>();
        if (null == m_view)
            Utility.LogWarning("view is null");

        if (null == m_model)
            Utility.LogWarning("model is null");
    }

	// Use this for initialization
	void Start () {
        m_view = GetComponent<TestGameView>();
        m_model = GetComponent<TestGameModel>();

        m_view.AddBtnEvent(OnButtonClick);
        m_model.m_event = m_view.UpdateEvent;
        ModelInit();
	}
	
    void ModelInit()
    {
        m_model.PlayerExp = 0;
        m_model.PlayerLevel = 1;
        m_model.PlayerName = "frank999";
    }

    public void OnButtonClick()
    {
        m_model.PlayerExp += 30;
        if (m_model.PlayerExp >= m_model.PlayerLevel * 100)
        {
            m_model.PlayerExp = 0;
            m_model.PlayerLevel++;
        }
    }
}
