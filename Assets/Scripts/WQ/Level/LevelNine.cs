using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelNine : MonoBehaviour 
{

	public static LevelNine _instance;
	public Transform originMousePos;
	public Transform garlandOriginPos;

	GameObject mouse;
	GameObject garland;

	GameObject hands;

    Animator mouseAnimator;

	bool showFingerOnMouse;
	bool mouseClicked;

	int garlandFrontLayer;//花环在老鼠头上时的层数
	int garlandBackLayer;//花环在后面时的层数
	int garlandInveisibleLayer;//花环隐藏时的层数
	void Awake()
	{

		_instance=this;
	}



	void Start () 
	{
		garlandFrontLayer=40;
		garlandBackLayer=5;	
		garlandInveisibleLayer=0;

		ShowMouse();
		ShowGarland();


		showFingerOnMouse=false;
		mouseClicked=false;

		Init();
	}

	void Init()
	{


		//花环的层要还原
		SetGarlandLayer(garlandInveisibleLayer);
		//手的位置还原
	}
	
	bool aniPlayed;
	bool audioAsidePlayed;

	void Update () 
	{
		if (FormalScene._instance.storyBegin) 
		{
			if (Manager.storyStatus==StoryStatus.Normal) 
			{
				if (!showFingerOnMouse) 
				{
					showFingerOnMouse=true;
					ShowFinger(mouse.transform.position);
				}
				if (showFingerOnMouse) 
				{
					if (!mouseClicked) 
					{
						ClickMouse();
					}
				}

				if (mouseClicked)
				{
					if (!audioAsidePlayed) 
					{
						//播放旁白 ，显示字幕
						BussinessManager._instance.PlayAudioAside();
						audioAsidePlayed=true;
					}

					FormalScene._instance.ShowSubtitle();

					//播放动画
					if (!aniPlayed) 
					{
						Debug.Log("开始播放动画");
						PlayAnimation();

						aniPlayed=true;
					}

				}

			}
			else if (Manager.storyStatus==StoryStatus.Recording || Manager.storyStatus ==StoryStatus.PlayRecord) 
			{
				
				if (!aniPlayed) 
				{

					//播放动画.......

					PlayAnimation();

					aniPlayed=true;
				}


			}



			//如果动画播放完了，字幕也显示完了，就跳转界面   to do....







			
		}
	}

	void PlayAnimation()
	{
		//狮子的手切换图片，花环显示，
		//狮子的手往老鼠的头上移动
		//老鼠播放动画，



	}

	void ClickMouse()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				return ;
			}
			//用这种方法来判断是否点击了对象比较准确些
			Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			if (col.Length>0) 
			{
				foreach (Collider2D c in col) 
				{
					if (c.tag=="Player") 
					{
						if (BussinessManager._instance.finger!=null) 
						{
							Destroy(BussinessManager._instance.finger);

						}
						mouseClicked=true;

					}
				}
			}
		}

	}

	void ShowMouse()
	{
		if (mouse==null) {
			mouse=Manager._instance.mouseGo;
		}

		mouse.transform.position=originMousePos;
		mouseAnimator=mouse.GetComponent<Animator>();


	}

	void ShowGarland()
	{
		if (garland==null) 
		{
			garland=Manager._instance.garland;
		}
		garland.transform.localPosition=garlandOriginPos;

	}
	void ShowFinger(Vector3 pos)
	{
		BussinessManager._instance.ShowFinger(pos);//这个坐标位置可以灵活设置  

	}

	void SetGarlandLayer(int layerNum)
	{
		garland.GetComponent<SpriteRenderer>().sortingOrder=layerNum;
	}


}
