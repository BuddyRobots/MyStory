using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelNine : MonoBehaviour 
{

	public static LevelNine _instance;
	public Transform originMouseTrans;
	public Transform originGarlandTran;
	public Transform originHandTran;

	GameObject mouse;
	GameObject garland;

	public GameObject hand;

    Animator mouseAnimator;

	bool showFingerOnMouse;
	bool mouseClicked;
	bool aniPlayed;
	bool audioAsidePlayed;
	bool pause;
	bool isOver;
	[HideInInspector]
	public bool aniDone;

	int garlandFrontLayer;//花环在老鼠头上时的层数
	int garlandBackLayer;//花环在后面时的层数
	int garlandInveisibleLayer;//花环隐藏时的层数

	float moveSpeed;//这个速度应该是 offset/老鼠动画的时间

	Vector3 originHandPos;
	Vector3 destHandPos;
	Vector3 desGarlandPos;
	Vector3 moveOffset;

	void Awake()
	{

		_instance=this;
	}

	void Start () 
	{

		Manager.storyStatus=StoryStatus.Normal;
		FormalScene._instance.nextBtn.gameObject.SetActive(false);

		garlandFrontLayer=40;
		garlandBackLayer=5;	
		garlandInveisibleLayer=0;
		originHandPos=originHandTran.position;

		ShowMouse();
		ShowGarland();


		Init();
	}

	void Init()
	{

		showFingerOnMouse=false;
		aniPlayed=false;
		audioAsidePlayed=false;
		pause=false;

		if (Manager.storyStatus ==StoryStatus.Normal) 
		{
			showFingerOnMouse=false;
			isOver=true;
			mouseClicked=false;


		}
		else if (Manager.storyStatus ==StoryStatus.Recording || Manager.storyStatus ==StoryStatus.PlayRecord)
		{
			showFingerOnMouse=true;
			isOver=false;
			mouseClicked=true;


		}


		//这里一定要把动画的速度还原为1，不然动画会卡住不动
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
			if (mouseClicked)
			{
				if (Manager.storyStatus==StoryStatus.Normal)
				{
					if (!audioAsidePlayed) 
					{
						isOver=false;
						//播放旁白 ，显示字幕
						BussinessManager._instance.PlayAudioAside();
						audioAsidePlayed=true;
					}
				}
				//播放动画
				if (!aniPlayed) 
				{
					//改变手的图片并显示花环
					ChangeHandSpriteAndShowGarland();
					PlayAnimation();

					aniPlayed=true;
				}
				FormalScene._instance.ShowSubtitle();

			}

			MoveTo();

			if(aniDone && Manager._instance.isSubtitleShowOver)
			{
				//在正常状态或者播放状态下
				if (Manager.storyStatus ==StoryStatus.Normal || Manager.storyStatus ==StoryStatus.PlayRecord)
				{
					FormalScene._instance.ChangeSceneAutomatically();
					mouseAnimator.CrossFade("idle",0);
				}	
				aniDone=false;

			}
	
		}
	}



	void MoveTo()
	{

		if (!pause) 
		{
			if(!isOver)
			{
				//手开始移动
				//花环并移动
				//如果花环移动到了目的地，就改变花环的层数
				hand.transform.position+= moveOffset.normalized * moveSpeed * Time.deltaTime;
				garland.transform.position+=moveOffset.normalized * moveSpeed * Time.deltaTime;
//				Debug.Log("-------"+Vector3.Distance(desGarlandPos, garland.transform.position));
				if(Vector3.Distance(desGarlandPos, garland.transform.position)<=0.1f)
				{
					isOver = true;
					hand.transform.position = destHandPos;
					garland.transform.position = desGarlandPos;
					SetGarlandLayer(garlandFrontLayer);
				}
			
			}
		}
	}

	void ChangeHandSpriteAndShowGarland()
	{
		hand.GetComponent<SpriteRenderer>().sprite=Resources.Load<Sprite>("Pictures/Hand/hand_1");
		SetGarlandLayer(garlandBackLayer);
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
		//有Animaator组件的话，一定要把Animator.speed置为1，不然动画会卡住不动，Init中已经设置过了，所以这里不用再重复
		Init();

		//老鼠动画还原，位置还原
		mouseAnimator.CrossFade("",0);
		mouseAnimator.CrossFade("Hooray",0);
		mouse.transform.position=originMouseTrans.position;
	
		//手的位置还原，图片还原
		hand.transform.position=originHandPos;
		hand.GetComponent<SpriteRenderer>().sprite=Resources.Load<Sprite>("Pictures/Hand/hand_1");

		//花环的位置和层还原
		garland.transform.position=originGarlandTran.position;
		SetGarlandLayer(garlandBackLayer);

	}


	public void PauseStory()
	{
		//如果在播放动画，就暂停动画；
		//如果在播放旁白，就暂停旁白；
		//如果有字幕，且在切换字幕，就暂停切换
		PauseAnimation();
		BussinessManager._instance.PauseAudioAside();
		SubtitleShow._instance.pause=true;
	}

	public void ResumeStory()
	{
		BussinessManager._instance.ResumeAudioAside();
		ResumeAnimation();
		SubtitleShow._instance.pause=false;
	}

	void PauseAnimation()
	{
		mouseAnimator.speed=0;
		pause=true;
	
	}
	void ResumeAnimation()
	{
		mouseAnimator.speed=1;
		pause=false;

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

		mouse.transform.position=originMouseTrans.position;
		mouseAnimator=mouse.GetComponent<Animator>();
		if (mouse.GetComponent<MouseCtrl>()==null) {
			mouse.AddComponent<MouseCtrl>();
		}

	}

	void ShowGarland()
	{
		if (garland==null) 
		{
			garland=Manager._instance.garland;
		}
		garland.transform.position=originGarlandTran.position;
		SetGarlandLayer(garlandInveisibleLayer);

		desGarlandPos=GameObject.Find("Mouse/GarlandDest").transform.position;
		Debug.Log("garland.transform.position--"+garland.transform.position);
		Debug.Log("desGarlandPos----"+desGarlandPos);

		moveOffset=desGarlandPos-garland.transform.position;

		destHandPos=originHandTran.position+moveOffset;

		//速度=offset/老鼠动画的时间 
		moveSpeed=(Vector3.Distance(destHandPos, originHandPos))/6.6f;

	}

	void ShowFinger(Vector3 pos)
	{
		BussinessManager._instance.ShowFinger(pos);//这个坐标位置可以灵活设置  

	}

	void SetGarlandLayer(int layerNum)
	{
		garland.GetComponent<SpriteRenderer>().sortingOrder=layerNum;
	}

	void OnDisable()
	{

		Manager._instance.Reset();
		mouseAnimator.CrossFade("idle",0);
		if (mouse.GetComponent<MouseCtrl>()!=null) 
		{
			Destroy(mouse.GetComponent<MouseCtrl>());
		}

	}

}
