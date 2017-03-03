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
				Debug.Log("场景单利---");
				GameObject go = new GameObject ();
				Debug.Log(go.name);
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
