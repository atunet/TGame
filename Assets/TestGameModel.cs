using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TestGameData
{
    public string playerName;
    public byte playerLevel;
    public uint playerExp;
}
    
public delegate void TestGameUpdateEvent(ref TestGameData data);

public class TestGameModel : MonoBehaviour {

    private TestGameData m_data;
    public TestGameUpdateEvent m_event = null;

    public void Start()
    {
        m_data = new TestGameData();
    }

    public string PlayerName
    {
        get { return m_data.playerName; }
        set { 
            m_data.playerName = value; 
            m_event(ref m_data);
        }
    }

    public byte PlayerLevel
    {
        get { return m_data.playerLevel; }
        set { 
            m_data.playerLevel = value; 
            m_event(ref m_data);
        }
    }

    public uint PlayerExp
    {
        get { return m_data.playerExp; }
        set { 
            m_data.playerExp = value; 
            m_event(ref m_data);
        }
    }
}
