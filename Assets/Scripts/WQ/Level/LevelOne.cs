using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelOne : MonoBehaviour 
{
	public static LevelOne _instance;

	public GameObject  grassL;
	public GameObject  grassR;


	private Animation grassAni;
	private Animator mouseAnimator;

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
	Vector3 originBallPos=new Vector3(1.02f,-2.93f,0);

	[HideInInspector]
	public bool secondSceneShow=false;
//	[HideInInspector]
//	public bool recordBtnHide;


	void Awake()
	{
		_instance=this;
	}

	void Start () 
	{
		grassAni=grassL.GetComponent<Animation>();
		Init();
		Manager._instance.mouseGo.GetComponent<Animator>().CrossFade("idle",0);
	}

	void Init()
	{
		showFingerOnGrass=false;
		Manager._instance.recordBtnHide=false;
		Manager._instance.levelOneOver=false;
	}


	void Update () 
	{
		storyBegin =FormalScene._instance.storyBegin ;
		//屏幕亮完以后故事才开始
		if (storyBegin) 
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
//				mouse.GetComponent<BoxCollider2D>().enabled=true;
				mouse.GetComponentInChildren<BoxCollider2D>().enabled=true;
//				Debug.Log("进入第二部分---出现小球");
				//进入第二部分，出现小手提示点击老鼠，出现球，给老鼠添加脚本
				if (!showFingerOnMouse)
				{
					Manager._instance.bgMusicFadeIn=true;
//					ShowBall();
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
				Debug.Log("当前点击是在UI 上");
				return ;
			}
//			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); 
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), grassL.transform.position); 

			if (hit.collider!=null) 
			{
				if (hit.collider.tag=="ClickObj") 
				{
					Debug.Log("点了小草-------");

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

	private void ShowMouse()
	{
		mouse=Manager._instance.mouseGo;
		mouse.transform.position=originMousePos;
		mouseAnimator=mouse.GetComponent<Animator>();
		mouseAnimator.CrossFade("idle",0);

		mouse.GetComponentInChildren<BoxCollider2D>().enabled=false;
		mouse.GetComponent<Rigidbody2D>().simulated=true;


	}

	private void ShowBall()
	{
		ball=Manager._instance.ball;
		ball.transform.position=originBallPos;
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

	/// <summary>
	/// 点击播放按钮，开启场景故事（有播放录音，有字幕，或者还有动画）
	/// </summary>
	public void PlayStoryWithAudioRecording()
	{
		//重新开始故事----只不过录音被替换
		PlayAnimation();
		ShowMouse();
		ShowBall();
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
		ShowBall();
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

	void OnDisable()
	{
		Manager._instance.Reset();
		if (mouse) 
		{
			if (mouse.GetComponent<MousePlayBall>()!=null) 
			{
				Destroy(mouse.GetComponent<MousePlayBall>());
			}
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
