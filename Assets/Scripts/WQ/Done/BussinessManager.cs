using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//包括小手的显示
public class BussinessManager : MonoBehaviour 
{

	public static BussinessManager _instance;
	[HideInInspector]
	public GameObject finger;
	private GameObject fingerPrefab;
	private Vector3 offSet = new Vector3 (1.3f, -0.7f, 0);//这个值表示小手距离需要点击的对象的距离（对象Position+offset=小手Position）
	[HideInInspector]
	public Vector3 prePos= Vector3.zero; // 记录当前指向的位置，如果没发生变化，不做任何操作


	private GameObject sceneParent;

	void Awake()
	{
		_instance=this;

		fingerPrefab=Resources.Load("Prefab/Finger",typeof(GameObject)) as GameObject;
		sceneParent=GameObject.Find("SceneParent");
	}

	void Start () 
	{

	}
	

	public void ShowFinger(Vector3 pos)
	{
//		Debug.Log("----出现小手");
		if (finger) 
		{
			Destroy (finger);
			finger = null;
		}
		finger = Instantiate (fingerPrefab) as GameObject;
		finger.name="finger";
		finger.transform.parent = sceneParent.transform;
		finger.transform.localScale = Vector3.one;
		finger.GetComponent<FingerCtrl> ().FingerShow (pos + offSet);
	}

	/// <summary>
	/// 
	/// </summary>
	public void PauseStory()
	{

		switch (LevelManager.currentLevelData.LevelID)
		{
		case 1:
			LevelOne._instance.PauseStory();
			break;
		case 2:
			break;
		case 3:
			break;
		case 4:
			break;
		case 5:
			break;
		case 6:
			break;
		case 7:
			break;
		case 8:
			break;
		case 9:
			break;
		default:
			break;
		}

	}

	/// <summary>
	/// 点击确定录音按钮，开始场景故事（没有旁白，有字幕，或者还有动画，同时开启录音和录屏）
	/// </summary>
	public void StartStoryToRecordAudioAndVideo()
	{
		switch (LevelManager.currentLevelData.LevelID)
		{
		case 1:
			LevelOne._instance.StartStoryToRecordAudioAndVideo();

			break;
		case 2:
			break;
		case 3:
			break;
		case 4:
			break;
		case 5:
			break;
		case 6:
			break;
		case 7:
			break;
		case 8:
			break;
		case 9:
			break;
		default:
			break;
		}

	}

	public void ResumeStory()
	{
		switch (LevelManager.currentLevelData.LevelID)
		{
		case 1:
			
			LevelOne._instance.ResumeStory();
			break;
		case 2:
			break;
		case 3:
			break;
		case 4:
			break;
		case 5:
			break;
		case 6:
			break;
		case 7:
			break;
		case 8:
			break;
		case 9:
			break;
		default:
			break;
		}

	}

	/// <summary>
	/// 点击播放按钮，开启场景故事（有播放录音，有字幕，或者还有动画）
	/// </summary>
	public void PlayStoryWithAudioRecording()
	{


	}

}
