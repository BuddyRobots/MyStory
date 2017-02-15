using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum ModelType
{
	Mouse,
	Ball,
	Garland
}

public enum Level
{
	One,
	Two,
	Three,
	Four,
	Five,
	Six,
	Seven,
	Eight,
	Nine
}


public class Manager : MonoBehaviour 
{
	public static bool musicOn=true;

	[HideInInspector]
	public AudioSource bgAudio;

	public static ModelType modelType;
	public static Level level;

	void Start () 
	{

		Manager.musicOn=true;

		bgAudio=GameObject.Find("Manager").GetComponent<AudioSource>();

		GameObject.DontDestroyOnLoad(gameObject);

	}
	

	void Update () 
	{

		if (Manager.musicOn)
		{

			if (!bgAudio.isPlaying) 
			{
				bgAudio.Play ();
			}
		}
		if (!Manager.musicOn )
		{

			//关闭音乐 
			if (bgAudio.isPlaying) 
			{
				Debug.Log("pause");
				bgAudio.Pause ();
			}
		}

	}
}
