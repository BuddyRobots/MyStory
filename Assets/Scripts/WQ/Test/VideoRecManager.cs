using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.mob;
using System;
using System.Runtime.InteropServices;

public class VideoRecManager : MonoBehaviour
{


//	[DllImport("__Internal")]
//	private static extern void _PauseShareREC();

//	[DllImport("__Internal")]
//	private static extern void _ResumeShareREC();

//	[DllImport("__Internal")]
//	private static extern void _IOSSaveImageToPhotosAlbum(string readAddr);

	[DllImport("__Internal")]
	private static extern void _IOSSaveVideoToPhotosAlbum(string readAddr);

	void Start () 
	{
		ShareREC.registerApp("1ae63a4932688");
		ShareREC.setSyncAudioComment(true);
	}
	

	public void StartRec()
	{
		
		ShareREC.startRecoring();
	}
		

	public void EndRec()
	{
		FinishedRecordEvent finishedEvent=new FinishedRecordEvent(recordFinishedHandler);
		ShareREC.stopRecording (finishedEvent);


	}
		
	void recordFinishedHandler(Exception ex)
	{
		if (ex==null) 
		{
			
			ShareREC.playLastRecording();

			Hashtable userData = new Hashtable();
			userData["score"] = "10000";
			ShareREC.editLastingRecording("这是我用shareREC录制的视频", userData, null);



			string path  =	ShareREC.lastRecordingPath();
			Debug.Log("----------path-is-------"+path);//----------path-is-------/private/var/mobile/Containers/Data/Application/A62B2128-C1B5-4A46-BEF8-1CF46651BB9F/tmp/9853DA2A3B219BFEC395B90667052DE4.mp4

		}

	}



//	public void TakeScreenShotAndSaveToAlbum()
//	{
//		string imageName="testPic";
//		Application.CaptureScreenshot(imageName);
//
//		string readAddr=Application.persistentDataPath+"/"+imageName;
//		Debug.Log("------readAddr-----"+readAddr);//------readAddr-----/var/mobile/Containers/Data/Application/A62B2128-C1B5-4A46-BEF8-1CF46651BB9F/Documents/testPic
//		SaveImageToPhotoAlbum(readAddr);
//	}



	public void SaveVideoToPhotoAlbum()
	{
		string videoName="video_test";
		_IOSSaveVideoToPhotosAlbum(ShareREC.lastRecordingPath());

	}
		
//	private void SaveImageToPhotoAlbum(string readAddr)
//	{
//		
//		_IOSSaveImageToPhotosAlbum(readAddr);
//
//	}



	/*
	public void TestFileCopy()
	{
		///测试通过，可以成功拷贝
		string srcPath="/Users/WangQian/Documents/T0/body.png";
		string destPath="/Users/WangQian/Documents/T1/copy_body.png";
		System.IO.File.Copy(srcPath,destPath);

	}
	*/

}
