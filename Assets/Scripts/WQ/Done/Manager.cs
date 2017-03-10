using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity;
using MyStory;



///该类用来--控制背景音乐的播放，淡入淡出，存储玩家在角色选择界面选择的角色类型

public class Manager :MonoBehaviour
{
	public static Manager _instance;

	public static bool musicOn=true;
	public static bool recordingDone=false;//录音是否结束的标志

	public static ModelType modelType;//玩家选择画的角色类型，作为绘画展示界面显示什么图片的依据

	public bool bgMusicFadeOut;
	public bool bgMusicFadeIn;

	[HideInInspector]
	public AudioSource bgAudio;

	// Deprecated sourceMat.
	[HideInInspector]
	public Mat sourceMat;//用来存储从拍摄界面取得的Mat

	public Mouse mouseGo;

	private float musicFadingTimer;//淡入淡出计时器
	private GameObject manager;


	public List<AudioClip> audioAside;

	[HideInInspector]
	public GameObject mouse;//存储的老鼠，每个场景里的老鼠都来自于这里，如果玩家没画老鼠，就用预先做好的老鼠形象，如果玩家画了小老鼠，就替换
	[HideInInspector]
	public GameObject ball;//存储的球
	[HideInInspector]
	public GameObject garland;//存储的花环


	void Awake()
	{
		if (_instance!=null) 
		{
			Destroy (gameObject);

		}
		else
		{
		    _instance=this;
			GameObject.DontDestroyOnLoad(gameObject);



		}

	}

	void Start () 
	{
		Manager.musicOn=true;
		bgMusicFadeOut=false;
		bgMusicFadeIn=false;
		recordingDone=false;

		bgAudio=GameObject.Find("Manager").GetComponent<AudioSource>();



//		mouse=Instantiate(Resources.Load("Prefab/Mouse")) as GameObject;
//		GameObject.DontDestroyOnLoad(mouse);
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
			musicFadingTimer+=Time.deltaTime;
			bgAudio.volume=Mathf.Lerp(1f,0,musicFadingTimer/Constant.MUSIC_FADINGTIME);
			if (musicFadingTimer>=Constant.MUSIC_FADINGTIME) 
			{
				musicFadingTimer=0;
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
			musicFadingTimer+=Time.deltaTime;
		
			bgAudio.volume=Mathf.Lerp(0,1f,musicFadingTimer/Constant.MUSIC_FADINGTIME);
			if (musicFadingTimer>=Constant.MUSIC_FADINGTIME) 
			{
				musicFadingTimer=1f;
				bgMusicFadeIn=false;
			}
			
		}
		#endregion

	}





}
