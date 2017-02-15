using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ModelAnimationShow : MonoBehaviour {

	private Button confirmBtn;
	private Button backBtn;
	private Button reDrawBtn;


	void Start () 
	{
		confirmBtn=transform.Find("Confirm").GetComponent<Button>();
		backBtn=transform.Find("Back").GetComponent<Button>();
		reDrawBtn=transform.Find("ReDraw").GetComponent<Button>();

		EventTriggerListener.Get(confirmBtn.gameObject).onClick=OnConfirmBtnClick;
		EventTriggerListener.Get(backBtn.gameObject).onClick=OnBackBtnClick;
		EventTriggerListener.Get(reDrawBtn.gameObject).onClick=OnReDrawBtnClick;

	}
	
	// Update is called once per frame
	void Update () {
		
	}



	private void OnConfirmBtnClick(GameObject btn)
	{
		SceneManager.LoadSceneAsync("5_SelectLevel");

	}

	private void OnBackBtnClick(GameObject btn)
	{

		SceneManager.LoadSceneAsync("3_TakingPhoto");

	}
	private void OnReDrawBtnClick(GameObject btn)
	{

		SceneManager.LoadSceneAsync("2_DrawModelShow");

	}
}
