using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class RecordingDoneFrameCtrl : MonoBehaviour {


	public Button nextLevelBtn;
	public Button recordAgainBtn;
	public Button shareBtn;
	public Button closeBtn;
	public Button playVideoBtn;

	public GameObject recordAgainFrame;




	void Start () 
	{
		EventTriggerListener.Get(nextLevelBtn.gameObject).onClick=OnNextLevelBtnClick;
		EventTriggerListener.Get(recordAgainBtn.gameObject).onClick=OnRecordAgainBtnClick;
		EventTriggerListener.Get(shareBtn.gameObject).onClick=OnShareBtnClick;
		EventTriggerListener.Get(closeBtn.gameObject).onClick=OnCloseBtnClick;
		EventTriggerListener.Get(playVideoBtn.gameObject).onClick=OnPlayVideoClick;



		gameObject.SetActive(false);

	}

	void OnNextLevelBtnClick(GameObject btn)
	{
		//当前界面隐藏  
		gameObject.SetActive(false);

		FormalScene._instance.EnterNextLevel();

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
		recordAgainFrame.SetActive(true);
		gameObject.SetActive(false);

	}

	void OnPlayVideoClick(GameObject btn)
	{
		//播放视频
		string moviePath = Path.Combine(Application.persistentDataPath, "/tempAudio.wav");

		if (File.Exists(moviePath))
			Debug.Log("movie exists.");
		else
			Debug.Log("movie does not exist.");

		Handheld.PlayFullScreenMovie("file://" + moviePath, Color.black, FullScreenMovieControlMode.CancelOnInput);

	}


}
