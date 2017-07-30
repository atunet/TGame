
using UnityEngine;

public class SingletonMB<T> : MonoBehaviour where T : Component
{
    public static T Instance
    {
        get
        {
            if (null == instance)
            {
                GameObject go = new GameObject(typeof(T).ToString());
                instance = go.AddComponent<T>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    private static T instance;
}