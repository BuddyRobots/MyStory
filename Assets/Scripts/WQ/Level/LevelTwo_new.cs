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


		Init();

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

		if (Manager.storyStatus ==StoryStatus.Normal) 
		{
			Debug.Log("正常状态");
			showFingerOnBall=false;
		}
		else if (Manager.storyStatus ==StoryStatus.UnNormal)
		{
			Debug.Log("非正常状态");

			showFingerOnBall=true;
		}


		ShowMouse();
		ShowBall();
	}




	void Update () 
	{
		if (FormalScene._instance.storyBegin)
		{
			//小老鼠移动到第一个点（播放跑进场的动画）
			if (!arrivedFirstDest)
			{
				//（播放进场的动画）   to do....
				destFlag++;
				dest=dest_0;
				isOver=false;
				arrivedFirstDest=true;
			}

			//如果到达了第一个点
			if (isOver && destFlag==1)//mouse.transform.position==dest_0) 
			{
				
				//如果是非正常状态下，就不用出现小手提示点击球，跳过这一步
				if (Manager.storyStatus==StoryStatus.UnNormal) 
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
			if (Manager.storyStatus==StoryStatus.UnNormal) 
			{
				BussinessManager._instance.DestroyFinger();

			}


			//如果点击了球，且销毁了小手，就要显示字幕，播放旁白，并且小老鼠移动到第二个点并且爬向第三个点
			if (ballClicked ) 
			{
				#region 显示字幕
				FormalScene._instance.ShowSubtitle();
				#endregion




				#region 播放旁白(只有正常状态下才需要播放旁白)
				if ( Manager.storyStatus==StoryStatus.Normal) 
				{
					Debug.Log("在正常状态下");
					BussinessManager._instance.PlayAudioAside();

				}
				#endregion





				#region 老鼠运动
				//小老鼠移动到第二个点（播放跑的动画）
				if (!arrivedSecondDest)
				{
					//（播放跑的动画）   to do....
					dest=dest_1;
					destFlag++;
					isOver=false;
					arrivedSecondDest=true;
				}
				//如果老鼠到了第二个点，就要移动到第三个点（播放爬的动画）
				if (isOver && destFlag==2)//mouse.transform.position==dest_1)
				{
					//（播放爬的动画）   to do....


					if (!arrivedThirdDest)
					{
						dest=dest_Final;
						isOver=false;
						arrivedThirdDest=true;
					}
				}
				#endregion
				
			}

			MoveTo(dest);

		}
	}

	private void MoveTo(Vector3 tar)
	{

		if(!isOver)
		{
//			Debug.Log("isover---false");
			Vector3 offSet = tar - mouse.transform.position;
			mouse.transform.position += offSet.normalized * 3f * Time.deltaTime;
			if(Vector3.Distance(tar, mouse.transform.position)<=0.1f)
			{
				Debug.Log("到达了终点");
				isOver = true;
				mouse.transform.position = tar;
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

			mouse=Instantiate(Resources.Load("Prefab/Mouse")) as GameObject;
			//			mouse=Manager._instance.mouseGo;
			if (mouse==null) 
			{
				Debug.Log("老鼠为空");
			}
			//			mouse.transform.parent=transform;//这里不能设置父对象，设置了以后老鼠就从DontdestroyOnLoad里出去了
			mouse.transform.position=originMousePos;
			Debug.Log("显示老鼠，老鼠的位置是---"+mouse.transform.position);
			Debug.Log("老鼠应该出现的位置为---"+originMousePos);


			mouse.name="Mouse";

			mouseAnimator=mouse.GetComponent<Animator>();

			GameObject.DontDestroyOnLoad(mouse);

		}
		else
		{

			mouse.transform.position=originMousePos;
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

	}


	public void StartStoryToRecordAudioAndVideo()
	{

		Init();

	}
	public void PlayStoryWithAudioRecording()
	{
		Init();
	}
}
