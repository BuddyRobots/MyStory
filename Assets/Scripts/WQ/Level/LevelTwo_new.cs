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
	public Transform tar_3;
	//三个目标点
	private Vector3 dest_0;
	private  Vector3 dest_1;
	private Vector3 dest_Final;//球的边缘的位置（改点的x===球的位置的X+球的图形的一半,y和z的值与球一样）

	private Vector3 dest;

	Vector3 originMousePos;
	Vector3 originBallPos;


	bool arrivedFirstDest;
	bool arrivedSecondDest;
	bool arrivedThirdDest;
	bool isOver;
	bool showFingerOnBall;
	bool ballClicked;
	bool storyGoOn;

	bool mouseAniDone;//动画是否结束的标志
	bool changeScene;

	bool pause;

	int destFlag;//值为1，2，3，1代表到达目标点1，2代表叨叨目标点2；。。。

	StoryStatus storyStatus;


	void Awake()
	{
		_instance=this;
	}

	void Start () 
	{
		Manager.storyStatus=StoryStatus.Normal;


		FormalScene._instance.nextBtn.gameObject.SetActive(false);

		dest_1=tar_1.position;
		dest_Final=tar_3.position;


		originMousePos=new Vector3(8.15f,-3.29f,0);//mouse.transform.position;
		originBallPos=new Vector3(-6f,2.2f,0);
		dest_0=new Vector3(tar_0.position.x,originMousePos.y,originMousePos.z);

		Manager._instance.ballPosForLevelThree=originBallPos;
		Manager._instance.mousePosForLevelThree=dest_Final;
		Init();
		Debug.Log("init 后老鼠的位置是--"+mouse.transform.position);

	}
	
	/// <summary>
	/// 初始化变量，比如老鼠的位置，球的位置，以及一些bool值
	/// </summary>
	public void Init()
	{
		
		destFlag=0;
		arrivedFirstDest=false;
		arrivedSecondDest=false;
		arrivedThirdDest=false;
		isOver=false;
		ballClicked=false;
		mouseAniDone=false;
		pause=false;
		changeScene=false;

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
		if (mouseAnimator!=null) {
			mouseAnimator.speed=1;
		}

//		mouseAnimator.SetBool("stop",true);
//		mouseAnimator.SetBool("stop",false);





	}




	void Update () 
	{

		Debug.Log("老鼠的位置是-----------"+mouse.transform.position);
		if (FormalScene._instance.storyBegin)
		{
			//小老鼠移动到第一个点（播放跑进场的动画）
			if (!arrivedFirstDest)
			{
				//（播放进场的动画）  
				mouseAnimator.SetTrigger("walkIn");
				destFlag++;
				dest=dest_0;
				isOver=false;
				arrivedFirstDest=true;
			}

			//如果到达了第一个点
			if (isOver && destFlag==1)//mouse.transform.position==dest_0) 
			{
				Debug.Log("到达第一个点");


				//如果是非正常状态下，就不用出现小手提示点击球，跳过这一步
				if (Manager.storyStatus==StoryStatus.Recording || Manager.storyStatus ==StoryStatus.PlayRecord) 
				{
					ballClicked=true;
				}
				else if (Manager.storyStatus==StoryStatus.Normal) //如果是正常状态下，就要出现小手提示点击球
				{
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


			//这里是保证录音和播放界面不会有小手出现
			if (Manager.storyStatus==StoryStatus.Recording || Manager.storyStatus ==StoryStatus.PlayRecord) 
			{
				BussinessManager._instance.DestroyFinger();

			}


			//如果点击了球，且销毁了小手，就要显示字幕，播放旁白，并且小老鼠移动到第二个点并且爬向第三个点
			if (ballClicked ) 
			{
				

				//（播放跑的动画）   to do....

				//刚体禁用
				mouse.GetComponent<Rigidbody2D>().simulated=false;


				if ( Manager.storyStatus==StoryStatus.Normal) 
				{
					Debug.Log("在正常状态下");
					//播放旁白(只有正常状态下才需要播放旁白)
					BussinessManager._instance.PlayAudioAside();
					// 显示字幕
					FormalScene._instance.ShowSubtitle();
				}
					
				#region 老鼠运动
				//小老鼠移动到第二个点（播放跑的动画）
				if (!arrivedSecondDest)
				{
					mouseAnimator.SetTrigger("walkToLion");
					dest=dest_1;
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
						dest=dest_Final;
						destFlag++;
						isOver=false;
						arrivedThirdDest=true;
					}
				}
				if (isOver && destFlag==3) 
				{
					Debug.Log("到达第三个点---老鼠动画结束,故事场景2动画结束");
					mouseAnimator.SetBool("idle",true);
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
		Debug.Log("move to");
		if (!pause) 
		{
			if(!isOver)
			{
				//			Debug.Log("isover---false");
				Vector3 offSet = tar - mouse.transform.position;
				mouse.transform.position += offSet.normalized * 1f * Time.deltaTime;
				if(Vector3.Distance(tar, mouse.transform.position)<=0.1f)
				{
					Debug.Log("到达了终点");
					isOver = true;
					mouse.transform.position = tar;
				}
			}

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

			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), ball.transform.position);

			if (hit.collider!=null) 
			{
				if (hit.collider.tag=="Ball") 
				{
					Debug.Log("点击了球");
					BussinessManager._instance.DestroyFinger();
					ballClicked=true;
				}
			}


		}

	}


	private void ShowMouse()
	{
		if (mouse ==null) 
		{
//			mouse=Instantiate(Resources.Load("Prefab/Mouse")) as GameObject;
			mouse=Manager._instance.mouseGo;

		}

		if (mouse!=null) 
		{
			mouse.transform.position=originMousePos;
			Debug.Log("显示老鼠，老鼠的位置是---"+mouse.transform.localPosition);
			Debug.Log("老鼠应该出现的位置为---"+originMousePos);


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
			ball.transform.position=originBallPos;
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

	}


	public void StartStoryToRecordAudioAndVideo()
	{
		if (BussinessManager._instance.finger!=null) 
		{
			Destroy(BussinessManager._instance.finger);

		}
		Init();

	}
	public void PlayStoryWithAudioRecording()
	{
		Init();
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
	}

	void ResumeAnimation()
	{

		mouseAnimator.speed=1;
		pause=false;
	}



}
