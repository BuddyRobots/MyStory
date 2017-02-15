using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtitleSynchronizeWithAudio : MonoBehaviour {

	string text;

	public AudioSource audioSource;
	private bool showSubtitle;

	void Start () 
	{
		showSubtitle=false;
		text="有一天，小老鼠在草丛中玩皮球。。。。";
	}


	public void PlayAudio()
	{
		if (!audioSource.isPlaying)
		{
			audioSource.Play();
		}
		else
		{
			audioSource.Stop();
			audioSource.Play();
		}

	}

	void Update()
	{
		if (audioSource.isPlaying) {
			showSubtitle=true;
		}
		else
		{
			showSubtitle=false;
		}
	}

	void OnGUI()
	{
		if (showSubtitle) {
			GUI.Label(new Rect(0,Screen.height -64,Screen .width,64),text);
		}

	}
}
