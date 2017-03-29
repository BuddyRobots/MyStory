using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelEight : MonoBehaviour 
{
	public static LevelEight _instance;
	
	GameObject mouse;
	Animator mouseAnimator;


	public Transform originMouseTrans;
	bool showFingerOnMouse;
	[HideInInspector]
	public bool netIsErased;


	bool nextBtnActivated;

	void Awake()
	{
		_instance=this;
	}

	void Start () 
	{
		//下一步按钮隐藏
		FormalScene._instance.nextBtn.gameObject.SetActive(false);
		FormalScene._instance.recordBtn.gameObject.SetActive(false);
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

		if (mouse.GetComponent<MouseDrag>()==null) 
		{
			mouse.AddComponent<MouseDrag>();
		}

	}



	void OnDisable()
	{
		Manager._instance.Reset();
		if (mouse.GetComponent<MouseDrag>()!=null)
		{
			Destroy(mouse.GetComponent<MouseDrag>());
		}
		mouse.GetComponent<Rigidbody2D>().gravityScale=0f;
		mouseAnimator.CrossFade("idle",0);

	}

}
