using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


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
	iVidCapPro.VideoDisposition videoPosition;
	int framesRecorded;
	bool setDebug;


	public RecordVideo(iVidCapPro _iVidCapPro, bool _setDebug = false)
	{
		vr = _iVidCapPro;
		videoPosition = iVidCapPro.VideoDisposition.Save_Video_To_Documents;
		framesRecorded = 0;
		setDebug = _setDebug;

		vr.SetDebug(_setDebug);
	}		

	public IEnumerator RecordForSeconds(int seconds, 
		iVidCapPro.VideoDisposition _videoPosition = iVidCapPro.VideoDisposition.Save_Video_To_Documents,
		string name = "MyRecordedVideo", int vidWidth = 640, int vidHeight = 480)
	{
		finishedRecording = false;
		videoPosition = _videoPosition;

		vr.RegisterSessionCompleteDelegate(HandleSessionComplete);
		vr.RegisterSessionErrorDelegate(HandleSessionError);

		#if UNITY_EDITOR
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
			Debug.Log("c.cs RecourdForSeconds() : Record video - Start!");
		yield return new WaitForSeconds(seconds);

		// Deprecated
		// Copy audio file out
//		string path = Application.persistentDataPath;
//		string sourceName = "tempAudio.wav";
//		string targetName = "recordedAudio.wav";
//		// Use Path class to manipulate file and directory paths.
//		string sourceFile = System.IO.Path.Combine(path, sourceName);
//		string targetFile = System.IO.Path.Combine(path, sourceName);
//		CopyFile(sourceFile, targetFile);

		vr.EndRecordingSession(
			videoPosition,          // where to put the finished video 
			out framesRecorded);    // # of video frames recorded
	}

	// This delegate function is called when the recording session has completed successfully
	// and the video file has been written.
	public void HandleSessionComplete()
	{
		// Do UI stuff when video is complete.
		finishedRecording = true;
		Debug.Log("RecordVideo.cs HandleSessionComplete() : Video record completed!");
	}

	// This delegate function is called if an error occurs during the recording session.
	public void HandleSessionError(iVidCapPro.SessionStatusCode errorCode)
	{
		// Do stuff when an error occurred.
		Debug.LogError("RecordVideo.cs HandleSessionError() : Error when record video!");
	}

	public bool CopyFile(string sourcePath, string targetPath)
	{
		if (!File.Exists(sourcePath))
			return false;

		File.Copy(sourcePath, targetPath, true);
		return true;
	}		
}