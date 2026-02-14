using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance != null) return _instance;
            
            _instance = FindFirstObjectByType<T>();
            if (_instance != null) return _instance;

            GameObject gameObject = new(typeof(T).Name);
            _instance = gameObject.AddComponent<T>();
            return _instance;
        }
    }
}