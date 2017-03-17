using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MousePlayBall : MonoBehaviour 
{

	public static MousePlayBall _instance;

	public float ballMoveSpeed=1f;
	private Animator mouseAnimator;


	public GameObject ball;

	void Awake()
	{

		_instance=this;
	}

	void Start () 
	{
		mouseAnimator=GetComponent<Animator>();
		//这里应该获得球   
		//to do...

		if (ball==null) 
		{
//			ball=transform.parent.Find("Ball").gameObject;
			ball=GameObject.Find("Ball");
		}


	}
		

	void Update () 
	{
		
		ClickTheMouse();
	
	}


	void ClickTheMouse()
	{
		if (Input.GetMouseButtonDown(0))
		{
			
//			RaycastHit2D[] hit =Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position); 
			RaycastHit2D hit=Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position); 
		
//			for (int i = 0; i < hit.Length; i++) {
//				Debug.Log("hit["+i+"] name=="+hit[i].collider.name);
//			}
			if (hit.collider!=null) 
			{
				Debug.Log("hit.collider!=null");
				Debug.Log("hit.collider.gameObject.name------"+hit.collider.gameObject.name);

				if (hit.collider.tag=="Player") 
				{
					Debug.Log("点了老鼠-------");

					//如果没有销毁小手，就销毁小手，同时录音按钮隐藏，下一步按钮出现，老鼠开始播放动画
					if (BussinessManager._instance.finger!=null) 
					{
						Destroy(BussinessManager._instance.finger);
						FormalScene._instance.nextBtn.gameObject.SetActive(true);
						FormalScene._instance.recordBtn.gameObject.SetActive(false);
					}
					Manager._instance.move=true;
					mouseAnimator.SetTrigger("standToRun");
		
				}
			}
		}

	}



	public void OrderMouseToKickBallOutSide()
	{
		Debug.Log("----KickTheBallOutSide()");
		Manager._instance.move=true;
		mouseAnimator.SetTrigger("standToRun");
		ballMoveSpeed=5f;

	}

	/// <summary>
	/// 检测老鼠和球的碰撞
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnCollisionEnter2D(Collision2D coll)
	{
		Debug.Log("OnCollisionEnter2D");
		if (coll.gameObject.tag!=null) 
		{

			Debug.Log("coll.gameObject.tag!=null----name is: "+coll.gameObject.name);
		}
		if (coll.gameObject.tag=="Ball") 
		{
			Manager._instance.move=false;
			Debug.Log("碰到了球");

			//碰到了球就切换动画
			mouseAnimator.SetTrigger("kickToStand");
			if (Manager._instance.levelOneOver) 
			{

				Debug.Log("切换关卡到第二关");

				StartCoroutine(waitMouseClickBall());

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

		FormalScene._instance.UpgradeLevel();

		FormalScene._instance.ScreenDarkenThenReloadFormalScene();
	}



	void BallMove()
	{
		Vector2 moveOffset=new Vector2(Random.Range(-3f,-6f),Random.Range(3f,6f));
		ball.GetComponent<Rigidbody2D>().velocity=moveOffset*ballMoveSpeed;
	
	}


}
