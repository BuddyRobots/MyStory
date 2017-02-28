using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleFadeInAndOut : MonoBehaviour 
{
	public Text subtitleText;
	public CanvasGroup canvasGroup;

	private List<string>  subTitleList=new List<string>();//存储每句话
	private List<float> recordingTimeList=new List<float>();//存储每句话对应的时间

	private float fadingTimer;//淡入淡出计时器

	private bool fadeOut=false;
	private bool fadeIn=false;
	private bool canvasGroupFadingOut;//字幕背景是否要淡出的标记


	void Awake()
	{
		subTitleList=LevelManager.currentLevelData.SubtitleList;
		recordingTimeList=LevelManager.currentLevelData.SentenceTimeList;

	}

	void Start () 
	{
		StartCoroutine(ChangeSubtitle());
	
	}


	/// <summary>
	/// 供外部调用的切换字幕的方法
	/// </summary>
	public void ChangeSubtitleLineByLine()
	{
		StartCoroutine(ChangeSubtitle());

	}

	//切换字幕
	IEnumerator ChangeSubtitle()
	{
		for (int i = 0; i < subTitleList.Count; i++) 
		{
			subtitleText.text=subTitleList[i];
			yield return new WaitForSeconds(recordingTimeList[i]-Constant.SUBTITLE_FADINGTIME);
			fadeOut=true;
			if (i==subTitleList.Count-1) 
			{
				canvasGroupFadingOut=true;
			}
			yield return new WaitForSeconds(Constant.SUBTITLE_FADINGTIME);
			if (i<subTitleList.Count-1)
			{
				fadeIn=true;
			}

		}
	}



	void Update () 
	{
		if (fadeOut)
		{
			FadeOut();
			fadeOut=false;
		}
		if (fadeIn) 
		{
			FadeIn();
			fadeIn=false;	
		}
		if (canvasGroupFadingOut) 
		{
			fadingTimer+=Time.deltaTime;
			canvasGroup.alpha=Mathf.Lerp(1f,0,fadingTimer/Constant.SUBTITLE_FADINGTIME);
			if (fadingTimer>=Constant.SUBTITLE_FADINGTIME) 
			{
				fadingTimer=0;
				canvasGroupFadingOut=false;
			}

		}
	}


	//淡出
	void FadeOut()
	{
		subtitleText.CrossFadeAlpha(0,Constant.SUBTITLE_FADINGTIME,true);
	}

	void FadeIn()
	{
		
		subtitleText.CrossFadeAlpha(1,Constant.SUBTITLE_FADINGTIME,true);
	}




}
