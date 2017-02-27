using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// singlton in a game
/// </summary>
public class AllSceneSinglton<T>:MonoBehaviour
	where T:Component
{
	private static T _instance;
	public static T Instance
	{
		get {
			if (_instance == null) 
			{
				GameObject go = new GameObject ();
				go.hideFlags = HideFlags.HideAndDontSave;
				DontDestroyOnLoad (go);
				_instance = (T)go.AddComponent(typeof(T));
				return _instance;
			} 
			else 
			{
				return _instance;
			}
		}
	}
}
