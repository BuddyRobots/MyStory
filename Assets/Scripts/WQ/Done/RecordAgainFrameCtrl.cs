using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.InteropServices;

public class RecordAgainFrameCtrl : MonoBehaviour 
{
	[DllImport("__Internal")]
	private static extern void _IOSSaveVideoToPhotosAlbum(string videoPath);



	public Button saveBtn;
	public Button recordAgainBtn;
	public Button closeBtn;


	public GameObject recordDoneFrame;


	void Start () 
	{
		EventTriggerListener.Get(saveBtn.gameObject).onClick=OnSaveBtnClick;
		EventTriggerListener.Get(recordAgainBtn.gameObject).onClick=OnRecordAgainBtnClick;
//		EventTriggerListener.Get(closeBtn.gameObject).onClick=OnCloseBtnClick;

		
	}
	
	void OnSaveBtnClick(GameObject btn)
	{
		SaveVideo();
	}

	public void SaveVideo()
	{
		Debug.Log("点击了保存按钮");

		FormalScene._instance.saveVideoOver=true;
		FormalScene._instance.saveSuccessNotice.SetActive(true);
		FormalScene._instance.saveSuccessNotice.GetComponent<CanvasGroup>().alpha=1;

		string srcPath = Path.Combine(Application.persistentDataPath, "MyRecordedVideo.mov");
		_IOSSaveVideoToPhotosAlbum(srcPath);


	}

	void OnRecordAgainBtnClick(GameObject btn)
	{
		gameObject.SetActive(false);

		FormalScene._instance.RecordAgain();

	}

	void OnCloseBtnClick(GameObject btn)
	{
		recordDoneFrame.SetActive(true);
		gameObject.SetActive(false);

	}
}
