using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

	private Button startBtn;
	private GameObject manager;

	void Start () 
	{
		startBtn=transform.Find("StartGameBtn").GetComponent<Button>();
		manager=GameObject.Find("Manager");
		EventTriggerListener.Get(startBtn.gameObject).onClick=OnStartBtnClick;
	}
	

	void OnStartBtnClick (GameObject go) 
	{
//		Debug.Log("click start btn");
//		GameObject.DontDestroyOnLoad(manager);

		SceneManager.LoadSceneAsync("1_ModelSelect");
	}
}
