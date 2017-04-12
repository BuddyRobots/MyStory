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
	public static StoryStatus storyStatus;//故事的进行状态，是正常进行的，还是非正常进行的（录音与播放的是非正常状态）

	[HideInInspector]
	public AudioSource bgAudio;
	public List<AudioClip> audioAside;

	// Deprecated sourceMat.
	[HideInInspector]
	public Mat sourceMat;//用来存储从拍摄界面取得的Mat

	[HideInInspector]
	public Mouse mouse;

	[HideInInspector]
	public GameObject mouseGo;//存储的老鼠，每个场景里的老鼠都来自于这里，如果玩家没画老鼠，就用预先做好的老鼠形象，如果玩家画了小老鼠，就替换
	[HideInInspector]
	public GameObject ball;//存储的球
	[HideInInspector]
	public GameObject garland;//存储的花环


	/// <summary>
	/// 手是移动还是暂停移动的标志
	/// </summary>
	[HideInInspector]
	public bool fingerMove=true;
	[HideInInspector]
	public bool isSubtitleShowOver;//场景中字幕是否显示完的标志----有的关卡得根据这个标志来判读场景故事是不是结束了该跳到下一关了
	[HideInInspector]
	public bool move;//背景是否移动的标志
	[HideInInspector]
	public bool levelOneOver;
	public bool bgMusicFadeOut;
	public bool bgMusicFadeIn;
	[HideInInspector]
	public bool recordBtnHide;

	/// <summary>
	/// position outside screen
	/// </summary>
	public Vector3 outsideScreenPos=new Vector3(0,20f,0);

	private float musicFadingTimer;//淡入淡出计时器


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


		if (mouseGo==null) 
		{
			mouseGo=Instantiate(Resources.Load("Prefab/Mouse")) as GameObject;
			mouseGo.name="Mouse";
			mouseGo.transform.position=Manager._instance.outsideScreenPos;
		}
		if (ball==null) 
		{
			ball=Instantiate(Resources.Load("Prefab/Ball")) as GameObject;
			ball.name="Ball";
			ball.transform.position=outsideScreenPos;//球在屏幕外面
		}
		if (garland==null)
		{
			garland=Instantiate(Resources.Load("Prefab/Garland")) as GameObject;
			garland.name="Garland";
			garland.transform.position=outsideScreenPos;
		}

		DontDestroyOnLoad(mouseGo);
		DontDestroyOnLoad(ball);
		DontDestroyOnLoad(garland);
	
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
			bgAudio.volume=Mathf.Lerp(1f,0.3f,musicFadingTimer/Constant.MUSIC_FADINGTIME);
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
		
			bgAudio.volume=Mathf.Lerp(0.3f,1f,musicFadingTimer/Constant.MUSIC_FADINGTIME);
			if (musicFadingTimer>=Constant.MUSIC_FADINGTIME) 
			{
				musicFadingTimer=1f;
				bgMusicFadeIn=false;
			}
			
		}
		#endregion
	}
		
	public void ChangeSceneToSetBgAudioVolumeNormal()
	{
		bgAudio.volume=1;
	}

	public void RecordingToSetBgAudioVolumeZero()
	{
//		bgAudio.volume=0;
		bgAudio.volume=0.3f;

	}



	/// <summary>
	/// 每一关结束时重新初始化对象信息
	/// </summary>
	public void Reset()
	{
		if (mouseGo) 
		{
			mouseGo.transform.position=outsideScreenPos;
			mouseGo.GetComponent<Animator>().CrossFade("idle",0);
		}
			
		if (ball) 
		{
			ball.transform.position=outsideScreenPos;
			if (ball.GetComponent<Rigidbody2D>()==null) 
			{
				ball.AddComponent<Rigidbody2D>();
			}

			ball.GetComponent<Rigidbody2D>().simulated=false;//防止球掉下去
			if (ball.GetComponent<Animator>()!=null) 
			{
				ball.GetComponent<Animator>().CrossFade("BallIdle",0);
			}
			
		}

		if (garland)
		{
			garland.transform.position=outsideScreenPos;

		}

	}

}
