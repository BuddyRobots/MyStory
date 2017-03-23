using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ModelChoose : MonoBehaviour {
	 
	private Button mouseBtn;
	private Button ballBtn;
	private Button garlandBtn;
	private Button backBtn;
	private Button nextBtn;

	private GameObject manager;


	void Start () 
	{
		mouseBtn =transform.Find("Mouse").GetComponent<Button>();
		ballBtn =transform.Find("Ball").GetComponent<Button>();
		garlandBtn =transform.Find("Garland").GetComponent<Button>();
		backBtn =transform.Find("Back").GetComponent<Button>();
		nextBtn =transform.Find("Next").GetComponent<Button>();

		manager=GameObject.Find("Manager");

		EventTriggerListener.Get(mouseBtn.gameObject).onClick=OnModelChooseBtnClick;
		EventTriggerListener.Get(ballBtn.gameObject).onClick=OnModelChooseBtnClick;
		EventTriggerListener.Get(garlandBtn.gameObject).onClick=OnModelChooseBtnClick;
		EventTriggerListener.Get(backBtn.gameObject).onClick=OnBackBtnClick;
		EventTriggerListener.Get(nextBtn.gameObject).onClick=OnNextBtnClick;

		if (!Manager._instance.mouseGo) 
		{
			Manager._instance.mouseGo=Instantiate(Resources.Load("Prefab/Mouse")) as GameObject;
			Manager._instance.mouseGo.name="Mouse";
			Manager._instance.mouseGo.transform.position=Manager._instance.outsideScreenPos;
		}

		DontDestroyOnLoad(Manager._instance.mouseGo);
	}
	


	void OnModelChooseBtnClick(GameObject btn)
	{
		//根据btn的名字来决定下一个场景中需要显示哪一个模型的展示图片    
		/// to do ...

		switch (btn.name) 
		{
		case "Mouse":
			Manager.modelType=ModelType.Mouse;
			break;
		case "Ball":
			Manager.modelType=ModelType.Ball;

			break;
		case "Garland":
			Manager.modelType=ModelType.Garland;

			break;
		default:
			break;
		}


		SceneManager.LoadSceneAsync("2_DrawModelShow");
		GameObject.DontDestroyOnLoad(manager);
	}

	private void OnBackBtnClick(GameObject btn)
	{

		SceneManager.LoadSceneAsync("0_StartGame");
		GameObject.DontDestroyOnLoad(manager);

	}
	private void OnNextBtnClick(GameObject btn)
	{

		SceneManager.LoadSceneAsync("5_SelectLevel");
		GameObject.DontDestroyOnLoad(manager);

	}
}
