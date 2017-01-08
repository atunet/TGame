using System;

public static class Utility
{
    public static void Log(string text_)
    {
        UnityEngine.Debug.Log(DateTime.Now.ToString() + " : " + text_);
    }

    public static void LogWarning(string text_)
    {
        UnityEngine.Debug.LogWarning(DateTime.Now.ToString() + " : " + text_);
    }

    public static void LogError(string text_)
    {
        UnityEngine.Debug.LogError(DateTime.Now.ToString() + " : " + text_);
    }
}
