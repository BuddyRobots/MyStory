using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DrawModelShow : MonoBehaviour {

	private Button takePhotoBtn;
	private Button backBtn;

	void Start () 
	{
		takePhotoBtn=transform.Find("TakePhotoBtn").GetComponent<Button>();
		backBtn=transform.Find("Back").GetComponent<Button>();
		EventTriggerListener.Get(takePhotoBtn.gameObject).onClick=OnTakePhotoBtnClick;
		EventTriggerListener.Get(backBtn.gameObject).onClick=OnBackBtnClick;


		ShowModelAccordingToModelChozen(Manager.modelType);
	}



	void ShowModelAccordingToModelChozen(ModelType ModelType)
	{
		switch (ModelType) 
		{
		case ModelType.Mouse:
			///@to do...显示3张老鼠图片与虚线框

			break;
		case ModelType.Ball:
			///@to do...显示3张皮球图片与虚线框

			break;
		case ModelType.Garland:
			///@to do...显示3张花环图片与虚线框

			break;
		default:
			break;
		}

	}

	void OnTakePhotoBtnClick(GameObject btn)
	{
		SceneManager.LoadSceneAsync("3_TakingPhoto");

	}

	private void OnBackBtnClick(GameObject btn)
	{

		SceneManager.LoadSceneAsync("1_ModelSelect");

	}
}
