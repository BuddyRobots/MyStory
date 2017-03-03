using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour 
{

	private Button startBtn;

	void Start () 
	{
		startBtn=transform.Find("StartGameBtn").GetComponent<Button>();
		EventTriggerListener.Get(startBtn.gameObject).onClick=OnStartBtnClick;
	
		Debug.Log("******----"+Application.targetFrameRate);
	}
	

	void OnStartBtnClick (GameObject go) 
	{

		SceneManager.LoadSceneAsync("1_ModelSelect");

	}
}
