using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevel : MonoBehaviour
{
	private Button backBtn;

	public Button[] levelBtns;




	void Start () 
	{
		backBtn=transform.Find("Back").GetComponent<Button>();
		EventTriggerListener.Get(backBtn.gameObject).onClick=OnBackBtnClick;


		for (int i = 0; i < levelBtns.Length; i++) 
		{
			EventTriggerListener.Get(levelBtns[i].gameObject).onClick=OnLevelBtnClick;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	private void OnBackBtnClick(GameObject btn)
	{

		SceneManager.LoadSceneAsync("1_ModelSelect");

	}



	private void OnLevelBtnClick(GameObject go)
	{
		//根据选关按钮的名字来记录关卡等级，下一个场景根据存储的等级判断需要显示什么样的图片和角色
		switch (go.name) 
		{
		case "Level_1":
			Manager.level=Level.One;
			break;
		case "Level_2":
			Manager.level=Level.Two;
			break;
		case "Level_3":
			Manager.level=Level.Three;
			break;
		case "Level_4":
			Manager.level=Level.Four;
			break;
		case "Level_5":
			Manager.level=Level.Five;
			break;
		case "Level_6":
			Manager.level=Level.Six;
			break;
		case "Level_7":
			Manager.level=Level.Seven;
			break;
		case "Level_8":
			Manager.level=Level.Eight;
			break;
		case "Level_9":
			Manager.level=Level.Nine;
			break;
		default:
			break;
		}

		SceneManager.LoadSceneAsync("6_FormalScene_0");
	}



}
