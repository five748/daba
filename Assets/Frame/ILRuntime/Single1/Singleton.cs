using System;

public class Singleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)Activator.CreateInstance(typeof(T), true);
            }

            return _instance;
        }
    }
    
    public void Clear() {
        _instance = default(T);
    }
}
