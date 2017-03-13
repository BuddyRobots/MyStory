using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleCtrl : MonoBehaviour 
{
	public static SubtitleCtrl _instance;
	public Text subtitleText;
	public CanvasGroup canvasGroup;
	[HideInInspector]
	public bool pauseChangeSubtitle=false;//是否需要暂停字幕切换

	private List<string>  subTitleList=new List<string>();//存储每句话
	private List<float> recordingTimeList=new List<float>();//存储每句话对应的时间

	private float subtitleBgFadingTimer;//淡入淡出计时器

	private bool fadeOut=false;
	private bool fadeIn=false;
	private bool canvasGroupFadingOut;//字幕背景是否要淡出的标记


	private float subtitleChangeTimer=0;//字幕切换计时器
	private int subtitleCountIndex=0;//字幕句数

	float  temp_fadeInTimer=0;
	float  temp_fadeOutTimer=0;

	void Awake()
	{
		_instance=this;

		subTitleList=LevelManager.currentLevelData.SubtitleList;
		recordingTimeList=LevelManager.currentLevelData.SentenceTimeList;
	}

	void OnEnable () 
	{
		Init();
	}
		
	public void Init()
	{
		fadeOut=false;
		fadeIn=false;
		canvasGroupFadingOut=false;
		subtitleBgFadingTimer=0;
		canvasGroup.alpha=1;
		
		pauseChangeSubtitle=false;
		subtitleCountIndex=0;
		subtitleChangeTimer=0;

		temp_fadeInTimer=0;
		temp_fadeOutTimer=0;
	}


	void Update () 
	{
		if (!pauseChangeSubtitle) 
		{

			if (subtitleCountIndex < subTitleList.Count) 
			{
				subtitleText.text=subTitleList[subtitleCountIndex];
				subtitleChangeTimer += Time.deltaTime;
				if (subtitleChangeTimer >= recordingTimeList[subtitleCountIndex]-Constant.SUBTITLE_FADINGTIME) //如果到了淡出的时间
				{
					
					fadeOut=true;
					fadeIn=false;
					if (fadeOut)
					{
						FadeOut();
					}

					if (subtitleCountIndex==subTitleList.Count-1) //如果到了淡出的时间，而且是最后一句（最后一句的时候字幕背景要和字幕一起淡出）
					{
						canvasGroupFadingOut=true;
					}
					if (canvasGroupFadingOut) //字幕背景开始淡出
					{

						subtitleBgFadingTimer+=Time.deltaTime;
						canvasGroup.alpha=Mathf.Lerp(1f,0,subtitleBgFadingTimer/Constant.SUBTITLE_FADINGTIME);
						if (subtitleBgFadingTimer>=Constant.SUBTITLE_FADINGTIME) 
						{



							//字幕切换完毕，屏幕需要变暗再变亮
							FormalScene._instance.screenGrowingDarkAndLight=true;

							canvasGroup.alpha=0;
							subtitleBgFadingTimer=Constant.SUBTITLE_FADINGTIME;
							canvasGroupFadingOut=false;
						}

					}


					if (subtitleChangeTimer>=recordingTimeList[subtitleCountIndex]) //如果当前这一句字幕的时间走完了，字幕要切换成下一句，重新开始计时
					{
						
						subtitleChangeTimer=0;
						subtitleCountIndex++;
						if (subtitleCountIndex<subTitleList.Count) 
						{
							subtitleText.text=subTitleList[subtitleCountIndex];
							fadeIn=true;
							fadeOut=false;
						}

//						if (fadeIn) 
//						{	
//							FadeIn();
//
//						}

					}


				}
				if (fadeIn) 
				{	
					FadeIn();

				}
			}

		}

	}




	//淡出
	void FadeOut()
	{
		temp_fadeOutTimer+=Time.deltaTime;
		Color temp=subtitleText.color;
		temp.a=Mathf.Lerp(1f,0,temp_fadeOutTimer/Constant.SUBTITLE_FADINGTIME);
//		Debug.Log("Fade  out-----temp.a----"+temp.a);

		subtitleText.color=temp;

		if (temp_fadeOutTimer>=Constant.SUBTITLE_FADINGTIME) 
		{
			temp_fadeOutTimer=0;
			fadeOut=false;
		}
	}

	void FadeIn()
	{
		temp_fadeInTimer+=Time.deltaTime;
		Color temp=subtitleText.color;
		temp.a=Mathf.Lerp(0f,1f,temp_fadeInTimer/Constant.SUBTITLE_FADINGTIME);
//		Debug.Log("Fade  In-----temp.a----"+temp.a);
		subtitleText.color=temp;

		if (temp_fadeInTimer>=Constant.SUBTITLE_FADINGTIME) 
		{
			temp_fadeInTimer=0;
			fadeIn=false;
		}
	}

}
