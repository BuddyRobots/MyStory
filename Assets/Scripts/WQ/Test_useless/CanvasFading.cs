using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasFading : MonoBehaviour {



	public CanvasGroup canvasGroup;
	public Text subtitleText;

	List<string>  subTitleList=new List<string>();//存储每句话
	List<float> recordingTimeList=new List<float>();//存储每句话对应的时间

	private float recordingTime;
	private float recordingTimer;
	const float SUBTITLE_FADINGTIME=2f;

	private float addingTime;

	private float preLastingTime;//每句话持续的时间

	private int index;

	bool fadeOut=false;
	bool fadeIn=false;



	void Awake()
	{
		string temp_0="小老鼠踢皮球，皮球滚来滚去。。";
		string temp_1="狮子把小老鼠抓住了，准备吃了小老鼠。";
		string temp_2="小老鼠和狮子成了好朋友";

		subTitleList.Add(temp_0);
		subTitleList.Add(temp_1);
		subTitleList.Add(temp_2);


		recordingTimeList.Add(3f);
		recordingTimeList.Add(8f);
		recordingTimeList.Add(5f);

		for (int i = 0; i < recordingTimeList.Count; i++) 
		{
			recordingTime +=recordingTimeList[i];
		}

	}

	void Start () 
	{
		StartCoroutine(ChangeSubtitle());

	}

	//切换字幕
	IEnumerator ChangeSubtitle()
	{
		for (int i = 0; i < subTitleList.Count; i++) 
		{
			preLastingTime=recordingTimeList[i];
			subtitleText.text=subTitleList[i];
			yield return new WaitForSeconds(recordingTimeList[i]-SUBTITLE_FADINGTIME);
			fadeOut=true;
			yield return new WaitForSeconds(SUBTITLE_FADINGTIME);
			if (i<subTitleList.Count-1)
			{
				fadeIn=true;
			}
		}
	}


	float fadingTimer;

	void Update () 
	{
		if (fadeOut)
		{
			fadingTimer+=Time.deltaTime;
			canvasGroup.alpha=Mathf.Lerp(1f,0,fadingTimer/SUBTITLE_FADINGTIME);
			if (fadingTimer>=SUBTITLE_FADINGTIME) 
			{
				fadingTimer=0;
				fadeOut=false;
				
			}

		}
		if (fadeIn) 
		{
			fadingTimer+=Time.deltaTime;
			canvasGroup.alpha=Mathf.Lerp(0,1f,fadingTimer/SUBTITLE_FADINGTIME);
			if (fadingTimer>=SUBTITLE_FADINGTIME) 
			{
				fadingTimer=0;
				fadeIn=false;	

			}

		}
	}




}
