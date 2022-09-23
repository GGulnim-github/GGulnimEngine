using UnityEngine;

/// <summary>
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// </summary>
public class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;

                if (_instance == null)
                {
                    _instance = new GameObject($"@{typeof(T)}", typeof(T)).GetComponent<T>();
                }
            }
            return _instance;
        }
    }
    protected virtual void Awake()
    {
        InitializeSingleton();
    }

	protected virtual void InitializeSingleton()
	{
		if (!Application.isPlaying)
		{
			return;
		}

		if (_instance == null)
		{
			_instance = this as T;
			DontDestroyOnLoad(transform.gameObject);
		}
		else
		{
			if (this != _instance)
			{
				Destroy(gameObject);
			}
		}
	}
}

