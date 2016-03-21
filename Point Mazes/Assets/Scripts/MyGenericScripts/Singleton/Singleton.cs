using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

	protected static T instance;
	
	/*
	 * Returns the instance of the Singleton
	 */
	public static T Instance {
		get	{
			if (instance == null) { instance = (T) FindObjectOfType(typeof(T)); }
			/*if (instance == null) { 
				Debug.LogError("An instance of " + typeof(T) + 
				               " is needed in the scene, but there is none."); 
			}*/
			return instance;
		}
	}

	protected void Awake() {
		if (instance == null) { instance = this as T; }
		else if (instance != null) { Destroy(this); }
	}
}
