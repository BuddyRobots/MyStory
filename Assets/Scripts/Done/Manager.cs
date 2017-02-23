using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//该类用来--控制背景音乐的播放，存储玩家在角色选择界面选择的角色类型

public class Manager : MonoBehaviour 
{
	public static Manager _instance;

	public static bool musicOn=true;
	public static bool recordingDone=false;

	public bool bgMusicFadeOut;
	public bool bgMusicFadeIn;

	public static ModelType modelType;//玩家选择画的角色类型，作为绘画展示界面显示什么图片的依据

	[HideInInspector]
	public AudioSource bgAudio;

	private float fadingTimer;//淡入淡出计时器
	private const float FADINGTIME=4f;


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





		if (bgMusicFadeOut) //如果背景音乐需要淡出，音量在固定时间内从1渐变成0，然后暂停
		{
			fadingTimer+=Time.deltaTime;
			bgAudio.volume=Mathf.Lerp(1f,0,fadingTimer/FADINGTIME);
			if (fadingTimer>=FADINGTIME) 
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
		
			bgAudio.volume=Mathf.Lerp(0,1f,fadingTimer/FADINGTIME);
			if (fadingTimer>=FADINGTIME) 
			{
				fadingTimer=1f;
				bgMusicFadeIn=false;
			}
			
		}

	}







}
