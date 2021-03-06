﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//包括小手的显示
public class BussinessManager : MonoBehaviour 
{

	public static BussinessManager _instance;

	[HideInInspector]
	public GameObject finger;
	private GameObject fingerPrefab;
	private GameObject sceneParent;
	private AudioSource audioSource;
	private Vector3 offSet = new Vector3 (1.3f, -0.7f, 0);//这个值表示小手距离需要点击的对象的距离（对象Position+offset=小手Position）

	void Awake()
	{
		_instance=this;

		fingerPrefab=Resources.Load("Prefab/Finger",typeof(GameObject)) as GameObject;
		sceneParent=GameObject.Find("SceneParent");
		audioSource=GameObject.Find("Main Camera").GetComponent<AudioSource>();
	}


	public void ShowFinger(Vector3 pos)
	{
		if (finger) 
		{
			Destroy (finger);
			finger = null;
		}
		finger = Instantiate (fingerPrefab) as GameObject;
		finger.name="finger";
		finger.transform.parent = sceneParent.transform;
		finger.transform.localScale = Vector3.one;
		finger.GetComponent<FingerCtrl> ().FingerShow (pos + offSet);
	}


	public void PauseStory()
	{
		switch (LevelManager.currentLevelData.LevelID)
		{
		case 1:
			LevelOne._instance.PauseStory();
			break;
		case 2:
			LevelTwo._instance.PauseStory();
			break;
		case 3:
			LevelThree._instance.PauseStory();
			break;
		case 4:
			break;
		case 5:
			LevelFive._instance.PauseStory();
			break;
		case 6:
			LevelSix._instance.PauseStory();
			break;
		case 7:
			LevelSeven._instance.PauseStory();
			break;
		case 8:
			break;
		case 9:
			LevelNine._instance.PauseStory();
			break;
		default:
			break;
		}

	}

	/// <summary>
	/// 点击确定录音按钮，开始场景故事（没有旁白，有字幕，或者还有动画，同时开启录音和录屏）
	/// </summary>
	public void StartStoryToRecordAudioAndVideo()
	{
		switch (LevelManager.currentLevelData.LevelID)
		{
		case 1:
			LevelOne._instance.StartStoryToRecordAudioAndVideo();

			break;
		case 2:
			LevelTwo._instance.StartStoryToRecordAudioAndVideo();
			break;
		case 3:
			LevelThree._instance.StartStoryToRecordAudioAndVideo();
			break;
		case 4:
			break;
		case 5:
			LevelFive._instance.StartStoryToRecordAudioAndVideo();
			break;
		case 6:
			LevelSix._instance.StartStoryToRecordAudioAndVideo();
			break;
		case 7:
			LevelSeven._instance.StartStoryToRecordAudioAndVideo();
			break;
		case 8:
			break;
		case 9:
			LevelNine._instance.StartStoryToRecordAudioAndVideo();
			break;
		default:
			break;
		}

	}

	public void ResumeStory()
	{
		switch (LevelManager.currentLevelData.LevelID)
		{
		case 1:
			LevelOne._instance.ResumeStory();
			break;
		case 2:
			LevelTwo._instance.ResumeStory();
			break;
		case 3:
			LevelThree._instance.ResumeStory();
			break;
		case 4:
			break;
		case 5:
			LevelFive._instance.ResumeStory();
			break;
		case 6:
			LevelSix._instance.ResumeStory();
			break;
		case 7:
			LevelSeven._instance.ResumeStory();
			break;
		case 8:
			break;
		case 9:
			LevelNine._instance.ResumeStory();
			break;
		default:
			break;
		}

	}
		

	public void InitTest()
	{

		switch (LevelManager.currentLevelData.LevelID)
		{
		case 1:
			LevelOne._instance.InitByClickingCloseBtnOfRecordingDoneFrame();
			break;
		case 2:
			LevelTwo._instance.InitByClickingCloseBtnOfRecordingDoneFrame();
			break;
		case 3:
			LevelThree._instance.InitByClickingCloseBtnOfRecordingDoneFrame();
			break;
		case 4:
			break;
		case 5:
			LevelFive._instance.InitByClickingCloseBtnOfRecordingDoneFrame();
			break;
		case 6:
			LevelSix._instance.InitByClickingCloseBtnOfRecordingDoneFrame();
			break;
		case 7:
			LevelSeven._instance.InitByClickingCloseBtnOfRecordingDoneFrame();
			break;
		case 8:
			break;
		case 9:
			LevelNine._instance.InitByClickingCloseBtnOfRecordingDoneFrame();

			break;
		default:
			break;
		}
	}


	public void DestroyFinger()
	{

		if (finger!=null) 
		{
			Destroy(finger);
		}
	}


	/// <summary>
	/// 播放旁白
	/// </summary>
	public void PlayAudioAside()
	{
		//背景音乐淡出
		Manager._instance.bgMusicFadeOut=true;
		audioSource.clip=Manager._instance.audioAside[LevelManager.currentLevelData.LevelID-1];
		audioSource.Play();
	}



	/// <summary>
	/// 暂停旁白播放
	/// </summary>
	public void PauseAudioAside()
	{
		audioSource.Pause();
	}

	/// <summary>
	/// 恢复旁白播放
	/// </summary>
	public 	void ResumeAudioAside()
	{
		audioSource.UnPause();

	}

}
