﻿using System.Collections;
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
	bool startStoryNormally;//该变量用来保证故事只进行一次


	bool storyBegin;//故事是否开始的标志
	bool showFingerOnGrass;//是否出现小手提示点击草
	[HideInInspector]
	public bool showFingerOnMouse;//是否出现小手提示点击老鼠
	bool grassClicked;//是否点击了草

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
		showFingerOnGrass=false;

	}

	void ShowFinger(Vector3 pos)
	{
		BussinessManager._instance.ShowFinger(pos);//这个坐标位置可以灵活设置  ***********

	}
		



	void Update () 
	{
		storyBegin =FormalScene._instance.storyBegin ;
		//屏幕亮完以后故事才开始
		if (storyBegin) 
		{
			//如果没有出现小手提示点击草，就出现小手提示点击草
			if (!showFingerOnGrass) 
			{
				Debug.Log("小手出现提示点击小草");
				ShowFinger(grass.transform.localPosition);

				showFingerOnGrass=true;
			}




			//出现小手提示点击草以后，点击草
			if (showFingerOnGrass) 
			{
				//如果没有点击草
				if (!grassClicked) 
				{
					ClickTheGrass();

				}

				
			}

			//如果点击了草，并且草播放完了动画
			if (grassClicked && !grassAni.IsPlaying(grassAniName)) 
			{

				if (!startStoryNormally) 
				{
					StartStoryNormally();
					startStoryNormally=true;
				}

			}
				
			//如果进入了第二部分
			if (secondSceneShow) 
			{
//				Debug.Log("进入第二部分---出现小球");
				//进入第二部分，出现小手提示点击老鼠，出现球，给老鼠添加脚本
				if (!showFingerOnMouse)
				{
					
					ShowBall();
					ShowFinger(mouse.transform.position);

					if (mouse.GetComponent<MousePlayBall>()==null) 
					{
						mouse.AddComponent<MousePlayBall>();
					}

					showFingerOnMouse=true;
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

					//如果没有销毁小手，则销毁小手，同时播放草的动画
					if (BussinessManager._instance.finger!=null) 
					{
						
						Destroy(BussinessManager._instance.finger);

						Debug.Log("销毁了小手");


						PlayAnimation();

					}

					grassClicked=true;
				}
			}
		}
	}


	/// <summary>
	/// 正常开始故事（有旁白，字幕，或者动画等）
	/// </summary>
	void StartStoryNormally()
	{
		ShowMouse();
		FormalScene._instance.ShowSubtitle();
		BussinessManager._instance.PlayAudioAside();
//		SubtitleCtrl._instance.Init();
	}

	private void ShowMouse()
	{
		if (mouse ==null) 
		{
			
			mouse=Instantiate(Resources.Load("Prefab/Mouse")) as GameObject;
//			mouse=Manager._instance.mouseGo;
			if (mouse==null) 
			{
				Debug.Log("老鼠为空");
			}
//			mouse.transform.parent=transform;//这里不能设置父对象，设置了以后老鼠就从DontdestroyOnLoad里出去了
			mouse.transform.localPosition=originMousePos;

			mouse.name="Mouse";

//			mouseAnimator=mouse.GetComponent<Animator>();
		
			GameObject.DontDestroyOnLoad(mouse);

		}
	    

	}




	private void ShowBall()
	{

		if (ball==null) 
		{
			ball=Instantiate(Resources.Load("Prefab/Ball")) as GameObject;
			ball.transform.localPosition=originBallPos;
			ball.name="Ball";
		
		}

	}
	/// <summary>
	/// 点击播放按钮，开启场景故事（有播放录音，有字幕，或者还有动画）
	/// </summary>
	public void PlayStoryWithAudioRecording()
	{
		
		//重新开始故事----只不过录音被替换
		PlayAnimation();
		ShowMouse();





	}


	void PlayAnimation()
	{
		grassAni.Play(grassAniName);
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
		//如果有小手提示点击，就销毁小手，点击失效 

		if (BussinessManager._instance.finger!=null) 
		{
			Destroy(BussinessManager._instance.finger);
		
		}
		PlayAnimation();
		ShowMouse();

		//如果有球的话，球要隐藏
		if (ball !=null) {
			ball.SetActive(false);
		}
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
		SubtitleShow._instance.pause=true;
		StopAllCoroutines();
	}


	public void ResumeStory()
	{
		ResumeNarratage();
		ResumeAnimation();
//		SubtitleCtrl._instance.pauseChangeSubtitle=false;
		SubtitleShow._instance.pause=false;
	}






}