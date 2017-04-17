using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class RecordingDoneFrameCtrl : MonoBehaviour {


	public Button continueBtn;
	public Button recordAgainBtn;
	public Button shareBtn;
//	public Button closeBtn;
	public Button playVideoBtn;

	public GameObject recordAgainFrame;

	void Start () 
	{
		EventTriggerListener.Get(continueBtn.gameObject).onClick=OnContinueBtnClick;
		EventTriggerListener.Get(recordAgainBtn.gameObject).onClick=OnRecordAgainBtnClick;
		EventTriggerListener.Get(shareBtn.gameObject).onClick=OnShareBtnClick;
//		EventTriggerListener.Get(closeBtn.gameObject).onClick=OnCloseBtnClick;//写在这里不起作用，放到了FormalScene中  ？？
		EventTriggerListener.Get(playVideoBtn.gameObject).onClick=OnPlayVideoClick;

	}

	void OnContinueBtnClick(GameObject btn)
	{
		//当前界面隐藏  
		gameObject.SetActive(false);
		Manager._instance.IOSSaveVideoToPhotosAlbum();
		FormalScene._instance.ContinueGame();

	}


	void OnRecordAgainBtnClick(GameObject btn)
	{
		//当前界面隐藏  
		gameObject.SetActive(false);
		recordAgainFrame.SetActive(true);
	}


	void OnShareBtnClick(GameObject btn)
	{
		//当前界面隐藏   TODO 

	}

	void OnCloseBtnClick(GameObject btn)
	{
		gameObject.SetActive(false);
	}

	void OnPlayVideoClick(GameObject btn)
	{
		//播放视频
		string srcPath = Path.Combine(Application.persistentDataPath, "MyRecordedVideo.mov");

		if (File.Exists(srcPath))
		{
			Debug.Log("-----MyRecordedVideo.mov exists in Application.persistentDataPath");

		}
		else
		{
			Debug.Log("------MyRecordedVideo.mov does not exist in Application.persistentDataPath");
		}

		Handheld.PlayFullScreenMovie("file://" + srcPath, Color.black, FullScreenMovieControlMode.CancelOnInput);

	}


}
