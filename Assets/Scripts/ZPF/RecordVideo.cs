using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RecordVideo {

	float _micLoudness;
	public float micLoudness
	{
		get
		{
			if (!vr)
			{
				Debug.LogError("RecordVideo.cs micLoudness : iVidCapPro not initialized!");
				return 0;
			}
			return vr.GetMicLoudness();
		}
	}

	public bool finishedRecording = false;
		
	iVidCapPro vr;
	int framesRecorded;
	bool setDebug;


	public RecordVideo(iVidCapPro _iVidCapPro, bool _setDebug = false)
	{
		vr = _iVidCapPro;
		setDebug = _setDebug;
		vr.SetDebug(_setDebug);
	}		

	public IEnumerator RecordForSeconds(int seconds, string name = "MyRecordedVideo", int vidWidth = 640, int vidHeight = 480)
	{
		finishedRecording = false;

		// Register a delegate to be called when the video is complete.
		vr.RegisterSessionCompleteDelegate(HandleSessionComplete);
		// Register a delegate in case an error occurs during the recording session.
		vr.RegisterSessionErrorDelegate(HandleSessionError);

		#if UNITY_EDITOR
		// "vr" is a reference to the iVidCapPro object.
		vr.BeginRecordingSession(
			name,                                      // name of the video
			vidWidth, vidHeight,                       // video width & height in pixels
			30,                                        // frames per second when frame rate Locked/Throttled
			iVidCapPro.CaptureAudio.No_Audio,          // whether or not to record audio
			iVidCapPro.CaptureFramerateLock.Locked);   // capture type: Unlocked, Locked, Throttled
		#elif UNITY_IOS
		vr.BeginRecordingSession(
		    name,                                      // name of the video
			vidWidth, vidHeight,                       // video width & height in pixels
			30,                                        // frames per second when frame rate Locked/Throttled
			iVidCapPro.CaptureAudio.Audio_Plus_Mic,    // whether or not to record audio
			iVidCapPro.CaptureFramerateLock.Locked);   // capture type: Unlocked, Locked, Throttled
		#endif

		// Things are happening here that you want to be recorded.
		if (setDebug)
			Debug.Log("RecordVideo.cs RecourdForSeconds() : Start record video!");
		yield return new WaitForSeconds(seconds);

		vr.EndRecordingSession(
			iVidCapPro.VideoDisposition.Save_Video_To_Album,  // where to put the finished video 
			out framesRecorded);                              // # of video frames recorded
	}

	// This delegate function is called when the recording session has completed successfully
	// and the video file has been written.
	public void HandleSessionComplete()
	{
		// Do UI stuff when video is complete.
		finishedRecording = true;
		Debug.Log("RecordVideoAndAudio.cs HandleSessionComplete() : Video record completed!");
	}

	// This delegate function is called if an error occurs during the recording session.
	public void HandleSessionError(iVidCapPro.SessionStatusCode errorCode)
	{
		// Do stuff when an error occurred.
		Debug.LogError("RecordVideoAndAudio.cs HandleSessionError() : Error when record video!");
	}
}