﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



///该类用来--控制背景音乐的播放，淡入淡出，存储玩家在角色选择界面选择的角色类型

public class Manager : MonoBehaviour 
{
	public static Manager _instance;

	public static bool musicOn=true;
	public static bool recordingDone=false;

	public static ModelType modelType;//玩家选择画的角色类型，作为绘画展示界面显示什么图片的依据

	public bool bgMusicFadeOut;
	public bool bgMusicFadeIn;

	[HideInInspector]
	public AudioSource bgAudio;

	[HideInInspector]
	public Texture2D texture;//用来存储从拍摄界面取得的Texture2D

	private float fadingTimer;//淡入淡出计时器

	void Awake()
	{
		_instance=this;

	}

	void Start () 
	{

		Manager.musicOn=true;

		bgMusicFadeOut=false;
		bgMusicFadeIn=false;

		bgAudio=GameObject.Find("Manager").GetComponent<AudioSource>();

		GameObject.DontDestroyOnLoad(gameObject);

	}
		

	void Update () 
	{

		#region 控制背景音乐的开和关（点击音乐按钮时）
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
		#endregion

		#region 控制背景音乐的淡入淡出(故事场景播放配音时淡出，配音结束时淡入)
		if (bgMusicFadeOut) //如果背景音乐需要淡出，音量在固定时间内从1渐变成0，然后暂停
		{
			fadingTimer+=Time.deltaTime;
			bgAudio.volume=Mathf.Lerp(1f,0,fadingTimer/Constant.MUSIC_FADINGTIME);
			if (fadingTimer>=Constant.MUSIC_FADINGTIME) 
			{
				fadingTimer=0;
				bgAudio.Pause ();
				bgMusicFadeOut=false;
			}
			
		}

		if (bgMusicFadeIn) //如果背景音乐需要淡入，先开启背景音乐，然后音量在固定时间内从0渐变成1
		{
			if (!bgAudio.isPlaying) 
			{
				bgAudio.Play();
			}
			fadingTimer+=Time.deltaTime;
		
			bgAudio.volume=Mathf.Lerp(0,1f,fadingTimer/Constant.MUSIC_FADINGTIME);
			if (fadingTimer>=Constant.MUSIC_FADINGTIME) 
			{
				fadingTimer=1f;
				bgMusicFadeIn=false;
			}
			
		}
		#endregion

	}




//	public void ShowFinger(Vector3 pos)
//	{
//		prePos=Vector3.zero; 
//		Debug.Log("----出现小手");
//		GameObject sceneParent=GameObject.Find("SceneParent");
////		if (prePos == pos) 
////		{
////			return;
////		}
////		prePos = pos;
//		if (finger) 
//		{
//			Destroy (finger);
//			finger = null;
//		}
//		finger = Instantiate (fingerPrefab) as GameObject;
//		finger.name="finger";
//		finger.transform.parent = sceneParent.transform;
//		finger.transform.localScale = Vector3.one;
//		finger.GetComponent<FingerCtrl> ().FingerShow (pos + offSet);
//	}



}
