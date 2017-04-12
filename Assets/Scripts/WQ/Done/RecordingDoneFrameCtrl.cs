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

		gameObject.SetActive(false);

	}



	private void TestFileCopy(string srcPath, string destPath)
	{
		///测试通过，可以成功拷贝
//		string srcPath="/Users/WangQian/Documents/T0/body.png";
//		string destPath="/Users/WangQian/Documents/T1/copy_body.png";
		System.IO.File.Copy(srcPath,destPath);

	}



	void OnPlayVideoClick(GameObject btn)
	{
		//播放视频
		string srcPath = Path.Combine(Application.persistentDataPath, "/MyRecordedVideo.mov");
		string destPath=Path.Combine(Application.streamingAssetsPath, "/MyRecordedVideo.mov");

		Debug.Log("srcPath------"+srcPath);
		Debug.Log("destPath------"+destPath);


//		TestFileCopy(srcPath, destPath);

		Debug.Log("--after TestFileCopy(srcPath, destPath)");

		if (File.Exists(srcPath))
			Debug.Log("-----movie exists in Application.streamingAssetsPath");
		else
			Debug.Log("------movie does not exist in Application.streamingAssetsPath");

		Handheld.PlayFullScreenMovie("file://" + srcPath, Color.black, FullScreenMovieControlMode.CancelOnInput);

	}


}
