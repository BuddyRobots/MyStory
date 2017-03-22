using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwo_new : MonoBehaviour 
{
	public static LevelTwo_new _instance;

	private Animator mouseAnimator;

	public GameObject mouse;
	public GameObject ball;
	public Transform tar_0;
	public Transform tar_1;
	public Transform tar_2;
	//三个目标点
	private Vector3 dest_0;
	private  Vector3 dest_1;
	private Vector3 dest_2;//球的边缘的位置（改点的x===球的位置的X+球的图形的一半,y和z的值与球一样）

	private Vector3 dest;

	Vector3 originMousePos;
	Vector3 originBallPos;

	public bool startToWalk;

	bool arrivedFirstDest;
	bool arrivedSecondDest;
	bool arrivedThirdDest;
	bool isOver;
	bool showFingerOnBall;
	bool ballClicked;
	bool storyGoOn;
	bool audioAsidePlayed;

	bool mouseAniDone;//动画是否结束的标志
	bool changeScene;

	bool pause;

	int destFlag;//值为1，2，3，1代表到达目标点1，2代表叨叨目标点2；。。。

	StoryStatus storyStatus;
	[HideInInspector]
	public float moveSpeed;


	void Awake()
	{
		_instance=this;
	}

	void Start () 
	{
		Manager.storyStatus=StoryStatus.Normal;


		FormalScene._instance.nextBtn.gameObject.SetActive(false);

		dest_1=tar_1.position;
		dest_2=tar_2.position;


		originMousePos=new Vector3(7.6f,-3.3f,0);//mouse.transform.position;
		originBallPos=new Vector3(-6f,2.3f,0);
		dest_0=new Vector3(tar_0.position.x,originMousePos.y,originMousePos.z);

		Manager._instance.ballPosForLevelThree=originBallPos;
		Manager._instance.mousePosForLevelThree=dest_2;
		Init();
		Debug.Log("init 后老鼠的位置是--"+mouse.transform.position);

	}

	/// <summary>
	/// 初始化变量，比如老鼠的位置，球的位置，以及一些bool值
	/// </summary>
	public void Init()
	{
		moveSpeed=2;
		destFlag=0;
		arrivedFirstDest=false;
		arrivedSecondDest=false;
		arrivedThirdDest=false;
		isOver=false;
		ballClicked=false;
		mouseAniDone=false;
		pause=false;
		changeScene=false;
		audioAsidePlayed=false;
		startToWalk=false;

		if (Manager.storyStatus ==StoryStatus.Normal) 
		{
			Debug.Log("正常状态");
			showFingerOnBall=false;
		}
		else if (Manager.storyStatus ==StoryStatus.Recording || Manager.storyStatus ==StoryStatus.PlayRecord)
		{
			Debug.Log("非正常状态");

			showFingerOnBall=true;
		}


		ShowMouse();
		ShowBall();


		//保证初始化的时候动画状态机不是暂停的
		if (mouseAnimator!=null) 
		{
			mouseAnimator.speed=1;
		} 
	}
	bool flag;

	void Update () 
	{

		if (FormalScene._instance.storyBegin)
		{
			//			Debug.Log("故事开始了---老鼠的位置是---"+mouse.transform.position);
			//小老鼠移动到第一个点（播放跑进场的动画）
			if (!arrivedFirstDest)
			{
				//（播放进场的动画）  
				//				mouseAnimator.SetTrigger("walkIn");
				mouseAnimator.CrossFade("01_WalkToBall",0);
				destFlag++;
				dest=dest_0;
				isOver=false;
				arrivedFirstDest=true;
			}

			//如果到达了第一个点
			if (isOver && destFlag==1)//mouse.transform.position==dest_0) 
			{
				if (startToWalk) 
				{
					//如果是非正常状态下，就不用出现小手提示点击球，跳过这一步
					if (Manager.storyStatus==StoryStatus.Recording || Manager.storyStatus ==StoryStatus.PlayRecord) 
					{
						if (mouse.transform.position.x!=tar_0.position.x) {
							mouse.transform.position=tar_0.position;
						}
						ballClicked=true;
						Debug.Log("非正常情况下到达第一个点-------");


					}
					else if (Manager.storyStatus==StoryStatus.Normal) //如果是正常状态下，就要出现小手提示点击球
					{
						if (!flag) 
						{
							mouseAnimator.CrossFade("idle",0);

							flag=true;
						}
						Debug.Log("正常情况下到达第一个点，播放站立动画");

						if (!showFingerOnBall) 
						{
							ShowFinger(ball.transform.position);

							Debug.Log("出现了小手，点击球吧");

							showFingerOnBall=true;
						}
						//如果出现了小手，就可以点击球了
						if (showFingerOnBall)
						{
							if (!ballClicked) 
							{
								//点击球并销毁老鼠
								ClickBall();
							}
						}

					}
					
				}

			}


			//这里是保证录音和播放界面不会有小手出现
			if (Manager.storyStatus==StoryStatus.Recording || Manager.storyStatus ==StoryStatus.PlayRecord) 
			{
				BussinessManager._instance.DestroyFinger();

			}


			//如果点击了球，且销毁了小手，就要显示字幕，播放旁白，并且小老鼠移动到第二个点并且爬向第三个点
			if (ballClicked ) 
			{
				//刚体禁用
				mouse.GetComponent<Rigidbody2D>().simulated=false;
				if ( Manager.storyStatus==StoryStatus.Normal) 
				{
					Debug.Log("在正常状态下");
					if (!audioAsidePlayed) 
					{
						//播放旁白(只有正常状态下才需要播放旁白)
						BussinessManager._instance.PlayAudioAside();
						audioAsidePlayed=true;
					}
					// 显示字幕
					FormalScene._instance.ShowSubtitle();
				}

				#region 老鼠运动
				//小老鼠移向第二个点（播放跑的动画）
				if (!arrivedSecondDest)
				{
					mouseAnimator.CrossFade("02_Walk",0);
//					
//					mouseAnimator.SetTrigger("walk");
					dest=dest_1;
					moveSpeed=1.8f;
					destFlag++;
					isOver=false;
					arrivedSecondDest=true;
				}
				//如果老鼠到了第二个点，就要移动到第三个点（播放爬的动画）
				if (isOver && destFlag==2)//mouse.transform.position==dest_1)
				{

					Debug.Log("到达第二个点");
					//（播放爬的动画）   to do....

					//					mouse.GetComponent<Rigidbody2D>().simulated=false;     //这句代码放到这里老鼠还是有刚体模拟，不起作用   ？？？？？


					if (!arrivedThirdDest)
					{
						dest=dest_2;
						destFlag++;
						isOver=false;
						arrivedThirdDest=true;
					}
				}
				if (isOver && destFlag==3) 
				{
					Debug.Log("到达第三个点---老鼠动画结束,故事场景2动画结束");
					mouseAnimator.CrossFade("idle",0);
					mouseAniDone=true;

				}



				#endregion

			}

			MoveTo(dest);

		}

		if (mouseAniDone && Manager._instance.isSubtitleShowOver)//如果老鼠动画播放结束了，字幕也结束了，就跳转界面
		{
			//在正常状态或者播放状态下
			if (Manager.storyStatus ==StoryStatus.Normal || Manager.storyStatus ==StoryStatus.PlayRecord)
			{
				if (!changeScene) 
				{

					FormalScene._instance.ChangeSceneAutomatically();

					changeScene=true;
				}
			}

		}
	}



	private void MoveTo(Vector3 tar)
	{
		if (!pause) 
		{
			if(!isOver)
			{
				Vector3 offSet = tar - mouse.transform.position;
				mouse.transform.position += offSet.normalized * moveSpeed * Time.deltaTime;
				Debug.Log("Vector3.Distance(tar, mouse.transform.position)---"+Vector3.Distance(tar, mouse.transform.position));
				if(Vector3.Distance(tar, mouse.transform.position)<=0.1f)
				{
					Debug.Log("到达了终点");
					isOver = true;
					mouse.transform.position = tar;
				}
			}
		}
		else
		{
			Debug.Log("暂停------------");
		}

	}

	void ShowFinger(Vector3 pos)
	{
		BussinessManager._instance.ShowFinger(pos);//这个坐标位置可以灵活设置  ***********

	}

	void ClickBall()
	{
		if (Input.GetMouseButtonDown(0))
		{
			//用这种方法来判断是否点击了对象比较准确些
			Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			if (col.Length>0) 
			{
				foreach (Collider2D c in col) 
				{
					if (c.tag=="Ball") 
					{
						BussinessManager._instance.DestroyFinger();
						ballClicked=true;
					}
				}
			}
		}
	}


	private void ShowMouse()
	{
		if (mouse ==null) 
		{
			mouse=Manager._instance.mouseGo;

		}

		if (mouse!=null) 
		{
			mouse.transform.position=originMousePos;
			mouse.name="Mouse";
			mouseAnimator=mouse.GetComponent<Animator>();
			if (mouse.GetComponent<MouseEnterScene>()==null) 
			{
				mouse.AddComponent<MouseEnterScene>();
			}
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
		else
		{
			ball.transform.position=originBallPos;
		}

		if (ball.GetComponent<Rigidbody2D>()!=null) 
		{
			ball.GetComponent<Rigidbody2D>().gravityScale=0;

		}
		if (ball.GetComponent<Animator>()!=null)
		{
			ball.GetComponent<Animator>().enabled=false;
		}

	}


	public void StartStoryToRecordAudioAndVideo()
	{
		if (BussinessManager._instance.finger!=null) 
		{
			Destroy(BussinessManager._instance.finger);

		}
		//		mouseAnimator.CrossFade("",0);
		//		mouseAnimator.CrossFade("01_WalkToBall",0);
		Init();

		//		mouseAnimator.Play("01_WalkToBall");



	}
	public void PlayStoryWithAudioRecording()
	{
		Init();
		mouseAnimator.CrossFade("",0);
		mouseAnimator.CrossFade("01_WalkToBall",0);
	}



	public void PauseStory()
	{

		PauseAnimation();
		BussinessManager._instance.PauseAudioAside();
		SubtitleShow._instance.pause=true;

	}

	public void ResumeStory()
	{
		ResumeAnimation();
		BussinessManager._instance.ResumeAudioAside();
		SubtitleShow._instance.pause=false;
	}


	void PauseAnimation()
	{

		mouseAnimator.speed=0;
		pause=true;
		Debug.Log("停止动画");
	}

	void ResumeAnimation()
	{

		mouseAnimator.speed=1;
		pause=false;
	}

	void OnDestroy()
	{


		mouse.transform.position=Manager._instance.outsideScreenPos;
	}

}
