using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    
    private static T _instance;
	private static readonly object _lock = new object();
	public static T instance
    {
		get
		{
			// Check if instance is null
			if (_instance == null)
			{
				// Lock to ensure thread safety
				lock (_lock)
				{
					// Search for an existing instance
					_instance = FindObjectOfType<T>();

					// If none found, create a new instance
					if (_instance == null)
					{
						var singletonObject = new GameObject(typeof(T).ToString());
						_instance = singletonObject.AddComponent<T>();
						DontDestroyOnLoad(singletonObject); // Optional: to persist across scenes
					}
				}
			}
			return _instance;
		}
	}
	protected virtual void Awake()
	{
		// Ensure that there is only one instance
		if (_instance == null)
		{
			//give the current reference to instance of the class T to _instance, only if it is of the class T.
			_instance = this as T;
			DontDestroyOnLoad(gameObject); // Optional: to persist across scenes
		}
		else if (_instance != this)
		{
			Destroy(gameObject); // Destroy duplicate instance
		}
	}
}
