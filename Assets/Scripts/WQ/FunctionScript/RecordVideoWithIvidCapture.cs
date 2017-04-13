using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordVideoWithIvidCapture : MonoBehaviour 
{
	public static RecordVideoWithIvidCapture _instance;

	public iVidCapPro vr;

	VideoRecorder recordVideo;

	[HideInInspector]
	public float micLoudness;

	void Awake()
	{
		_instance=this;
		recordVideo = new VideoRecorder(vr, true);
		//构造完recordVideo后注册委托 formalscene 中的 ShowRecordDone()
		//TODO

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
		Manager.recordingDone=recordVideo.finishedRecording;
	}
}
