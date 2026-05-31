using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    public static T Instance = null;
    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = gameObject.GetComponent<T>();
        }
    }
}
