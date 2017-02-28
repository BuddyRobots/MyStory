using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayAudioInScene : MonoBehaviour 
{
	private Button backBtn;
	private Button nextBtn;
	private Button shareBtn;
	private Button saveVideoToAlbumBtn;

	void Start () 
	{
		backBtn=transform.Find("Back").GetComponent<Button>();
		nextBtn=transform.Find("Next").GetComponent<Button>();
		shareBtn=transform.Find("Share").GetComponent<Button>();
		saveVideoToAlbumBtn=transform.Find("SaveVideoToAlbum").GetComponent<Button>();

		EventTriggerListener.Get(backBtn.gameObject).onClick=OnBackBtnClick;
		EventTriggerListener.Get(nextBtn.gameObject).onClick=OnNextBtnClick;
		EventTriggerListener.Get(shareBtn.gameObject).onClick=OnShareBtnClick;
		EventTriggerListener.Get(saveVideoToAlbumBtn.gameObject).onClick=OnAlbumBtnClick;

	}

	void ShowSceneAccordingToLevel(Level level)
	{
		switch (level) 
		{
		case Level.One:
			break;



		default:
			break;
		}

	}

	private void OnBackBtnClick(GameObject btn)
	{
		//切换到上一个场景界面   to do...
	
		///for test..
		SceneManager.LoadSceneAsync("5_SelectLevel");

	}

	private void OnNextBtnClick(GameObject btn)
	{
		//切换到下一个场景界面   to do...

	}

	private void OnShareBtnClick(GameObject btn)
	{
		//分享到微信朋友圈  to do...

	
	}

	private void OnAlbumBtnClick(GameObject btn)
	{
		//保存视频到相册，提示保存成功  to do...
	

	}

}
