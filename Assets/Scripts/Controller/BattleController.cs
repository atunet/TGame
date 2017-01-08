using UnityEngine;
using System.Collections;

public class BattleController : MonoBehaviour 
{
    public Animator m_animator;

    private float m_lastAttackTime = 0f;
    private float m_lastSkill1Time = 0f;
    private float m_lastSkill2Time = 0f;
    private float m_lastSkill3Time = 0f;

    public void OnStart()
    {
        m_lastAttackTime = Time.time - 3f;
        m_lastSkill1Time = m_lastAttackTime;
        m_lastSkill2Time = m_lastAttackTime;
        m_lastSkill3Time = m_lastAttackTime;
    }

    public void OnAttackClick()
    {
        Debug.Log("OnAttackClick called");
        if (Time.time - m_lastAttackTime > 1)
        {
            m_lastAttackTime = Time.time;
            m_animator.SetTrigger("Jab");
        }
    }

    public void OnSkill1Click()
    {
        //Debug.Log("OnSkill1Click called");
        if (Time.time - m_lastSkill1Time > 2)
        {
            m_lastSkill1Time = Time.time;
            m_animator.SetTrigger("Spinkick");
        }
    }

    public void OnSkill2Click()
    {
        Debug.Log("OnSkill2Click called");
        if (Time.time - m_lastSkill2Time > 2)
        {
            m_lastSkill2Time = Time.time;
            m_animator.SetTrigger("SAMK");

        }
    }

    public void OnSkill3Click()
    {        
        Debug.Log("OnSkill3Click called");
        if (Time.time - m_lastSkill3Time > 2)
        {           
            m_lastSkill3Time = Time.time;  
            m_animator.SetTrigger("RISING_P");
        }
    }

    public void OnPauseClick()
    {
        Debug.Log("OnPauseClick called");
    }

    public void OnExitClick()
    {
        Debug.Log("OnExitClick called");
    }
}
