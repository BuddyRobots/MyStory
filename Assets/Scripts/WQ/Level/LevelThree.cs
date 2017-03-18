using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// 屏幕变亮后，移动摄像机到狮子的位置，点击狮子，播放动画（眼睛跳出外面看小老鼠），怒吼，狮子抖动，老鼠抖动，球抖动，老鼠和球一起滑落掉出屏幕
/// </summary>
public class LevelThree : MonoBehaviour {


	public static LevelThree _instance;

	private Animator mouseAnimator;
	private Animator ballAnimator;
	private Animator lionAnimator;

	GameObject mouse;
	GameObject ball;
	GameObject cam;
	public GameObject lion;

	Vector3 originMousePos;
	Vector3 originBallPos;
	Vector3 lionHeadPos;
	private Vector3 destCamPos;
	Vector3 originCamPos;

	bool showFingerOnLion;//是否出现小手提示点击狮子
	bool lionClick;
	bool shakeTofall;
	bool canShowFinger;
	[HideInInspector]
	public bool mouseFall;
	[HideInInspector]
	public	bool ballFall;
	public bool pause;

	float camMovespeed;

	void Awake()
	{
		_instance=this;
	}

	void Start ()
	{
		FormalScene._instance.nextBtn.gameObject.SetActive(false);
		camMovespeed=3f;
		originCamPos=new Vector3(0,0,-10);
		destCamPos=new Vector3 (-9.07f,0,-10);
		cam=GameObject.Find("Main Camera");
		lionAnimator=lion.GetComponent<Animator>();

		Init();
	}

	void Init()
	{
		pause=false;
		mouseFall=false;
		ballFall=false;
		shakeTofall=false;
		canShowFinger=false;
		originMousePos=new Vector3(-4.4f,2.06f,0);
		originBallPos=new Vector3(-6f,2.2f,0);
		cam.transform.position=originCamPos;

		if (Manager.storyStatus ==StoryStatus.Normal) 
		{
			showFingerOnLion=false;
		}
		else if (Manager.storyStatus ==StoryStatus.Recording || Manager.storyStatus ==StoryStatus.PlayRecord)
		{

			showFingerOnLion=true;
		}


		ShowMouse();
		ShowBall();


		//保证初始化的时候动画状态机不是暂停的
		if (mouseAnimator!=null) 
		{
			mouseAnimator.speed=1;
		}
		if (lionAnimator!=null) 
		{
			lionAnimator.speed=1;
		}
		if (ballAnimator!=null) 
		{
			ballAnimator.speed=1;
		}


	}


	void Update () 
	{
		if (FormalScene._instance.storyBegin )
		{
			#region 移动摄像机
			if (cam.transform.position.x>destCamPos.x) 
			{
				cam.transform.Translate(Vector3.left*camMovespeed*Time.deltaTime);
				
			}
			else
			{
				cam.transform.position=destCamPos;
				canShowFinger=true;
			}
			#endregion

			if (canShowFinger) 
			{
				Debug.Log("Manager.storyStatus-------"+Manager.storyStatus);
				if (Manager.storyStatus==StoryStatus.Normal) 
				{
					//出现小手
					if (!showFingerOnLion) 
					{
						ShowFinger(lion.transform.position);
						showFingerOnLion=true;
					}


					if (showFingerOnLion)
					{

						//点击狮子
						if (!lionClick) 
						{
							ClickLion();
						}
					}
				}
				else if (Manager.storyStatus==StoryStatus.Recording || Manager.storyStatus ==StoryStatus.PlayRecord) 
				{
					Debug.Log("Manager.storyStatus--"+Manager.storyStatus);
					lionClick=true;
				}

			}


			if (lionClick) 
			{

				//如果是正常状态下就播放旁白
				if (Manager.storyStatus==StoryStatus.Normal)
				{
					//播放旁白 ，显示字幕
					BussinessManager._instance.PlayAudioAside();
					FormalScene._instance.ShowSubtitle();
				}
				else
				{

				}

				//播放动画
				if (!shakeTofall) 
				{
					PlayAnimation();

					shakeTofall=true;
				}
			}


			if (mouseFall && !pause) {
				
				mouse.transform.Translate(Vector3.down*Time.deltaTime);

			}

			if (ballFall && !pause) {
				ball.transform.Translate(Vector3.down*Time.deltaTime);
			}

		}
		
	}




	void ClickLion()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				return ;
			}
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), lion.transform.position); 
			if (hit.collider!=null) 
			{
				if (hit.collider.tag=="Lion") 
				{
					//如果没有销毁小手，则销毁小手，同时播放草的动画
					if (BussinessManager._instance.finger!=null) 
					{

						Destroy(BussinessManager._instance.finger);
					}

					lionClick=true;
				}
			}
		}

	}


	/// <summary>
	/// 点击播放按钮，开启场景故事（有播放录音，有字幕，或者还有动画）
	/// </summary>
	public void PlayStoryWithAudioRecording()
	{
		Init();
	}
	public void StartStoryToRecordAudioAndVideo()
	{
		//如果有小手提示点击，就销毁小手，点击失效 

		if (BussinessManager._instance.finger!=null) 
		{
			Destroy(BussinessManager._instance.finger);

		}

		lionAnimator.SetBool("lionShaking",false);
		lionAnimator.SetBool("idle",true);
		Init();

	}
		
	public void PauseStory()
	{
		Debug.Log("---levelOne--PauseStory()");
		//如果在播放动画，就暂停动画；
		//如果在播放旁白，就暂停旁白；
		//如果有字幕，且在切换字幕，就暂停切换
		PauseAnimation();
		BussinessManager._instance.PauseAudioAside();
		SubtitleShow._instance.pause=true;
		pause=true;

	}
	public void ResumeStory()
	{
		pause=false;
		BussinessManager._instance.ResumeAudioAside();
		ResumeAnimation();
		SubtitleShow._instance.pause=false;
	}



	void PlayAnimation()
	{
		if (lionAnimator.GetBool("idle")) 
		{
			lionAnimator.SetBool("idle",false);
		}

		lionAnimator.SetBool("lionShaking",true);
		ballAnimator.SetTrigger("ShakeToFall");
		mouseAnimator.SetTrigger("fall");

	}

	void PauseAnimation()
	{
		mouseAnimator.speed=0;
		ballAnimator.speed=0;
		lionAnimator.speed=0;

	}

	void ResumeAnimation()
	{
		mouseAnimator.speed=1;
		ballAnimator.speed=1;
		lionAnimator.speed=1;

	}



	void ShowFinger(Vector3 pos)
	{
		BussinessManager._instance.ShowFinger(pos);//这个坐标位置可以灵活设置  ***********

	}


	private void ShowMouse()
	{
		if (mouse ==null) 
		{
//			mouse=Instantiate(Resources.Load("Prefab/Mouse")) as GameObject;
			mouse=Manager._instance.mouseGo;
			if (mouse==null) 
			{
				Debug.Log("老鼠为空");
			}
			else
			{
				mouse.transform.position=originMousePos;

				mouse.name="Mouse";
				mouseAnimator=mouse.GetComponent<Animator>();
				if (mouse.GetComponent<Rigidbody2D>()!=null)
				{
					mouse.GetComponent<Rigidbody2D>().simulated=false;
				}
			}
			if (mouse.GetComponent<MouseFall>()==null) 
			{
				mouse.AddComponent<MouseFall>();
			}

	
		}



	}




	private void ShowBall()
	{

		if (ball==null) 
		{
			ball=Instantiate(Resources.Load("Prefab/Ball")) as GameObject;
			ball.transform.position=originBallPos;
			ball.name="Ball";
			if (ball.GetComponent<Rigidbody2D>()!=null) 
			{
				Debug.Log("qiu  you gang ti ");
				ball.GetComponent<Rigidbody2D>().simulated=false;
				Debug.Log("ball.GetComponent<Rigidbody2D>().simulated--"+ball.GetComponent<Rigidbody2D>().simulated);
			}
			ballAnimator=ball.GetComponent<Animator>();
			if (ball.GetComponent<BallFall>()==null) 
			{
				ball.AddComponent<BallFall>();
			}

		}


	}



	public void MouseFall()
	{


	}

	public void BallFall()
	{


	}




}
