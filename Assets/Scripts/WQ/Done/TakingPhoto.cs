using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MyStory;


public class TakingPhoto : MonoBehaviour 
{
	private Button confirmBtn;
	private Button backBtn;

	void Start () 
	{
		confirmBtn=transform.Find("Confirm").GetComponent<Button>();
		backBtn=transform.Find("Back").GetComponent<Button>();
		EventTriggerListener.Get(confirmBtn.gameObject).onClick=OnConfirmBtnClick;
		EventTriggerListener.Get(backBtn.gameObject).onClick=OnBackBtnClick;
	}

	private void OnConfirmBtnClick(GameObject btn)
	{		
		//GetImage._instance.isTakingPhoto = true; //开启取图片

		//存储拍摄得到的texture2D
		Manager._instance.texture=GetImage._instance.texture;

		SceneManager.LoadSceneAsync("4_ModelAnimationShow");
	}

	private void OnBackBtnClick(GameObject btn)
	{
		SceneManager.LoadSceneAsync("2_DrawModelShow");
	}
}