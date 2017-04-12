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
//		Manager.recordingDone=recordVideo.finishedRecording;
	}


	public void  RecordVideo()
	{
		if (recordVideo.isRecording)
			return;
		
		Debug.Log("RecordVideoWithIvidCapture---- RecordVideo()-----默认录音5秒");
		Debug.Log("Application.dataPath-----"+Application.dataPath);
		Debug.Log("Application.persistentDataPath-----"+Application.persistentDataPath);

		StartCoroutine(recordVideo.RecordForSeconds(5));
	}

	void Update()
	{
		micLoudness = recordVideo.micLoudness;
//		Debug.Log("micLoudness-----"+micLoudness);
		Manager.recordingDone=recordVideo.finishedRecording;
	}
}
