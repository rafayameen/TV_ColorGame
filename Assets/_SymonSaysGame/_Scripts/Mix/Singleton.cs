using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T sInstance = null;

    public static T Instance
    {
        get
        {
            return sInstance;
        }
    }

    protected virtual void Awake()
    {
        if (sInstance == null)
        {
            sInstance = (T)this;
            DontDestroyOnLoad(gameObject);
        }
        else if (sInstance != this)
        {
            Destroy(gameObject);
        }
    }
}
