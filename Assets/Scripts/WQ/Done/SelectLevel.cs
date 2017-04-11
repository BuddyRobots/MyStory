using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevel : MonoBehaviour
{
	public Button[] levelBtns;
	public LevelItemData data;

	private Button backBtn;

	void Start () 
	{
		Manager._instance.bgMusicFadeOut=false;

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
		int levelID=GetLevel(go.name);//得到关卡数字
		Debug.Log("currentPlayLevelID----"+levelID);
		data=LevelManager.Instance.GetSingleLevelItem(levelID);//根据关卡数字拿到关卡数据
		LevelManager.Instance.SetCurrentLevel(data);//保存当前关卡信息
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
		LevelItemData data=null;
		for (int i = 0; i < LevelManager.Instance.levelItemDataList.Count; i++) 
		{
			data =LevelManager.Instance.levelItemDataList[i];
			Button btn=levelBtns[i];
			int levelID=GetLevel(btn.gameObject.name);
			if (data.LevelID ==levelID) 
			{
//				Debug.Log("levelID-----"+levelID + "progress==" + data.Progress);
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
