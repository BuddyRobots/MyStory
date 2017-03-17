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

	GameObject mouse;
	GameObject ball;
	GameObject cam;
	public GameObject lion;

	Vector3 originMousePos;
	Vector3 originBallPos;
	Vector3 lionHeadPos;
	private Vector3 destCamPos;
	Vector3 originCamPos;

	bool storyBegin;//故事是否开始的标志
	bool showFingerOnLion;//是否出现小手提示点击狮子
	bool startStoryNormally;//该变量用来保证故事只进行一次
	bool lionClick;

	float speed;

	void Awake()
	{
		_instance=this;
	}

	void Start ()
	{
		FormalScene._instance.nextBtn.gameObject.SetActive(false);
		speed=3f;
		originCamPos=new Vector3(0,0,-10);
		destCamPos=new Vector3 (-9.07f,0,-10);
		cam=GameObject.Find("Main Camera");

		Init();
	}

	void Init()
	{
		originMousePos=new Vector3(-4.9f,1.73f,0);
		originBallPos=new Vector3(-6f,2.21f,0);
		cam.transform.position=originCamPos;
		ShowMouse();
		ShowBall();

	}
	bool canShowFinger;
	// Update is called once per frame
	void Update () 
	{

		if (storyBegin !=FormalScene._instance.storyBegin ) 
		{
			storyBegin =FormalScene._instance.storyBegin ;
		}

		if (storyBegin )
		{
			#region 移动摄像机
			if (cam.transform.position.x>destCamPos.x) 
			{
				cam.transform.Translate(Vector3.left*speed*Time.deltaTime);
				
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
					Debug.Log("zheng chagn ");
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
				else if (Manager.storyStatus==StoryStatus.UnNormal) {
					Debug.Log("Manager.storyStatus--"+Manager.storyStatus);
					lionClick=true;
				}

			}


			if (lionClick) 
			{

				//如果是正常状态下就播放旁白
				if (Manager.storyStatus==StoryStatus.Normal)
				{


					//播放旁白  。。。。。to do 


					BussinessManager._instance.PlayAudioAside();
					FormalScene._instance.ShowSubtitle();
				}
				else
				{


				}

				//播放动画，显示字幕





			}




		}
		
	}

	void ClickLion()
	{

		if (Input.GetMouseButtonDown(0))
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				Debug.Log("当前点击是在UI 上");
				return ;
			}
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), lion.transform.position); 
			if (hit.collider!=null) 
			{
				if (hit.collider.tag=="Lion") 
				{
					Debug.Log("点了lion-------");

					//如果没有销毁小手，则销毁小手，同时播放草的动画
					if (BussinessManager._instance.finger!=null) 
					{

						Destroy(BussinessManager._instance.finger);

						Debug.Log("销毁了小手");


//						PlayAnimation();

					}

					lionClick=true;
				}
			}
		}

	}
	/// <summary>
	/// 正常开始故事（有旁白，字幕，或者动画等）
	/// </summary>
	void StartStoryNormally()
	{
		FormalScene._instance.ShowSubtitle();
		GameObject.Find("Main Camera").GetComponent<AudioSource>().clip=LevelManager.currentLevelData.AudioAside;//Resources.Load<AudioClip>("Audio/Seagulls");
		GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();
	
	}

	/// <summary>
	/// 点击播放按钮，开启场景故事（有播放录音，有字幕，或者还有动画）
	/// </summary>
	public void PlayStoryWithAudioRecording()
	{

		//重新开始故事----只不过录音被替换
	





	}
	public void StartStoryToRecordAudioAndVideo()
	{
		//如果有小手提示点击，就销毁小手，点击失效 

		if (BussinessManager._instance.finger!=null) 
		{
			Destroy(BussinessManager._instance.finger);

		}




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

	}


	public void ResumeStory()
	{
		BussinessManager._instance.ResumeAudioAside();
		ResumeAnimation();
		SubtitleShow._instance.pause=false;
	}

	void PlayAnimation()
	{
		
	}

	void PauseAnimation()
	{
		

	}
	void ResumeAnimation()
	{
		

	}



	void ShowFinger(Vector3 pos)
	{
		BussinessManager._instance.ShowFinger(pos);//这个坐标位置可以灵活设置  ***********

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

			mouseAnimator=mouse.GetComponent<Animator>();

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
}
