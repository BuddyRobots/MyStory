using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSeven : MonoBehaviour 
{

	public static LevelSeven _instance;

	GameObject mouse;

	Animator mouseAnimator;

	public Transform originMouseTrans;
	public Transform destMouseTrans;

	[HideInInspector]
	public float speed;

	[HideInInspector]
	public bool walkToLionAniOver;
	[HideInInspector]
	public bool aniDone;

	private bool walkAniPlayed;
	private bool move;
	private bool showFingerOnMouse;
	private bool mouseClicked;
	private bool audioAsidePlayed;
	private bool talkAniPlayed;


	void Awake()
	{
		_instance=this;

	}


	void Start() 
	{
		FormalScene._instance.nextBtn.gameObject.SetActive(false);
		Manager.storyStatus=StoryStatus.Normal;

		Init();

		ShowMouse();
	}

	public void InitTest()
	{
		FormalScene._instance.nextBtn.gameObject.SetActive(false);
		Manager.storyStatus=StoryStatus.Normal;

		Init();

		ShowMouse();
		if (BussinessManager._instance.finger) {
			Destroy(BussinessManager._instance.finger);
		}

	}

	void Init()
	{
		speed=1.26f;
		walkToLionAniOver=false;
		aniDone=false;
		walkAniPlayed=false;
		mouseClicked=false;
		audioAsidePlayed=false;
		talkAniPlayed=false;
		showFingerOnMouse=false;
		move=true;//一开始老鼠就是可以移动的
	}


	void Reset()
	{
		Init();
		mouseAnimator.speed=1;
		mouse.transform.position=originMouseTrans.position;

		mouseAnimator.CrossFade("",0);
		mouseAnimator.CrossFade("WalkToLion_level7",0);

	}

	void Update () 
	{
		if (FormalScene._instance.storyBegin) 
		{


			//老鼠播放走的动画
			if (!walkAniPlayed) 
			{
				walkAniPlayed=true;
				mouseAnimator.CrossFade("",0);

				mouseAnimator.CrossFade("WalkToLion_level7",0);

			}
			if (walkToLionAniOver) //第一个动画播放完了，如果是正常情况就出现小手，点击了小手播放第二个动画；如果是非正常状态下，就直接播放第二个动画
			{


				if (Manager.storyStatus==StoryStatus.Normal) 
				{
					if (!showFingerOnMouse) 
					{
						ShowFinger(mouse.transform.position);
						showFingerOnMouse=true;
					}
					if (showFingerOnMouse) 
					{
						if (!mouseClicked) 
						{
							ClickMouse();
						}
						
					}
				}
				else if (Manager.storyStatus ==StoryStatus.Recording || Manager.storyStatus ==StoryStatus.PlayRecord) 
				{
					//直接播放动画

					if (!talkAniPlayed) 
					{
						mouseAnimator.CrossFade("Talk",0);

						FormalScene._instance.ShowSubtitle();

						talkAniPlayed=true;
					}

				}
				if (mouseClicked) 
				{
				
					//正常情况下直接播放动画，旁白，字幕

					if (Manager.storyStatus==StoryStatus.Normal)
					{

						if (!audioAsidePlayed)
						{
							//播放旁白 ，显示字幕
							BussinessManager._instance.PlayAudioAside();

							audioAsidePlayed=true;
						}

						if (!talkAniPlayed) 
						{
							mouseAnimator.CrossFade("Talk",0);
							FormalScene._instance.ShowSubtitle();

							talkAniPlayed=true;
						}


					}

				}

			//动画和字幕都结束后自动切换场景
				if (aniDone && Manager._instance.isSubtitleShowOver) 
				{
					//在正常状态或者播放状态下
					if (Manager.storyStatus ==StoryStatus.Normal || Manager.storyStatus ==StoryStatus.PlayRecord)
					{
						FormalScene._instance.ChangeSceneAutomatically();

						mouseAnimator.CrossFade("idle",0);

						aniDone=false;

					}
				}
			
			}

			MouseMove();

		}
	}




	void LateUpdate()
	{
		mouse.transform.localRotation=Quaternion.Euler(0,180,0);

	}


	void MouseMove()
	{
		if (move) 
		{
			mouse.transform.Translate(Vector3.left*speed*Time.deltaTime);

			if (mouse.transform.position.x>=destMouseTrans.position.x) 
			{
				mouse.transform.position=destMouseTrans.position;
				move=false;
			}

		}

	}

	void ShowFinger(Vector3 pos)
	{
		BussinessManager._instance.ShowFinger(pos);//这个坐标位置可以灵活设置  ***********

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
						BussinessManager._instance.DestroyFinger();
						mouseClicked=true;
					}
				}
			}
		}

	}

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

	public void PauseStory()
	{
		//如果在播放动画，就暂停动画；
		//如果在播放旁白，就暂停旁白；
		//如果有字幕，且在切换字幕，就暂停切换
		PauseAnimation();
		BussinessManager._instance.PauseAudioAside();
		SubtitleShow._instance.pause=true;
		move=false;

	}
	public void ResumeStory()
	{
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


	void ShowMouse()
	{
		if (mouse==null) 
		{
			mouse=Manager._instance.mouseGo;
		}
		mouse.transform.position=originMouseTrans.position;

		mouseAnimator=mouse.GetComponent<Animator>();
		mouseAnimator.CrossFade("",0);
		mouseAnimator.CrossFade("idle",0);
		mouseAnimator.speed=1;


		if (mouse.GetComponent<MouseCtrl>()==null) 
		{
			mouse.AddComponent<MouseCtrl>();
		}

		if (mouse.GetComponent<Rigidbody2D>()==null) 
		{
			mouse.AddComponent<Rigidbody2D>();
		}
		mouse.GetComponent<Rigidbody2D>().gravityScale=0;
		mouse.GetComponent<Rigidbody2D>().constraints=RigidbodyConstraints2D.FreezeRotation;
		mouse.GetComponent<Rigidbody2D>().simulated=true;

	}

	void OnDisable()
	{
		if (Manager._instance)
		{
			Manager._instance.Reset();

		}
		if (mouseAnimator) 
		{
			mouseAnimator.CrossFade("idle",0);

		}
		if (mouse.GetComponent<MouseCtrl>()!=null) 
		{
			Destroy(mouse.GetComponent<MouseCtrl>());
		}
	}

}
