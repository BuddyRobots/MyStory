using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.InteropServices;

public class RecordAgainFrameCtrl : MonoBehaviour 
{
	public Button saveBtn;
//	public Button recordAgainBtn;
//	public Button closeBtn;

	public GameObject recordDoneFrame;


	void Start () 
	{
		EventTriggerListener.Get(saveBtn.gameObject).onClick=OnSaveBtnClick;
//		EventTriggerListener.Get(recordAgainBtn.gameObject).onClick=OnRecordAgainBtnClick; //写在这里不起作用，放到了FormalScene中
//		EventTriggerListener.Get(closeBtn.gameObject).onClick=OnCloseBtnClick;//写在这里不起作用，放到了FormalScene中
		
	}
	
	void OnSaveBtnClick(GameObject btn)
	{
		SaveVideo();
	}

	public void SaveVideo()
	{
		FormalScene._instance.SaveSucceed();
		Manager._instance.IOSSaveVideoToPhotosAlbum();
		gameObject.SetActive(false);
		FormalScene._instance.RecordVideo();

	}
		
//	void OnRecordAgainBtnClick(GameObject btn)
//	{
//		gameObject.SetActive(false);
//		FormalScene._instance.RecordVideo();
//	}
//
//	void OnCloseBtnClick(GameObject btn)
//	{
//		recordDoneFrame.SetActive(true);
//		gameObject.SetActive(false);
//
//	}
}
