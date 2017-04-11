using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordAgainFrameCtrl : MonoBehaviour {




	public Button saveBtn;
	public Button recordAgainBtn;
	public Button closeBtn;


	public GameObject recordDoneFrame;


	void Start () 
	{
		EventTriggerListener.Get(saveBtn.gameObject).onClick=OnSaveBtnClick;
		EventTriggerListener.Get(recordAgainBtn.gameObject).onClick=OnRecordAgainBtnClick;
		EventTriggerListener.Get(closeBtn.gameObject).onClick=OnCloseBtnClick;

		
	}
	
	void OnSaveBtnClick(GameObject btn)
	{
		FormalScene._instance.SaveVideo();
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
