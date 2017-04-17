using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelOne : MonoBehaviour 
{
	public static LevelOne _instance;

	public GameObject  grassL;
	public GameObject  grassR;
	public Transform originMouseTrans;
	public Transform originBallTrans;

	//第一关的场景中有老鼠和球
	private GameObject mouse;
	private GameObject ball;

	private Animation grassAni;
	private Animator mouseAnimator;

	[HideInInspector]
	public bool showFingerOnMouse;//是否出现小手提示点击老鼠
	[HideInInspector]
	public bool secondSceneShow=false;

	private	bool startStoryNormally;//该变量用来保证故事只进行一次
	private bool showFingerOnGrass;//是否出现小手提示点击草
	private bool grassClicked;//是否点击了草
	private bool aniPlayed;
	private bool flag;

	string grassAniName="Grass_2";

	void Awake()
	{
		_instance=this;
	}

	void Start () 
	{
		Manager.storyStatus=StoryStatus.Normal;
		grassAni=grassL.GetComponent<Animation>();

		Init();
	
	}

	void Init()
	{
		Manager._instance.recordBtnHide=false;
		Manager._instance.levelOneOver=false;

		startStoryNormally=false;
		aniPlayed=false;
		flag=false;

		if (Manager.storyStatus ==StoryStatus.Normal) 
		{
			showFingerOnGrass=false;
			grassClicked=false;
		}
		else if (Manager.storyStatus ==StoryStatus.Recording)
		{
			showFingerOnGrass=true;
			grassClicked=true;
		}
	}

	public void InitByClickingCloseBtnOfRecordingDoneFrame()
	{
		mouse.transform.position=Manager._instance.outsideScreenPos;
		ball.transform.position=Manager._instance.outsideScreenPos;
		ball.GetComponent<Rigidbody2D>().simulated=false;
		grassL.GetComponent<BoxCollider2D>().enabled=true;
		grassR.GetComponent<BoxCollider2D>().enabled=true;

		Manager.storyStatus=StoryStatus.Normal;
		Init();
		Manager ._instance.fingerMove=true;

	}


	void Update () 
	{
		if (FormalScene._instance.storyBegin) 
		{
			if (Manager.storyStatus==StoryStatus.Normal)
			{
				//出现小手提示点击草,只出现一次
				if (!showFingerOnGrass) 
				{
					ShowFinger(grassL.transform.localPosition);
					showFingerOnGrass=true;
				}
				//点击草
				if (showFingerOnGrass) 
				{
					if (!grassClicked) 
					{
						ClickGrass();
					}
				}

			}
			else if (Manager.storyStatus==StoryStatus.Recording) 
			{

				//播放动画
				if (!aniPlayed) 
				{
					PlayAnimation();
					aniPlayed=true;
				}
			}

			//如果点击了草，并且草播放完了动画
			if (grassClicked && !grassAni.IsPlaying(grassAniName)) 
			{
				if (Manager.storyStatus==StoryStatus.Normal) 
				{
					if (!startStoryNormally) 
					{
						StartStoryNormally();
						startStoryNormally=true;
					}
				}
				else if (Manager.storyStatus==StoryStatus.Recording)
				{
					if (!flag) 
					{
						ShowMouse();
						mouse.SetActive(true);
						ShowBall();
						ball.SetActive(true);
						FormalScene._instance.ShowSubtitle();

						flag=true;
					}
				}
			}
				
			//如果进入了第二部分
			if (secondSceneShow) 
			{
				mouse.GetComponentInChildren<BoxCollider2D>().enabled=true;
				//进入第二部分，出现小手提示点击老鼠，出现球，给老鼠添加脚本
				if (!showFingerOnMouse)
				{
					Manager._instance.bgMusicFadeIn=true;
					ShowFinger(mouse.transform.position);
					if (mouse.GetComponent<MousePlayBall>()==null) 
					{
						mouse.AddComponent<MousePlayBall>();
					}
					showFingerOnMouse=true;
				}
			}

			if (Manager._instance.recordBtnHide) 
			{
				FormalScene._instance.recordBtn.gameObject.SetActive(false);

			}

		}

	}

	void ShowFinger(Vector3 pos)
	{
		BussinessManager._instance.ShowFinger(pos);//这个坐标位置可以灵活设置  ***********

	}

	void ClickGrass()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				return ;
			}

			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), grassL.transform.position); 

			if (hit.collider!=null) 
			{
				if (hit.collider.tag=="ClickObj") 
				{
					//如果没有销毁小手，则销毁小手，同时播放草的动画
					if (BussinessManager._instance.finger!=null) 
					{

						Destroy(BussinessManager._instance.finger);
						PlayAnimation();
					}

					grassL.GetComponent<BoxCollider2D>().enabled=false;
					grassR.GetComponent<BoxCollider2D>().enabled=false;

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
		ShowBall();
		FormalScene._instance.ShowSubtitle();
		BussinessManager._instance.PlayAudioAside();
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
		mouse.SetActive(false);
		ball.SetActive(false);
		//草重新播放动画，从新出现老鼠和球
		 //TODO

//		grassClicked=false;
		flag=false;
		aniPlayed=false;

	}
		
	public void PauseStory()
	{
		PauseAnimation();
		BussinessManager._instance.PauseAudioAside();
		SubtitleShow._instance.pause=true;
		StopAllCoroutines();
	}
		
	public void ResumeStory()
	{
		ResumeAnimation();
		BussinessManager._instance.ResumeAudioAside();
		SubtitleShow._instance.pause=false;
	}

	private void ShowMouse()
	{
		if (mouse==null) 
		{
			mouse=Manager._instance.mouseGo;

		}
		mouse.transform.position=originMouseTrans.position;
		mouseAnimator=mouse.GetComponent<Animator>();
		mouseAnimator.CrossFade("idle",0);

		mouse.GetComponentInChildren<BoxCollider2D>().enabled=false;
		if (mouse.GetComponent<Rigidbody2D>()==null) 
		{
			mouse.AddComponent<Rigidbody2D>();
		}
		mouse.GetComponent<Rigidbody2D>().gravityScale=0;
		mouse.GetComponent<Rigidbody2D>().constraints=RigidbodyConstraints2D.FreezeRotation;
		mouse.GetComponent<Rigidbody2D>().simulated=true;



	}

	private void ShowBall()
	{
		ball=Manager._instance.ball;
		ball.transform.position=originBallTrans.position;
		if (ball.GetComponent<Rigidbody2D>()==null)
		{
			ball.AddComponent<Rigidbody2D>();
		}
		ball.GetComponent<Rigidbody2D>().simulated=true;
		ball.GetComponent<Rigidbody2D>().velocity=Vector2.zero;
		ball.GetComponent<Rigidbody2D>().angularDrag=2f;

		if (ball.GetComponent<BallMoveWithBg>()==null) 
		{
			ball.AddComponent<BallMoveWithBg>();
		}
	}

	void OnDisable()
	{
		Manager._instance.Reset();
		if (mouse) 
		{
			if (mouse.GetComponent<MousePlayBall>()!=null) 
			{
				Destroy(mouse.GetComponent<MousePlayBall>());
			}
			mouse.GetComponentInChildren<BoxCollider2D>().enabled=true;
		}


		if (ball) 
		{
			if (ball.GetComponent<BallMoveWithBg>()!=null) 
			{
				Destroy(ball.GetComponent<BallMoveWithBg>());
			}
			ball.GetComponent<Rigidbody2D>().velocity=Vector2.zero;
			ball.transform.rotation=Quaternion.Euler(0, 0, 0);


			if (ball.GetComponent<Rigidbody2D>())
			{
				Destroy(ball.GetComponent<Rigidbody2D>());
			}
		}


	}

}
