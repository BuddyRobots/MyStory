using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelEight : MonoBehaviour 
{
	public static LevelEight _instance;
	
	private GameObject mouse;
	public GameObject net;


	Animator mouseAnimator;


	public Transform originMouseTrans;
	bool showFingerOnMouse;
	[HideInInspector]
	public bool netIsErased;


	bool nextBtnActivated;

	[HideInInspector]
	public bool mouseClicked;

	float alphaChangeTimer=0;
	float alphaChangeTime=1f;

	Color col;


	void Awake()
	{
		_instance=this;
	}

	void Start () 
	{

		Manager.storyStatus=StoryStatus.Normal;

		//下一步按钮隐藏
		FormalScene._instance.nextBtn.gameObject.SetActive(false);
		FormalScene._instance.recordBtn.gameObject.SetActive(false);

		col=net.GetComponent<SpriteRenderer>().color;
		ShowMouse();

	}




	void Update ()
	{
		if (FormalScene._instance.storyBegin) 
		{

			if (!showFingerOnMouse)
			{
				ShowFinger(mouse.transform.position);

				showFingerOnMouse=true;
			}
			//如果显示了小手，就可以开始点击老鼠了
			if (showFingerOnMouse) 
			{
				ClickMouse();
			}


			if (netIsErased) 
			{
				if (!nextBtnActivated) 
				{
					FormalScene._instance.nextBtn.gameObject.SetActive(true);


					nextBtnActivated=true;
				}

				alphaChangeTimer+=Time.deltaTime;
				col.a=Mathf.Lerp(1,0,alphaChangeTimer/alphaChangeTime);
				if (net) {
					net.GetComponent<SpriteRenderer>().color=col;

				}
				if (alphaChangeTimer>=alphaChangeTime) 
				{
					alphaChangeTimer=alphaChangeTime;

					//  TO MODIFY --------------------------
					if (net !=null) 
					{
						Destroy(net);
						MouseDrag._instance.isOnNet = false;//加上这一行之后网消失后老鼠就不会有咬网的动画了，但是为什么呢？？？

					}

					//----------------------------
				}

			}
		
		}


	}

	void ClickMouse()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				Debug.Log("当前点击是在UI 上");
				return ;
			}


			Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			if (col.Length>0) 
			{
				foreach (Collider2D c in col) 
				{
					if (c.tag=="Player") 
					{
						//如果没有销毁小手，则销毁小手
						if (BussinessManager._instance.finger!=null) 
						{
							Destroy(BussinessManager._instance.finger);
							mouseClicked=true;

						}
					}
				}
			}


		}
	}





	void ShowFinger(Vector3 pos)
	{
		BussinessManager._instance.ShowFinger(pos);//这个坐标位置可以灵活设置

	}

	void ShowMouse()
	{
		if (mouse ==null) 
		{
			mouse=Manager._instance.mouseGo;
		}
		mouse.transform.position=originMouseTrans.position;
		mouseAnimator=mouse.GetComponent<Animator>();
		mouseAnimator.CrossFade("idle",0);


		if (mouse.GetComponent<Rigidbody2D>()!=null) 
		{
			mouse.GetComponent<Rigidbody2D>().simulated=true;
		}
		else
		{
			mouse.AddComponent<Rigidbody2D>();
			mouse.GetComponent<Rigidbody2D>().gravityScale=0;
			mouse.GetComponent<Rigidbody2D>().constraints=RigidbodyConstraints2D.FreezeRotation;

		}

		if (mouse.GetComponent<MouseDrag>()==null) 
		{
			mouse.AddComponent<MouseDrag>();
		}

	}



	void OnDisable()
	{
		Manager._instance.Reset();
		if (mouse) 
		{
			if (mouse.GetComponent<MouseDrag>()!=null)
			{
				Destroy(mouse.GetComponent<MouseDrag>());
			}
			if (mouse.GetComponent<Rigidbody2D>()!=null) {

				//			mouse.GetComponent<Rigidbody2D>().simulated=false;
				Destroy(mouse.GetComponent<Rigidbody2D>());
//				mouse.GetComponent<Rigidbody2D>().gravityScale=0;

			}
			mouseAnimator.CrossFade("idle",0);
		}


	}

}
