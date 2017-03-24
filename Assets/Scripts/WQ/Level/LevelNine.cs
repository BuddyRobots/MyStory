using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelNine : MonoBehaviour 
{

	public static LevelNine _instance;
	public Transform originMousePos;
	public Transform originGarlandPos;
	public Transform originHandPos;

	GameObject mouse;
	GameObject garland;

	GameObject hands;

    Animator mouseAnimator;

	bool showFingerOnMouse;
	bool mouseClicked;
	bool move;
	bool aniPlayed;
	bool audioAsidePlayed;


	int garlandFrontLayer;//花环在老鼠头上时的层数
	int garlandBackLayer;//花环在后面时的层数
	int garlandInveisibleLayer;//花环隐藏时的层数

	Vector3 handDestPos;
	Vector3 garlandDesPos;
	Vector3 offset;


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

		Init();
	}

	void Init()
	{

		showFingerOnMouse=false;
		mouseClicked=false;
		move=false;
		aniPlayed=false;
		audioAsidePlayed=false;

		//花环的层要还原
		SetGarlandLayer(garlandInveisibleLayer);


		//手的位置还原  to do.....




		if (Manager.storyStatus ==StoryStatus.Normal) 
		{
			showFingerOnMouse=false;
		}
		else if (Manager.storyStatus ==StoryStatus.Recording || Manager.storyStatus ==StoryStatus.PlayRecord)
		{
			showFingerOnMouse=true;
		}

		if (mouseAnimator!=null) 
		{
			mouseAnimator.speed=1;
		}

	}
	

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

			if (mouseClicked)
			{
				if (Manager.storyStatus==StoryStatus.Normal)
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



			//如果动画播放完了，字幕也显示完了，就跳转界面   to do....







			
		}
	}



	void MoveTo()
	{
		//手变换图片，并开始移动


		//花环改变层，并移动



		//如果花环移动到了目的地，就改变花环的层数


	}




	void PlayAnimation()
	{
		//老鼠播放动画，
		mouseAnimator.CrossFade("Hooray",0);



	}
	/// <summary>
	/// 点击播放按钮，开启场景故事（有播放录音，有字幕，或者还有动画）
	/// </summary>
	public void PlayStoryWithAudioRecording()
	{

		Reset();

	}

	public void StartStoryToRecordAudioAndVideo()
	{
		//如果有小手提示点击，就销毁小手，点击失效 

		if (BussinessManager._instance.finger!=null) 
		{
			Destroy(BussinessManager._instance.finger);

		}

		Reset();

	}
	void Reset()
	{
		Init();

		mouseAnimator.CrossFade("",0);
		mouseAnimator.CrossFade("Hooray",0);
	
		mouse.transform.position=originMousePos.position;

	}


	public void PauseStory()
	{
		//如果在播放动画，就暂停动画；
		//如果在播放旁白，就暂停旁白；
		//如果有字幕，且在切换字幕，就暂停切换
		PauseAnimation();
		BussinessManager._instance.PauseAudioAside();
		SubtitleShow._instance.pause=true;
//		pause=true;
		move=false;

	}

	public void ResumeStory()
	{
//		pause=false;
		move=true;
		BussinessManager._instance.ResumeAudioAside();
		ResumeAnimation();
		SubtitleShow._instance.pause=false;
	}

	void PauseAnimation()
	{
		mouseAnimator.speed=0;
	
	}
	void ResumeAnimation()
	{
		mouseAnimator.speed=1;

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
		if (mouse==null) 
		{
			mouse=Manager._instance.mouseGo;
		}

		mouse.transform.position=originMousePos.position;
		mouseAnimator=mouse.GetComponent<Animator>();


	}

	void ShowGarland()
	{
		if (garland==null) 
		{
			garland=Manager._instance.garland;
		}
		garland.transform.position=originGarlandPos.position;

		garlandDesPos=GameObject.Find("Mouse/GarlandDest").transform.position;

		offset=garlandDesPos-garland.transform.position;

		handDestPos=originHandPos.position+offset;

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
