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
	}
		

	void Update () 
	{
		if (ball==null) 
		{
			ball=transform.parent.Find("Ball").gameObject;
		}
		ClickTheMouse();
		
	}


	void ClickTheMouse()
	{
		if (Input.GetMouseButtonDown(0))
		{
			
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); 
			if (hit.collider!=null) 
			{
				if (hit.collider.tag=="Player") 
				{
					Debug.Log("点了老鼠-------");


					if (BussinessManager._instance.finger!=null) 
					{
						Destroy(BussinessManager._instance.finger);
						FormalScene._instance.nextBtn.gameObject.SetActive(true);


						FormalScene._instance.recordBtn.gameObject.SetActive(false);




					}

					Manager._instance.move=true;

					//老鼠播放动画   并开始移动背景，球也移动，如果老鼠碰到了球，老鼠停止动画，球往左边移动随机一段距离

					mouseAnimator.SetTrigger("standToRun");

				}
			}
		}

	}



	public void KickTheBallOutSide()
	{
		Debug.Log("----KickTheBallOutSide()");
		Manager._instance.move=true;
		mouseAnimator.SetTrigger("standToRun");
		ballMoveSpeed=5f;

	}


	void OnCollisionEnter2D(Collision2D coll) 
	{
		if (coll.gameObject.tag=="Ball") 
		{
			Manager._instance.move=false;
			Debug.Log("碰到了球");

			mouseAnimator.SetTrigger("kickToStand");
			if (Manager._instance.levelOneSceneClosed) 
			{

				Debug.Log("切换关卡到第二关");





				StartCoroutine(waitMouseClickBall());





			}

		}

	}


	IEnumerator  waitMouseClickBall()
	{
		//等待老鼠踢球动画的时间-------

		yield return new WaitForSeconds(1.2f);
		FormalScene._instance.UpgradeLevel();
		SceneManager.LoadSceneAsync("6_FormalScene_0");
	}


	void BallMove()
	{
		Vector2 moveOffset=new Vector2(Random.Range(-3f,-6f),Random.Range(3f,6f));
		ball.GetComponent<Rigidbody2D>().velocity=moveOffset*ballMoveSpeed;

//		BallMovine_Test.instance.Move();
	}


}
