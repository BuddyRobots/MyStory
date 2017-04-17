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

	public bool isRecording = false;
	public bool finishedRecording = false;
		
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
		finishedRecording = false;
		isRecording = true;
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
			Debug.Log("RecordVideo.cs RecordForSeconds() : Video recording Start!");
		
			yield return new WaitForSeconds(seconds);

			videoPosition = _videoPosition;
			status = vr.EndRecordingSession(videoPosition, out framesRecorded);

			if (status == iVidCapPro.SessionStatusCode.OK) {
				Debug.Log("RecordVideo.cs RecordForSeconds() : Video recording complete! Please wait while the video is finalized");
			} 
		} else {
			Debug.Log("RecordVideo.cs RecordForSeconds() : Failed to start recording video. SessionStatus : " + status.ToString());
		}
	}
		
	public void RegisterSessionCompleteDelegate(iVidCapPro.SessionCompleteDelegate del)
	{
		vr.RegisterSessionCompleteDelegate(del);
	}

	public void EndRecording()
	{
		if (!isRecording)
		{
			Debug.Log("RecordVideo.cs Endrecording() : Session is not started!");
			return;
		}

		videoPosition = iVidCapPro.VideoDisposition.Discard_Video;
		iVidCapPro.SessionStatusCode status =
			vr.EndRecordingSession(videoPosition, out framesRecorded);

		if (status == iVidCapPro.SessionStatusCode.OK) {
			Debug.Log("RecordVideo.cs RecordForSeconds() : Video discarded.");
		} 
	}

	private void HandleSessionComplete()
	{
		isRecording = false;
		finishedRecording = true;
		Debug.Log("RecordVideo.cs HandleSessionComplete() : Video recording session completed!");
	}

	private void HandleSessionError(iVidCapPro.SessionStatusCode errorCode)
	{
		isRecording = false;
		Debug.LogError("RecordVideo.cs HandleSessionError() : Error when recording video! ErrorCode : " + errorCode);
	}
}