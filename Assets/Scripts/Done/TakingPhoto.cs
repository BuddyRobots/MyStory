using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TakingPhoto : MonoBehaviour 
{
	private Button confirmBtn;
	private Button backBtn;

	void Start () {
		confirmBtn=transform.Find("Confirm").GetComponent<Button>();
		backBtn=transform.Find("Back").GetComponent<Button>();
		EventTriggerListener.Get(confirmBtn.gameObject).onClick=OnConfirmBtnClick;
		EventTriggerListener.Get(backBtn.gameObject).onClick=OnBackBtnClick;

	}
	

	void Update () {
		
	}

	private void OnConfirmBtnClick(GameObject btn)
	{
		SceneManager.LoadSceneAsync("4_ModelAnimationShow");

	}

	private void OnBackBtnClick(GameObject btn)
	{

		SceneManager.LoadSceneAsync("2_DrawModelShow");

	}
}
