using System.Collections;
using UnityEngine;


public class VideoRecorder {

	float _micLoudness;
	public float micLoudness
	{
		get	{
			if (!vr)
			{
				Debug.LogError("RecordVideo.cs micLoudness : iVidCapPro not initialized!");
				return 0;
			}
			return vr.GetMicLoudness();
		}
	}

	public bool finishedRecording = true;
		
	iVidCapPro vr;
	iVidCapPro.VideoDisposition videoPosition;
	int framesRecorded;


	public VideoRecorder(iVidCapPro _iVidCapPro, bool _setDebug = false)
	{
		vr = _iVidCapPro;
		videoPosition = iVidCapPro.VideoDisposition.Save_Video_To_Documents;
		framesRecorded = 0;

		vr.SetDebug(_setDebug);
		vr.RegisterSessionCompleteDelegate(HandleSessionComplete);
		vr.RegisterSessionErrorDelegate(HandleSessionError);
	}		

	public IEnumerator RecordForSeconds(int seconds, 
		iVidCapPro.VideoDisposition _videoPosition = iVidCapPro.VideoDisposition.Save_Video_To_Documents,
		string name = "MyRecordedVideo", int vidWidth = 640, int vidHeight = 480)
	{		
		iVidCapPro.SessionStatusCode status;

		#if UNITY_EDITOR
		status = vr.BeginRecordingSession(
			name, vidWidth, vidHeight, 30,                                        
			iVidCapPro.CaptureAudio.No_Audio,          
			iVidCapPro.CaptureFramerateLock.Locked);
		
		#elif UNITY_IOS && !UNITY_EDITOR
		status = vr.BeginRecordingSession(
		    name, vidWidth, vidHeight, 30,                                        
			iVidCapPro.CaptureAudio.Audio_Plus_Mic,    
			iVidCapPro.CaptureFramerateLock.Locked);

		#endif

		if (status == iVidCapPro.SessionStatusCode.OK) {
			finishedRecording = false;
			Debug.Log("RecordVideo.cs RecordForSeconds() : Video recording Start!");
		} else {
			finishedRecording = true;
			Debug.Log("RecordVideo.cs RecordForSeconds() : Failed to start recording video.");
		}

		yield return new WaitForSeconds(seconds);

		videoPosition = _videoPosition;
		status = vr.EndRecordingSession(videoPosition, out framesRecorded);

		if (status == iVidCapPro.SessionStatusCode.OK) {
			Debug.Log("RecordVideo.cs RecordForSeconds() : Video recording complete! Please wait while the video is finalized");
		} 
	}
		
	public void HandleSessionComplete()
	{
		finishedRecording = true;
		Debug.Log("RecordVideo.cs HandleSessionComplete() : Video recording session completed!");
	}

	public void HandleSessionError(iVidCapPro.SessionStatusCode errorCode)
	{
		finishedRecording = true;
		Debug.LogError("RecordVideo.cs HandleSessionError() : Error when recording video! ErrorCode : " + errorCode);
	}
}