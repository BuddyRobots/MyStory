using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordVideoWithIvidCapture : MonoBehaviour 
{
	public static RecordVideoWithIvidCapture _instance;

	public iVidCapPro vr;

	public VideoRecorder recordVideo;

	[HideInInspector]
	public float micLoudness;

	void Awake()
	{
		_instance=this;
		recordVideo = new VideoRecorder(vr, true);

	}

	void Start()
	{
		//构造完recordVideo后传递委托 formalscene 中的 ShowRecordDone()

		recordVideo.RegisterSessionCompleteDelegate(FormalScene._instance.ShowRecordDone);

	}


	public void  RecordVideo()
	{
		if (recordVideo.isRecording)
			return;

		StartCoroutine(recordVideo.RecordForSeconds(LevelManager.currentLevelData.RecordTime));
	}

	void Update()
	{
		micLoudness = recordVideo.micLoudness;
//		Manager.recordingDone=recordVideo.finishedRecording;
	}
}
