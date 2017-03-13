using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevel : MonoBehaviour
{
	private Button backBtn;

	public Button[] levelBtns;


	public LevelItemData data;

	void Start () 
	{
		backBtn=transform.Find("Back").GetComponent<Button>();
		EventTriggerListener.Get(backBtn.gameObject).onClick=OnBackBtnClick;


		for (int i = 0; i < levelBtns.Length; i++) 
		{
			EventTriggerListener.Get(levelBtns[i].gameObject).onClick=OnLevelBtnClick;
		}

		LevelManager.Instance.LoadLocalLevelProgressData();
		RefreshLevelUI();
	}


	private void OnBackBtnClick(GameObject btn)
	{

		SceneManager.LoadSceneAsync("1_ModelSelect");
	}



	private void OnLevelBtnClick(GameObject go)
	{
		//根据选关按钮的名字来记录关卡等级，下一个场景根据存储的等级判断需要显示什么样的图片和角色
//		switch (go.name) 
//		{
//		case "Level_1":
//			Manager.level=Level.One;
//			break;
//		case "Level_2":
//			Manager.level=Level.Two;
//			break;
//		case "Level_3":
//			Manager.level=Level.Three;
//			break;
//		case "Level_4":
//			Manager.level=Level.Four;
//			break;
//		case "Level_5":
//			Manager.level=Level.Five;
//			break;
//		case "Level_6":
//			Manager.level=Level.Six;
//			break;
//		case "Level_7":
//			Manager.level=Level.Seven;
//			break;
//		case "Level_8":
//			Manager.level=Level.Eight;
//			break;
//		case "Level_9":
//			Manager.level=Level.Nine;
//			break;
//		default:
//			break;
//		}


		int levelID=GetLevel(go.name);//得到关卡数字
		Debug.Log("levelID----"+levelID);
		data=LevelManager.Instance.GetSingleLevelItem(levelID);//根据关卡数字拿到关卡数据
		LevelManager.Instance.SetCurrentLevel(data);//保存当前关卡信息




//		Manager._instance.GetRecordTimeOfLevel(Manager.level);//选关完就可以确定当前关卡的录音时间是多少
		SceneManager.LoadSceneAsync("6_FormalScene_0");
	}

	/// <summary>
	/// 获取关卡
	/// </summary>
	/// <returns>返回关卡数字</returns>
	/// <param name="levelName">参数是“LevelD01”类型的字符串</param>
	/// 如果返回0则表示解析错误；
	public int GetLevel(string btnName)
	{

		int ret=0;
		string temp =btnName.Substring (6);
		int.TryParse(temp,out ret);

		return ret;


	}


	public void RefreshLevelUI()
	{




		Debug.Log("----RefreshLevelUI()");
		LevelItemData data=null;
		for (int i = 0; i < LevelManager.Instance.levelItemDataList.Count; i++) 
		{
			data =LevelManager.Instance.levelItemDataList[i];
			Button btn=levelBtns[i];
			int levelID=GetLevel(btn.gameObject.name);
//			Debug.Log("levelID-----"+levelID);
			if (data.LevelID ==levelID) 
			{
				switch (data.Progress)
				{

				case LevelProgress.Todo:
					btn.gameObject.SetActive(false);
					
					break;
				case LevelProgress.Done:
					btn.gameObject.SetActive(true);
					break;

				default:
					break;
				}

			}

		}
	}





}
