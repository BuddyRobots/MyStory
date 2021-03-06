﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 老鼠被狮子抓住，点击老鼠随机播放一个挣扎动画，并成钟摆运动
/// </summary>
public class LevelFour : MonoBehaviour 
{
	public static LevelFour _instance;
	Animator mouseAnimator;


	[HideInInspector]
    public 	GameObject mouse;
	private GameObject center;

	public Transform originMouseTrans;

	[HideInInspector]
	public bool showFingerOnMouse;//是否出现小手提示点击老鼠
	private bool storyBegin;//故事是否开始的标志


	void Awake()
	{
		_instance=this;
	}

	void Start () 
	{
		Manager.storyStatus=StoryStatus.Normal;
		
		center=GameObject.Find("Manager");
		if (center) 
		{
			center.transform.position=new Vector3(1.53f,2.73f,0);
			if (center.transform.FindChild("Ball")) 
			{
				center.transform.FindChild("Ball").gameObject.SetActive(false);

			}
		}
		
		
		FormalScene._instance.recordBtn.gameObject.SetActive(false);
		Manager._instance.mouseGo.transform.position=originMouseTrans.position;
		ShowMouse();

	}




	void Update () 
	{
		if (storyBegin !=FormalScene._instance.storyBegin ) 
		{
			storyBegin =FormalScene._instance.storyBegin ;
		}

		if (storyBegin )
		{

			//如果没有显示小手提示点击老鼠，就显示小手（保证只显示一次）
			if (!showFingerOnMouse)
			{

				ShowFinger(mouse.transform.position);
				showFingerOnMouse=true;
			}

			//如果显示了小手，就可以开始点击老鼠了
			if (showFingerOnMouse) 
			{
				ClickMouseToStruggle();
			}
		}


	}
	void ShowMouse()
	{
		if (mouse ==null) 
		{
			mouse=Manager._instance.mouseGo;
		}
		mouseAnimator=mouse.GetComponent<Animator>();
		mouseAnimator.runtimeAnimatorController=Resources.Load("Animation/WJ/StruggleAnimations/MouseStruggleController") as RuntimeAnimatorController;
		mouseAnimator.CrossFade("idle",0);


		mouse.GetComponentInChildren<BoxCollider2D>().enabled=true;



		if (mouse.GetComponent<Rigidbody2D>()!=null) 
		{
			mouse.GetComponent<Rigidbody2D>().simulated=true;
		}

		if (mouse.GetComponent<Pendulum2D>()==null) 
		{
			mouse.AddComponent<Pendulum2D>();
		}
		else
		{
			mouse.GetComponent<Pendulum2D>().enabled=true;
		}


	}




	void ShowFinger(Vector3 pos)
	{
		BussinessManager._instance.ShowFinger(pos);//这个坐标位置可以灵活设置  ***********

	}

	void ClickMouseToStruggle()
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
						CheckIfItIsNecessaryToDestroyFinger();
						MouseStruggle();
					}
				}
			}


		}
	}

	void CheckIfItIsNecessaryToDestroyFinger()
	{
		//如果没有销毁小手，则销毁小手
		if (BussinessManager._instance.finger!=null) 
		{
			Destroy(BussinessManager._instance.finger);

		}
		
	}


	void MouseStruggle()
	{
		int randomNum=Random.Range(1,4);

		switch (randomNum)
		{
//		case 1:
//
//			mouseAnimator.SetTrigger("first");
//
//			break;
//		case 2:
//			mouseAnimator.SetTrigger("second");
//
//			break;
//		case 3:
//			mouseAnimator.SetTrigger("third");
//
//			break;



		case 1:
			mouseAnimator.CrossFade("",0);
			mouseAnimator.CrossFade("Struggle_01",0);

			break;
		case 2:
			mouseAnimator.CrossFade("",0);
			mouseAnimator.CrossFade("Struggle_02",0);

			break;
		case 3:
			mouseAnimator.CrossFade("",0);
			mouseAnimator.CrossFade("Struggle_03",0);

			break;
		}
	}



	void OnDisable()
	{
		Manager._instance.Reset();

		if (mouse)
		{
			if (mouse.GetComponent<Pendulum2D>()) 
			{
				Destroy(mouse.GetComponent<Pendulum2D>());
			}
			mouseAnimator.runtimeAnimatorController=Resources.Load("Animation/WJ/StandPoseAnimations/MouseStandPoseController") as RuntimeAnimatorController;

			mouse.transform.localRotation=Quaternion.Euler(0,0,0);
		}



		
	}


}
