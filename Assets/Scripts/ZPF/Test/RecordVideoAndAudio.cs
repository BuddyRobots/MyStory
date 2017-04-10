using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordVideoAndAudio : MonoBehaviour {

	public Slider slider;

	public iVidCapPro vr;
	int vidWidth = 640;
	int vidHeight = 480;
	int framesRecorded;


	public Camera mainCamera;
	public Camera camera;
	RenderTexture renderTexture;


	int test_frameCount = 0;


	void Awake()
	{
		vr.SetDebug(true);
		renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
		renderTexture.Create();
		//mainCamera.targetTexture = renderTexture;

		camera.targetTexture = renderTexture;
		renderTexture = RenderTexture.active;
	}

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(RecordForSeconds(10));
	}		

	IEnumerator RecordForSeconds(int seconds)
	{
		// Register a delegate to be called when the video is complete.
		vr.RegisterSessionCompleteDelegate(HandleSessionComplete);
		// Register a delegate in case an error occurs during the recording session.
		vr.RegisterSessionErrorDelegate(HandleSessionError);

		#if UNITY_EDITOR
		// "vr" is a reference to the iVidCapPro object.
		vr.BeginRecordingSession(
			"MyRecordedVideo",                         // name of the video
			vidWidth, vidHeight,                       // video width & height in pixels
			30,                                        // frames per second when frame rate Locked/Throttled
			iVidCapPro.CaptureAudio.No_Audio,          // whether or not to record audio
			iVidCapPro.CaptureFramerateLock.Locked);   // capture type: Unlocked, Locked, Throttled
		#elif
		vr.BeginRecordingSession(
			"MyRecordedVideo",                         // name of the video
			vidWidth, vidHeight,                       // video width & height in pixels
			30,                                        // frames per second when frame rate Locked/Throttled
			iVidCapPro.CaptureAudio.Audio_Plus_Mic,    // whether or not to record audio
			iVidCapPro.CaptureFramerateLock.Locked);   // capture type: Unlocked, Locked, Throttled
		#endif

		// Things are happening here that you want to be recorded.
		Debug.Log("RecordVideoAndAudio.cs RecourdForSeconds() : Start record video!");
		yield return new WaitForSeconds(seconds);

		vr.EndRecordingSession(
			iVidCapPro.VideoDisposition.Save_Video_To_Album,  // where to put the finished video 
			out framesRecorded);                        // # of video frames recorded
	}

	// This delegate function is called when the recording session has completed successfully
	// and the video file has been written.
	public void HandleSessionComplete()
	{
		// Do UI stuff when video is complete.
		Debug.Log("RecordVideoAndAudio.cs HandleSessionComplete() : Video record completed!");
	}

	// This delegate function is called if an error occurs during the recording session.
	public void HandleSessionError(iVidCapPro.SessionStatusCode errorCode)
	{
		// Do stuff when an error occurred.
		Debug.LogError("RecordVideoAndAudio.cs HandleSessionError() : Error when record video!");
	}

	void Update()
	{
		float micLoudness = vr.GetMicLoudness(test_frameCount++);
		slider.value = micLoudness*100;
	}
}