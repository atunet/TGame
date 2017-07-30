
public class Singleton<T> where T : class, new()
{
    public static T Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new T();
            }
            return instance;
        }
    }

    public static void DelInstance()
    {
        instance = null;
    }

    private static T instance;
}
