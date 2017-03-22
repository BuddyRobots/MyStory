using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTwo_new : MonoBehaviour 
{
	public static LevelTwo_new _instance;

	private Animator mouseAnimator;
	[HideInInspector]
	public GameObject mouse;
	[HideInInspector]
	public GameObject ball;

	public Transform tar_0;
	public Transform tar_1;
	public Transform tar_2;
	//三个目标点
	private Vector3 dest_0;
	private Vector3 dest_1;
	private Vector3 dest_2;//球的边缘的位置（改点的x===球的位置的X+球的图形的一半,y和z的值与球一样）

	private Vector3 dest;

	Vector3 originMousePos;
	Vector3 originBallPos;

	public bool startToWalk;//第一个动画结束帧将值设为true

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
	bool  flag;
	bool  crawlDone;

	bool pause;

	bool move=false;

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

		dest_0=tar_0.position;
		dest_1=tar_1.position;
		dest_2=tar_2.position;


		originMousePos=new Vector3(7.6f,-3.3f,0);//mouse.transform.position;
		originBallPos=new Vector3(-6f,2.3f,0);

		Init();

		ShowMouse();
		ShowBall();

		//保证初始化的时候动画状态机不是暂停的
		if (mouseAnimator!=null) 
		{
			mouseAnimator.speed=1;
		} 

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
		isOver=true;
		ballClicked=false;
		mouseAniDone=false;
		pause=false;
		changeScene=false;
		audioAsidePlayed=false;
		startToWalk=false;
		move=false;
		flag=false;
		crawlDone=false;


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
	}




	void Update () 
	{

		if (FormalScene._instance.storyBegin && !pause)
		{
			if (Manager.storyStatus==StoryStatus.Normal) 
			{
				if (!flag) 
				{
					move=true;
					flag=true;
				}

				if (mouse.transform.position.x>=tar_0.position.x) 
				{
					mouseAnimator.Play("01_WalkToBall");
				}
				else
				{


					if (startToWalk) //第一个动画播放完了
					{

						if (!ballClicked) 
						{
							mouseAnimator.CrossFade("idle",0);//播放站立动画

						}
						else
						{


						}



						//出现小手
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

						if (ballClicked) //如果点击了球，就要开始播放走的动画
						{
							//播放旁白，显示字幕
							if (!audioAsidePlayed) 
							{
								BussinessManager._instance.PlayAudioAside();
								audioAsidePlayed=true;
							}
							FormalScene._instance.ShowSubtitle();

							//老鼠移动，播放动画
							if (mouse.transform.position.x<=tar_0.position.x && mouse.transform.position.x>=tar_1.position.x) 
							{

								//mouseAnimator.SetTrigger("walk");
								mouseAnimator.CrossFade("02_Walk",0);
								moveSpeed=1.8f;
							}
							else if(mouse.transform.position.x<=tar_1.position.x &&  mouse.transform.position.x> tar_2.position.x)//老鼠网上爬
							{
								if (!crawlDone) {
									mouseAnimator.CrossFade("03_CrawlUp",0);

									crawlDone=true;
								}
								if(!arrivedThirdDest)
								{
									move=false;
									dest=dest_2;
									destFlag++;
									isOver=false;
									arrivedThirdDest=true;
								}

								if (isOver && destFlag==1 ) 
								{
									Debug.Log("到达第三个点---老鼠动画结束,故事场景2动画结束");
									mouseAnimator.CrossFade("idle",0);
									mouseAniDone=true;
								}

							}

						}

					}

				}

			}
			else if(Manager.storyStatus ==StoryStatus.PlayRecord || Manager.storyStatus ==StoryStatus.Recording)
			{

				Debug.Log("@@@@@@@@@@@@@@@@@@");
				//老鼠直接连着播放3个动画

				if (mouse.transform.position.x>=tar_0.position.x) 
				{
					Debug.Log("播放走的动画");
					if (mouseAnimator==null) {
						Debug.Log("空----");
					}
					//					mouseAnimator.CrossFade("01_WalkToBall",0);
				}
				else
				{
					if (startToWalk) //第一个动画播放完了
					{
						Debug.Log("第一个动画播放完了");
						//老鼠移动，播放动画
						if (mouse.transform.position.x<=tar_0.position.x && mouse.transform.position.x>=tar_1.position.x) 
						{

							//mouseAnimator.SetTrigger("walk");
							mouseAnimator.CrossFade("02_Walk",0);
							moveSpeed=1.8f;
						}
						else if(mouse.transform.position.x<=tar_1.position.x &&  mouse.transform.position.x> tar_2.position.x)//老鼠网上爬
						{
							if (!crawlDone) 
							{
								Debug.Log("老鼠开始爬-----");
								mouseAnimator.CrossFade("03_CrawlUp",0);

								crawlDone=true;
							}
							if(!arrivedThirdDest)
							{
								move=false;
								dest=dest_2;
								destFlag++;
								isOver=false;
								arrivedThirdDest=true;
							}

							if (isOver && destFlag==1 ) 
							{
								Debug.Log("到达第三个点---老鼠动画结束,故事场景2动画结束");
								mouseAnimator.CrossFade("idle",0);
								mouseAniDone=true;
							}

						}


					}

				}

			}

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

		if (move && pause==false) 
		{
			mouse.transform.Translate(Vector3.left*moveSpeed*Time.deltaTime);
		}

		MoveTo(dest);


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
				//
				//				if(Vector3.Distance(tar, mouse.transform.position)<=0.1f)
				//				{
				//					Debug.Log("到达了终点");
				//					isOver = true;
				//					mouse.transform.position = tar;
				//				}
				if (mouse.transform.position.x<tar.x) 
				{
					isOver = true;
					mouse.transform.position = tar;
				}
			}

		}
		else
		{

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
					if (c.tag=="Ball") {
						Debug.Log("点击了球");
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
		if (ball.GetComponent<Animator>()!=null) {
			ball.GetComponent<Animator>().enabled=false;
		}

	}


	public void StartStoryToRecordAudioAndVideo()
	{
		if (BussinessManager._instance.finger!=null) 
		{
			Destroy(BussinessManager._instance.finger);

		}

//		Init();
		startToWalk=false;
		crawlDone=false;
		arrivedThirdDest=false;
		isOver=true;
		mouseAniDone=false;
		move=true;
		pause=false;
		FormalScene._instance.storyBegin=false;

		mouseAnimator.CrossFade("",0);
		mouseAnimator.CrossFade("01_WalkToBall",0);
		mouse.transform.position=originMousePos;
		moveSpeed=2;
		move=true;
		startToWalk=false;


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


		//		mouse.transform.position=Manager._instance.outsideScreenPos;
	}

}
