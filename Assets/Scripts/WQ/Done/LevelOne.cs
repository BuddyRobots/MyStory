using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelOne : MonoBehaviour 
{
	public static LevelOne _instance;

	public GameObject  grass;

	private Animation grassAni;
	private Animator mouseAnimator;
	bool startStoryStraight=false;//是否可以直接开始故事
	bool grassAniPlay;//草是否播放了动画
	bool startStory;//该变量用来保证故事只进行一次

	string grassAniName="Grass_2";

	//第一关的场景中有老鼠和球
	GameObject mouse;
	GameObject ball;


	Vector3 originMousePos=new Vector3(4.7f,-2.9f,0);
	Vector3 originBallPos=new Vector3(0.39f,-2.9f,0);

	[HideInInspector]
	public bool secondSceneShow=false;



	void Awake()
	{
		_instance=this;
	}

	void Start () 
	{
		grassAni=grass.GetComponent<Animation>();
		startStoryStraight=false;
		ShowFinger(grass.transform.localPosition);


	}

	void ShowFinger(Vector3 pos)
	{
		BussinessManager._instance.ShowFinger(pos);//这个坐标位置可以灵活设置  ***********

	}
		

	bool fingerToClickMouseShow;

	void Update () 
	{

		if (!startStoryStraight) 
		{
			ClickTheGrass();
		}

		///草的动画播放完以后再开始故事
		if (grassAniPlay && !grassAni.IsPlaying(grassAniName)) //如果草播放了动画，且已经播完了
		{
			if (!startStory) 
			{
				StartStoryNormally();
				startStory=true;
			}
		}




		if (secondSceneShow) 
		{
//			Debug.Log("进入第二部分");
			//进入第二部分，出现小手提示点击老鼠，出现球，点击老鼠，播放动画，走到球的位置，播放踢球动画，球随机往前走
			if (!fingerToClickMouseShow)
			{
				ShowBall();
				ShowFinger(mouse.transform.position);
				fingerToClickMouseShow=true;
				if (mouse.GetComponent<MousePlayBall>()==null) 
				{
					mouse.AddComponent<MousePlayBall>();
				}

			}

		}



	}



	void ClickTheGrass()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				Debug.Log("当前点击是在UI 上");
				return ;
			}
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); 
			if (hit.collider!=null) 
			{
				if (hit.collider.tag=="ClickObj") 
				{
					Debug.Log("点了小草-------");
					if (BussinessManager._instance.finger!=null) 
					{

						startStoryStraight=true;
						Destroy(BussinessManager._instance.finger);
						Debug.Log("销毁了小手");

						//TODO....动画播放完以后，出现小老鼠，同时播放旁白,显示字幕  
						PlayAnimation();
						//StartStoryNormally();
					}
				}
			}
		}
	}


	/// <summary>
	/// 正常开始故事（有旁白，字幕，或者动画等）
	/// </summary>
	void StartStoryNormally()
	{
//		PlayAnimation();
		ShowMouse();
		FormalScene._instance.ShowSubtitle();
		GameObject.Find("Main Camera").GetComponent<AudioSource>().clip=LevelManager.currentLevelData.AudioAside;//Resources.Load<AudioClip>("Audio/Seagulls");
		GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();
//		SubtitleCtrl._instance.Init();
	}

	private void ShowMouse()
	{
		if (mouse ==null) 
		{
			
			mouse=Instantiate(Resources.Load("Prefab/Mouse")) as GameObject;
			//		mouse=Manager._instance.mouse;
			mouse.transform.parent=transform;
			mouse.transform.localPosition=originMousePos;

		
			mouse.name="Mouse";

			mouseAnimator=mouse.GetComponent<Animator>();


		}
	    

	}




	private void ShowBall()
	{

		if (ball==null) 
		{
			ball=Instantiate(Resources.Load("Prefab/Ball")) as GameObject;
			ball.transform.parent=transform;
			ball.transform.localPosition=originBallPos;
			ball.name="Ball";
		
		}

	}
	/// <summary>
	/// 点击播放按钮，开启场景故事（有播放录音，有字幕，或者还有动画）
	/// </summary>
	public void PlayStoryWithAudioRecording()
	{
		//播放草的动画

	}


	void PlayAnimation()
	{
		grassAni.Play(grassAniName);
		grassAniPlay=true;
	}

	void PauseAnimation()
	{
		if (grassAni.isPlaying) 
		{
			grassAni[grassAniName].speed=0;
		}

	}
	void ResumeAnimation()
	{
		grassAni[grassAniName].speed=1;

	}




	public void StartStoryToRecordAudioAndVideo()
	{




		//如果有小手提示点击，就销毁小手，点击失效   to  do.....
		startStoryStraight=true;
		if (BussinessManager._instance.finger!=null) 
		{

			startStoryStraight=true;
			Destroy(BussinessManager._instance.finger);
		
		}
		ShowMouse();
	

	}



	/// <summary>
	/// 暂停旁白播放
	/// </summary>
	void PauseNarratage()
	{
		GameObject.Find("Main Camera").GetComponent<AudioSource>().Pause();
	}
	/// <summary>
	/// 恢复旁白播放
	/// </summary>
	void ResumeNarratage()
	{
		GameObject.Find("Main Camera").GetComponent<AudioSource>().UnPause();

	}
		
	public void PauseStory()
	{
		Debug.Log("---levelOne--PauseStory()");
		//如果在播放动画，就暂停动画；
		//如果在播放旁白，就暂停旁白；
		//如果有字幕，且在切换字幕，就暂停切换
		PauseAnimation();
		PauseNarratage();
//		SubtitleCtrl._instance.pauseChangeSubtitle=true;
		Show._instance.pause=true;
		StopAllCoroutines();
	}


	public void ResumeStory()
	{
		ResumeNarratage();
		ResumeAnimation();
//		SubtitleCtrl._instance.pauseChangeSubtitle=false;
		Show._instance.pause=false;
	}






}
