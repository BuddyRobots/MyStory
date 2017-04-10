using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordVideoWithIvidCapture : MonoBehaviour 
{
	public static RecordVideoWithIvidCapture _instance;

	public iVidCapPro vr;

	RecordVideo recordVideo;

	[HideInInspector]
	public float micLoudness;

	void Awake()
	{
		_instance=this;
		recordVideo = new RecordVideo(vr, true);
	}

	public void  RecordVideo()
	{
		Debug.Log("RecordVideoWithIvidCapture---- RecordVideo()-----默认录音5秒");
		StartCoroutine(recordVideo.RecordForSeconds(5));
	}

	void Update()
	{
		micLoudness = recordVideo.micLoudness;

	}
}
