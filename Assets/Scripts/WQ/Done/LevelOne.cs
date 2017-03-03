using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelOne : MonoBehaviour 
{
	public static LevelOne _instance;
	private Animation ani;
	bool startStoryStraight=false;//是否可以直接开始故事



	void Awake()
	{
		_instance=this;
	}

	void Start () 
	{
		ani=transform.Find("Grass").GetComponent<Animation>();
		startStoryStraight=false;

	}
	



	void Update () 
	{
		if (!startStoryStraight) 
		{
			ClickTheGrass();
		}


	}


	void ClickTheGrass()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				Debug.Log("当前触摸在UI上");
				return ;
			}
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); 
			if (hit.collider!=null) 
			{
				if (hit.collider.tag=="ClickObj") 
				{
					startStoryStraight=true;

					if (BussinessManager._instance.finger!=null) 
					{


						Destroy(BussinessManager._instance.finger);
						Debug.Log("销毁了小手");

						//TODO....动画播放完以后，出现小老鼠，同时播放旁白,显示字幕  
						StartStory();
					}
				}
			}
		}
	}

	void PlayAnimation()
	{
		ani.Play("GrassScale");
	}

	void PauseAnimation()
	{
		if (ani.isPlaying) 
		{
			ani["GrassScale"].speed=0;
		}

	}
	void ResumeAnimation()
	{
		ani["GrassScale"].speed=1;

	}


	public void StartStory()
	{
		Debug.Log("-----startStory()");
		PlayAnimation();
		FormalScene._instance.ShowSubtitle();
		GameObject.Find("Main Camera").GetComponent<AudioSource>().clip=LevelManager.currentLevelData.AudioAside;//Resources.Load<AudioClip>("Audio/Seagulls");
		GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();


		SubtitleCtrl._instance.Init();
		//字幕-----

	}



	/// <summary>
	/// 暂停旁白
	/// </summary>
	void PauseAudio()
	{

		GameObject.Find("Main Camera").GetComponent<AudioSource>().Pause();
	}

	public void PauseStory()
	{
		//如果在播放动画，就暂停动画；如果在播放旁白，就暂停旁白；如果在切换字幕，就暂停切换
		Debug.Log("-----PauseStory()");

		PauseAnimation();
		PauseAudio();
		SubtitleCtrl._instance.pauseChangeSubtitle=true;


	}


	public void ResumeStory()
	{
		GameObject.Find("Main Camera").GetComponent<AudioSource>().UnPause();//Play();
		ResumeAnimation();
		SubtitleCtrl._instance.pauseChangeSubtitle=false;
	}








}
