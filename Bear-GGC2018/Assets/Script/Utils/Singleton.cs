using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            _instance = FindObjectOfType<T>();
            if (_instance == null)
            {
                //Debug.LogError("Singleton <" + typeof(T) + "> does not exists in the hierarchy.");
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {//youbeach 
            if (!Application.isEditor)
            {
                Debug.LogError(this + " You have two singletons of the same type in the hierarchy");
                Destroy(this.gameObject);
            }
        }
    }
}