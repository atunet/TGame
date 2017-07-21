using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MyJoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler 
{
    enum EState
    {
        IDLE,
        MOVING,
    };
    EState m_state;

    private RectTransform m_rectTrans;
    private Vector2 m_originPosition;
    private float m_movementRadius;     // 像素单位半径
    private float m_resetSpeed = 10.0f;  // 像素单位回归速度

    void Start () 
    {      
        m_state = EState.IDLE;

        m_rectTrans = GetComponent<RectTransform>();
        m_originPosition = m_rectTrans.anchoredPosition;
        m_movementRadius = m_rectTrans.sizeDelta.x/2;

        Utility.Log("start called,origin position x:" + m_originPosition.x + ",y:" + m_originPosition.y + ",radius:" + m_movementRadius);
    }
	

    void Update()
    {
        if (m_state == EState.IDLE)
        {
            if ((m_rectTrans.anchoredPosition - m_originPosition).magnitude > 0.3f)
            {
                m_rectTrans.anchoredPosition = Vector2.Lerp(m_rectTrans.anchoredPosition, m_originPosition, Time.deltaTime * m_resetSpeed);
            }
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        Utility.Log("OnPointerDown was called");

        m_state = EState.MOVING;

        SetNewPos(data);

        UpdateInputValue(m_rectTrans.anchoredPosition, true);
    }

    public void OnPointerUp(PointerEventData data)
    {
        Utility.Log("OnPointerUp was called");

        m_state = EState.IDLE;

        UpdateInputValue(Vector2.zero, false);
    }

    public void OnDrag(PointerEventData data)
    {
        Utility.Log("OnDrag was called");

        //m_state = EState.MOVING;

        SetNewPos(data);

        UpdateInputValue(m_rectTrans.anchoredPosition, true);
    }

    private void SetNewPos(PointerEventData data)
    {
        Vector3 worldPos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_rectTrans, data.position, data.pressEventCamera, out worldPos))
        {
            m_rectTrans.position = worldPos;
        }
         
        /*  
        // method 1
        Vector2 offset = m_rectTrans.anchoredPosition - m_originPosition;
        if (offset.magnitude > m_movementRadius) // or use vector3.Distance(...)
        {
            offset = offset.normalized * m_movementRadius;
            m_rectTrans.anchoredPosition = offset;
        }
        */
        // method 2
        Vector2 offset2 = m_rectTrans.anchoredPosition - m_originPosition;
        m_rectTrans.anchoredPosition = m_originPosition + Vector2.ClampMagnitude(offset2, m_movementRadius);
    }
        
    private void UpdateInputValue(Vector2 value, bool useJoystick)
    {
        var delta = m_originPosition - value;
        delta.y = -delta.y;
        delta /= m_movementRadius;
       
        xInputManager.SetHorizontalValue(-delta.x, useJoystick);
        xInputManager.SetVerticalValue(delta.y, useJoystick);

        Utility.Log("update input value:" + -delta.x + "," + delta.y);
    }
}
