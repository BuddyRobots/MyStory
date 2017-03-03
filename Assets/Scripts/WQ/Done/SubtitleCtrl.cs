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

	private float fadingTimer;//淡入淡出计时器

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
		fadingTimer=0;
		canvasGroup.alpha=1;
		
		pauseChangeSubtitle=false;
		subtitleCountIndex=0;
		subtitleChangeTimer=0;

		temp_fadeInTimer=0;
		temp_fadeOutTimer=0;
	}



//	void Update () 
//	{
//		if (!pauseChangeSubtitle) 
//		{
//			// 字幕的切换
//			ChangeSubtitleLineByLine();
//			// 字幕的淡入淡出
//			FadeInAndOut();
//		}
//			
//	}
//



	void Update () 
	{
		if (!pauseChangeSubtitle) 
		{

			if (subtitleCountIndex<subTitleList.Count) 
			{
				subtitleText.text=subTitleList[subtitleCountIndex];
				subtitleChangeTimer+=Time.deltaTime;
				if (subtitleChangeTimer>=recordingTimeList[subtitleCountIndex]-Constant.SUBTITLE_FADINGTIME) //如果到了淡出的时间
				{
					
					fadeOut=true;
					if (fadeOut)
					{
						FadeOut();
					}

					if (subtitleCountIndex==subTitleList.Count-1) //如果到了淡出的时间，而且是最后一句（最后一句的时候字幕背景要和字幕一起淡出）
					{
						canvasGroupFadingOut=true;
					}
					if (canvasGroupFadingOut) 
					{

						fadingTimer+=Time.deltaTime;
						canvasGroup.alpha=Mathf.Lerp(1f,0,fadingTimer/Constant.SUBTITLE_FADINGTIME);
						if (fadingTimer>=Constant.SUBTITLE_FADINGTIME) 
						{
							canvasGroup.alpha=0;
							fadingTimer=Constant.SUBTITLE_FADINGTIME;
							canvasGroupFadingOut=false;
						}

					}


					if (subtitleChangeTimer>=recordingTimeList[subtitleCountIndex]) 
					{
						
						subtitleChangeTimer=0;
						subtitleCountIndex++;
						if (subtitleCountIndex<subTitleList.Count) 
						{
							subtitleText.text=subTitleList[subtitleCountIndex];

						}

						if (subtitleCountIndex<=subTitleList.Count-1)
						{
							fadeIn=true;

						}
						if (fadeIn) 
						{	
							FadeIn();

						}

					}

				}
			}

		}

	}


	void ChangeSubtitleLineByLine()
	{

		if (subtitleCountIndex<subTitleList.Count) 
		{
			subtitleText.text=subTitleList[subtitleCountIndex];
			subtitleChangeTimer+=Time.deltaTime;
			if (subtitleChangeTimer>=recordingTimeList[subtitleCountIndex]-Constant.SUBTITLE_FADINGTIME) 
			{
				Debug.Log("time to fade out ----");
				fadeOut=true;
//				fadeIn=false;
				if (subtitleCountIndex==subTitleList.Count-1) //最后一句的时候字幕背景要和字幕一起淡出
				{
					
						canvasGroupFadingOut=true;
					

				}
				if (subtitleChangeTimer>=recordingTimeList[subtitleCountIndex]) 
				{
					Debug.Log("time to fade in-----");
					subtitleChangeTimer=0;
					subtitleCountIndex++;
					if (subtitleCountIndex<=subTitleList.Count-1)
					{
						fadeIn=true;
//						fadeOut=false;
					}
				}
			}
		}
	}





//	void FadeInAndOut()
//	{
//		if (fadeOut)
//		{
//			FadeOut();
//		}
//		if (fadeIn) 
//		{
//			FadeIn();	
//		}
//		if (canvasGroupFadingOut) 
//		{
//			canvasGroupFadingOut=false;
//			
//			fadingTimer+=Time.deltaTime;
//			canvasGroup.alpha=Mathf.Lerp(1f,0,fadingTimer/Constant.SUBTITLE_FADINGTIME);
//			if (fadingTimer>=Constant.SUBTITLE_FADINGTIME) 
//			{
//				Debug.Log("背景淡出---");
//				fadingTimer=Constant.SUBTITLE_FADINGTIME;
//			}
//
//		}
//
//	}
//


	//淡出
	void FadeOut()
	{
		temp_fadeOutTimer+=Time.deltaTime;
		Color temp=subtitleText.color;
		temp.a=Mathf.Lerp(1f,0,temp_fadeOutTimer/Constant.SUBTITLE_FADINGTIME);
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
		temp.a=Mathf.Lerp(1f,0,temp_fadeInTimer/Constant.SUBTITLE_FADINGTIME);
		subtitleText.color=temp;

		if (temp_fadeInTimer>=Constant.SUBTITLE_FADINGTIME) 
		{
			temp_fadeInTimer=0;
			fadeIn=false;
		}
	}

}
