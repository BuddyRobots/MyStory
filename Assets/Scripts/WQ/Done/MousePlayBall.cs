using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MousePlayBall : MonoBehaviour 
{

	public static MousePlayBall _instance;

	public float ballMoveSpeed;
	private Animator mouseAnimator;


	public GameObject ball;
	bool mouseIsMoving;
	bool flag;

	void Awake()
	{

		_instance=this;
	}

	void Start () 
	{

		ballMoveSpeed=1f;
		mouseIsMoving=false;
		flag=false;
		mouseAnimator=GetComponent<Animator>();
	
		if (ball==null) 
		{
			ball=GameObject.Find("Ball");
		}


	}
		

	void Update () 
	{
		if (ball==null) 
		{
			ball=GameObject.Find("Ball");
		}
		ClickMouse();
	
	}

	void ClickMouse()
	{
		if (Input.GetMouseButtonDown(0))
		{
			//用这种方法来判断是否点击了对象比较准确些
			Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			if (col.Length>0) 
			{
				foreach (Collider2D c in col) 
				{
					if (c.tag=="Player") 
					{
						//如果没有销毁小手，就销毁小手，同时录音按钮隐藏，下一步按钮出现，老鼠开始播放动画
						if (BussinessManager._instance.finger!=null) 
						{
							Destroy(BussinessManager._instance.finger);
							FormalScene._instance.nextBtn.gameObject.SetActive(true);
						}

					if (!mouseIsMoving) 
					{
						mouseIsMoving=true;
						Manager._instance.move=true;
						mouseAnimator.SetTrigger("standToRun");
					}
						
					}
				}
			}
		}
	}

	/// <summary>
	/// 老鼠完成了一次运动
	/// </summary>
	public void MouseFinishedKickingBallOnce()
	{
		mouseIsMoving=false;

	}



	public void OrderMouseToKickBallOutSide()
	{
		ballMoveSpeed=5f;
		Manager._instance.move=true;

		if (!mouseIsMoving) 
		{
			mouseIsMoving=true;
			Manager._instance.move=true;
			mouseAnimator.SetTrigger("standToRun");
		}
	}

	/// <summary>
	/// 检测老鼠和球的碰撞
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnCollisionEnter2D(Collision2D coll)
	{

		if (coll.gameObject.tag=="Ball") 
		{
			Manager._instance.move=false;

			//碰到了球就切换动画
			mouseAnimator.CrossFade("KickingToStand",0);
			if (Manager._instance.levelOneOver) 
			{
				if (!flag) // to ensure the coroutine be called only once
				{
					flag =true;
					StartCoroutine(waitMouseClickBall());

				}
			}
		}
	}



	IEnumerator  waitMouseClickBall()
	{
		yield return new WaitForSeconds(1.2f);//这个时间是等待老鼠踢完球的时间
		UpradeLevelAndChangeScene();
	}

	/// <summary>
	/// 保存好关卡信息，并切换场景
	/// </summary>
	void UpradeLevelAndChangeScene()
	{
		Debug.Log("MousePlayBall---UpradeLevelAndChangeScene()");
		FormalScene._instance.UpgradeLevel();


		FormalScene._instance.ScreenDarkenThenReloadFormalScene();
	}



	void BallMove()
	{
		Vector2 moveOffset=new Vector2(Random.Range(-3f,-6f),Random.Range(3f,6f));
		ball.GetComponent<Rigidbody2D>().velocity=moveOffset*ballMoveSpeed;
	
	}


}
