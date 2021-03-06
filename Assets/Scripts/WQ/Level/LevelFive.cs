﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFive : MonoBehaviour 
{
	public static LevelFive _instance;


	Animator mouseAnimator;

	GameObject ball;
	GameObject mouse;

	public Transform originMouseTrans;
	public Transform originBallTrans;

	bool storyPlay;
	[HideInInspector]
	public bool aniDone;


	void Awake()
	{
		_instance=this;
	}

	void Start () 
	{
		Manager.storyStatus=StoryStatus.Normal;
		FormalScene._instance.nextBtn.gameObject.SetActive(false);

		EnlargeCameraSize();
		ShowMouse();
		ShowBall();

		Init();
	}


	public void Init()
	{
		storyPlay=false;
		aniDone=false;

		if (mouseAnimator!=null) 
		{
			mouseAnimator.speed=1;
		}

	}

	public void InitByClickingCloseBtnOfRecordingDoneFrame()
	{
		Manager.storyStatus=StoryStatus.Normal;
		FormalScene._instance.nextBtn.gameObject.SetActive(false);

		EnlargeCameraSize();
		ShowMouse();
		ShowBall();

		Init();
		MouseRunAway._instance.ResetSpeed();

	}
	void Update () 
	{
		if (FormalScene._instance.storyBegin) 
		{

			if (!storyPlay )
			{
				mouseAnimator.CrossFade("",0);

				mouseAnimator.CrossFade("RunAway",0);
//				if (MouseRunAway._instance) 
//				{
					MouseRunAway._instance.move=true;
//				}

				if (Manager.storyStatus==StoryStatus.Normal) 
				{
					BussinessManager._instance.PlayAudioAside();
				}
				FormalScene._instance.ShowSubtitle();


				storyPlay=true;
			}


			//如果动画播完了，字幕也结束了，就切换场景
			if (aniDone && Manager._instance.isSubtitleShowOver) 
			{
				mouseAnimator.CrossFade("idle",0);
				//在正常状态或者播放状态下
				if (Manager.storyStatus ==StoryStatus.Normal)
				{
					FormalScene._instance.ChangeSceneAutomatically();
					MouseRunAway._instance.move=false;


					aniDone=false;

				}
			}

		}
		
	}

	void LateUpdate()
	{
		mouse.transform.localRotation=Quaternion.Euler(0,0,0);
	}


	void EnlargeCameraSize()
	{
		Camera.main.orthographicSize=6f;
	}
		

	void ShowMouse()
	{
		if (mouse==null) 
		{
			mouse=Manager._instance.mouseGo;
		}
		mouse.transform.position=originMouseTrans.position;
		mouseAnimator=mouse.GetComponent<Animator>();
		mouseAnimator.CrossFade("",0);

		mouseAnimator.CrossFade("MouseRunAway_Idle",0);
		if (mouse.GetComponent<Rigidbody2D>()==null) 
		{
			mouse.AddComponent<Rigidbody2D>();
		}
		mouse.GetComponent<Rigidbody2D>().gravityScale=0;
		mouse.GetComponent<Rigidbody2D>().constraints=RigidbodyConstraints2D.FreezeRotation;
		mouse.GetComponent<Rigidbody2D>().simulated=true;


		if (mouse.GetComponent<MouseRunAway>()==null) 
		{
			mouse.AddComponent<MouseRunAway>();
		}

	}

	void ShowBall()
	{
		if (ball==null) 
		{
			ball=Manager._instance.ball;

		}
		ball.transform.parent=GameObject.Find("Mouse/Hip/Torso/L arm/L hand").transform;
		Debug.Log("ball.transform.parent--"+ball.transform.parent);
		ball.transform.localPosition=originBallTrans.position;
		ball.SetActive(true);


	}


	public void StartStoryToRecordAudioAndVideo()
	{
		Init();
		mouse.transform.position=originMouseTrans.position;
		mouseAnimator.CrossFade("",0);
		mouseAnimator.CrossFade("RunAway",0);
		MouseRunAway._instance.ResetSpeed();

	}


	public void PauseStory()
	{
		MouseRunAway._instance.move=false;
		PauseAnimation();
		BussinessManager._instance.PauseAudioAside();
		SubtitleShow._instance.pause=true;

	}

	public void ResumeStory()
	{
		MouseRunAway._instance.move=true;
		ResumeAnimation();
		BussinessManager._instance.ResumeAudioAside();
		SubtitleShow._instance.pause=false;
	}

	void PauseAnimation()
	{

		mouseAnimator.speed=0;

	}
	void ResumeAnimation()
	{

		mouseAnimator.speed=1;
	}




	void OnDisable()
	{
		
		ball.transform.parent=null;
		ball.transform.position=Manager._instance.outsideScreenPos;


		Manager._instance.Reset();
		if (mouse.GetComponent<MouseRunAway>()) 
		{
			Destroy(mouse.GetComponent<MouseRunAway>());
		}
		mouseAnimator.CrossFade("idle",0);

	}


}
