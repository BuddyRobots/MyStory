using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 老鼠去狮子头上捡球（点击球，播放老鼠动画，并移动老鼠到狮子旁边，然后爬到老鼠头上）
/// </summary>
public class LevelTwo : MonoBehaviour 
{
	public static LevelTwo _instance;

	private Animator mouseAnimator;

	public GameObject mouse;
	public GameObject ball;

	bool storyBegin;//故事是否开始的标志
	bool showFingerOnBall;
	bool ballClicked;
	bool  mouseMoveToSecondPos;

	Vector3 originMousePos;
	Vector3 originBallPos;


	public Transform dest_0;
	public Transform dest_1;
	public Transform targetPos;


	Vector3 offsetToBall;

	[HideInInspector]
	public bool mouseMove;//is mouse need to move or not


	void Awake()
	{
		_instance=this;
	}
		
	void Start ()
	{
//		ShowBall();
//		ShowMouse();
		offsetToBall=dest_1.localPosition-ball.transform.localPosition;
		Debug.Log("0000---"+mouse.transform.position);
		Debug.Log("1111---"+dest_0.position);

	}
	
	bool arrivedFirstDest;
	bool arrivedSecondDest;

	void Update()
	{

		////如果屏幕亮了，老鼠就走进来
		if (FormalScene._instance.storyBegin) 
		{
			isOver=false;
		}



	}
//	void Update ()
//	{
//		//如果屏幕亮了，老鼠就走进来
////		if (FormalScene._instance.storyBegin) {
////			MouseMoving._instance.moveToLeft=true;
////		}
//		//如果老鼠到达了第一个目的地
//
//
//		if (true) {
//			
//		}
//
//		if (mouse.transform.localPosition.x<=dest_0.position.x) 
//		{
//
//
//			MouseMoving._instance.moveToLeft=false;
//
//			if (!showFingerOnBall) 
//			{
//
//
////				ShowFinger(ball.transform.localPosition);
//
//				Debug.Log("出现了小手，点击球吧");
//
//				showFingerOnBall=true;
//			}
//
//			if (showFingerOnBall)
//			{
//
//				if (!ballClicked)
//				{
//					ClickBall();
//				}
//
//
//			}
//
//
//
//			if (ballClicked)
//			{
//
//
//				MouseMoving._instance.moveToLeft=true;
//				if (mouse.transform.localPosition.x<=dest_1.position.x) //老鼠走到第二个点
//				{
//					MouseMoving._instance.moveToLeft=false;
//					if (!mouseMoveToSecondPos)
//					{
//						isOver=false;
//						mouseMoveToSecondPos=true;
//					}
//					//老鼠往狮子头上爬。。。。。
//
//					//				mouse.transform.position = new Vector3(
//					//					Mathf.Lerp(posStop_1.localPosition.x,ball.transform.localPosition.x,Time.deltaTime),
//					//					Mathf.Lerp(posStop_1.localPosition.y,ball.transform.localPosition.y,Time.deltaTime),
//					//					0);
//
//
//
//
//
//				}
//
//			}
//
//
//
//			MoveTo(targetPos.localPosition);
//
//
//		}
//
//
//
//
//	}

	private void ClickBall()
	{

		if (Input.GetMouseButtonDown(0))
		{
			
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), ball.transform.position);//Vector2.zero); 
//			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), );//(ball.transform.localPosition-Camera.main.ScreenToWorldPoint(Input.mousePosition))); 
			if (hit.collider!=null) 
			{
				if (hit.collider.tag=="Ball") 
				{
					Debug.Log("点击了球");
					//如果没有销毁小手，则销毁小手，同时播放草的动画
//					if (BussinessManager._instance.finger!=null) 
//					{
//
//						Destroy(BussinessManager._instance.finger);
//
//						Debug.Log("销毁了小手");
//
//					}

					ballClicked=true;
				}
			}




			Collider2D[] col=Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));

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



	
	void ShowFinger(Vector3 pos)
	{
		BussinessManager._instance.ShowFinger(pos);//这个坐标位置可以灵活设置  ***********

	}



	bool isOver=true;

	private void MoveTo(Vector3 tar)
	{
		if(!isOver)
		{
			Vector3 offSet = tar - mouse.transform.localPosition;
			mouse.transform.localPosition += offSet.normalized * 3f * Time.deltaTime;
			if(Vector3.Distance(tar, transform.localPosition)<=2f)
			{
				Debug.Log("到达了终点");
				isOver = true;
				transform.localPosition = tar;
			}
		}

	}







}
