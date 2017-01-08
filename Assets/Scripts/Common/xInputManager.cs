using UnityEngine;

public class xInputManager 
{
    static public Quaternion GetWorldRotation(float h_, float v_)
    {
        //float h = GetHorizontalValue();
        //float v = GetVerticalValue();
        float upRotation = Vector2.Angle(Vector2.up, new Vector2(h_, v_));
        if (h_ < 0f) upRotation = 360f - upRotation;
        return Quaternion.Euler(0f, upRotation, 0f);
    }

    static public void SetHorizontalValue(float val, bool use)
    {
        s_horizontalValue = val;
        s_useJoystick = use;
    }

    static public float GetHorizontalValue()
    {
        if (s_useJoystick)
        {
            return s_horizontalValue;
        }
        else
        {
            return Input.GetAxis("Horizontal");
        }
    }

    static public float GetHorizontalValueRaw()
    {
        if (s_useJoystick)
        {
            return (s_horizontalValue >= 0) ? Mathf.Ceil(s_horizontalValue) : Mathf.Floor(s_horizontalValue);
        }
        else
        {
            float h = Input.GetAxis("Horizontal");
            return (h >= 0) ? Mathf.Ceil(h) : Mathf.Floor(h);
        }
    }

    static public void SetVerticalValue(float val, bool use)
    {
        s_verticalValue = val;
        s_useJoystick = use;
    }

    static public float GetVerticalValue()
    {
        if (s_useJoystick)
        {
            return s_verticalValue;
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }

    static public float GetVerticalValueRaw()
    {
        if (s_useJoystick)
        {
            return (s_verticalValue >= 0) ? Mathf.Ceil(s_verticalValue) : Mathf.Floor(s_verticalValue);
        }
        else
        {
            float v = Input.GetAxis("Vertical");
            return (v >= 0) ? Mathf.Ceil(v) : Mathf.Floor(v);
        }
    }

    static private bool s_useJoystick = false;
    static private float s_horizontalValue = 0.0f;
    static private float s_verticalValue = 0.0f;
}
