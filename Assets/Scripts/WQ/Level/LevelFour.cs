using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 老鼠被狮子抓住，点击老鼠随机播放一个挣扎动画，并成钟摆运动
/// </summary>
public class LevelFour : MonoBehaviour 
{
	public static LevelFour _instance;

	[HideInInspector]
    public 	GameObject mouse;

	bool storyBegin;//故事是否开始的标志

	Animator mouseAnimator;

	Vector3 originMousePos=new Vector3(-0.3f,2f,0);
	Vector3 outsidePos=new Vector3(50f,50f,0);

	[HideInInspector]
	public bool showFingerOnMouse;//是否出现小手提示点击老鼠



	void Awake()
	{
		_instance=this;
	}

	void Start () 
	{
		FormalScene._instance.recordBtn.gameObject.SetActive(false);
		ShowMouse();
	}

	void ShowMouse()
	{
		if (mouse ==null) 
		{
			
			mouse=Instantiate(Resources.Load("Prefab/Mouse_UpsideDown")) as GameObject;//code for test




//			mouse=Manager._instance.mouseGo;// real code 
			//拿到老鼠后，得手动给老鼠添加碰撞体，Animator组件
			// to do....
			//
			//
			//
			//






			if (mouse==null) 
			{
				Debug.Log("老鼠为空");
			}
			mouse.transform.localPosition=originMousePos;
			mouse.name="Mouse";
			mouseAnimator=mouse.GetComponent<Animator>();

			GameObject.DontDestroyOnLoad(mouse);

		}
	}


	void ShowFinger(Vector3 pos)
	{
		BussinessManager._instance.ShowFinger(pos);//这个坐标位置可以灵活设置  ***********

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


	void ClickMouseToStruggle()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				Debug.Log("当前点击是在UI 上");
				return ;
			}
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); 
			if (hit.collider!=null) 
			{
				if (hit.collider.tag=="Player") 
				{
					
					CheckIfItIsNecessaryToDestroyFinger();
					MouseStruggle();
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

		Debug.Log("----"+randomNum);

		switch (randomNum)
		{
		case 1:

			mouseAnimator.SetTrigger("first");

			break;
		case 2:
			mouseAnimator.SetTrigger("second");

			break;
		case 3:
			mouseAnimator.SetTrigger("third");

			break;
		}
	}


	void OnDestroy()
	{

		mouse.transform.localPosition=outsidePos;
	}


}
