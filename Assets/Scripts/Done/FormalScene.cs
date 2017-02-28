using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FormalScene : MonoBehaviour 
{
	private Button backBtn;
	private Button nextBtn;
	private Button recordBtn;
	private Button shareBtn;
	private Button saveVideoToAlbumBtn;
	private Button cancelBtn;
	private Button startRecordBtn;

	private Button playBtn;
	private Button recordAgainBtn_RecordDoneFrame;
	private Button nextBtn_RecordDoneFrame;
	private Button shareBtn_RecordDoneFrame;
	private Button saveVideoToAlbumBtn_RecordDoneFrame;
	private Button confirmBtn;

	private GameObject music;
	private GameObject mask;
	private GameObject noticeToRecordFrame;
	private GameObject recordingFrame;
	private GameObject recordDoneFrame;
	private GameObject winFrame;
	private GameObject subtitle;
	private GameObject sceneParent;

	private Slider recordTimeSlider;

	private bool sliderMoving=false;

	private float sliderMovingTimer=0;

	private int currentLevelID;


	private AudioSource audioSource;


	public LevelItemData data;

	public GameObject sceneLevel_1;
	public GameObject sceneLevel_2;
	public bool subtitleShow=false;//字幕是否显示的标记




	void Start () 
	{
		Debug.Log("fomalScene --初始化");
		backBtn=transform.Find("Back").GetComponent<Button>();
		nextBtn=transform.Find("Next").GetComponent<Button>();
		recordBtn=transform.Find("Record").GetComponent<Button>();
		shareBtn=transform.Find("Share").GetComponent<Button>();
		saveVideoToAlbumBtn=transform.Find("SaveVideoToAlbum").GetComponent<Button>();
		cancelBtn=transform.Find("NoticeToRecordFrame/Cancel").GetComponent<Button>();
		startRecordBtn=transform.Find("NoticeToRecordFrame/StartRecord").GetComponent<Button>();


		playBtn=transform.Find("RecordDoneFrame/Play").GetComponent<Button>();
		recordAgainBtn_RecordDoneFrame=transform.Find("RecordDoneFrame/RecordAgain").GetComponent<Button>();
		nextBtn_RecordDoneFrame=transform.Find("RecordDoneFrame/Next").GetComponent<Button>();
		shareBtn_RecordDoneFrame=transform.Find("RecordDoneFrame/Share").GetComponent<Button>();
		saveVideoToAlbumBtn_RecordDoneFrame=transform.Find("RecordDoneFrame/SaveToAlbum").GetComponent<Button>();
		confirmBtn=transform.Find("WinFrame/Btn").GetComponent<Button>();

		music=transform.Find("Music").gameObject;
		mask=transform.Find("Mask").gameObject;
		noticeToRecordFrame=transform.Find("NoticeToRecordFrame").gameObject;
		recordingFrame=transform.Find("RecordingFrame").gameObject;
		recordDoneFrame=transform.Find("RecordDoneFrame").gameObject;
		winFrame=transform.Find("WinFrame").gameObject;
		subtitle=transform.Find("Subtitle").gameObject;

		recordTimeSlider=transform.Find("RecordingFrame/RecordTimeSlider").GetComponent<Slider>();



		audioSource=GameObject.Find("Manager").GetComponent<AudioSource>();


		//for test ..
//		audioSource.clip=Resources.Load<AudioClip>("Audio/Seagulls");

		sceneParent=GameObject.Find("SceneParent");

		sliderMoving=false;
		subtitleShow=false;

		EventTriggerListener.Get(backBtn.gameObject).onClick=OnBackBtnClick;
		EventTriggerListener.Get(nextBtn.gameObject).onClick=OnNextBtnClick;
		EventTriggerListener.Get(recordBtn.gameObject).onClick=OnRecordBtnClick;
		EventTriggerListener.Get(shareBtn.gameObject).onClick=OnShareBtnClick;
		EventTriggerListener.Get(saveVideoToAlbumBtn.gameObject).onClick=OnAlbumBtnClick;
		EventTriggerListener.Get(cancelBtn.gameObject).onClick=OnCancelBtnClick;
		EventTriggerListener.Get(startRecordBtn.gameObject).onClick=OnStartRecordBtnClick;

		EventTriggerListener.Get(playBtn.gameObject).onClick=OnPlayBtnClick;
		EventTriggerListener.Get(recordAgainBtn_RecordDoneFrame.gameObject).onClick=OnRecordAgainBtnClick;
		EventTriggerListener.Get(nextBtn_RecordDoneFrame.gameObject).onClick=OnNextBtnClick;
		EventTriggerListener.Get(shareBtn_RecordDoneFrame.gameObject).onClick=OnShareBtnClick;
		EventTriggerListener.Get(saveVideoToAlbumBtn_RecordDoneFrame.gameObject).onClick=OnSaveVideoToAlbumBtnClick;
		EventTriggerListener.Get(confirmBtn.gameObject).onClick=OnConfirmBtnClick;










		//需要根据当前关卡信息来显示对应的关卡的故事情景
		currentLevelID=LevelManager.currentLevelData.LevelID;
		ShowSceneAccordingToLevelID(currentLevelID);


//		Manager._instance.ShowFinger(new Vector3(1.3f,0.3f,0));
		BussinessManager._instance.ShowFinger(new Vector3(1.3f,0.3f,0));


	}




	void ShowSceneAccordingToLevelID(int levelID)
	{


		Debug.Log("----当前是第 "+levelID+" 关-----");


		//复制相应的预制体，并调用对应的初始化函数    

	
		///@ ToDO......
		GameObject tempScene=null;
		switch (levelID)
		{

		case 1:
			tempScene=Instantiate(sceneLevel_1) as GameObject;
			Debug.Log("克隆了第一关场景");
			break;
		
		case 2:
			tempScene=Instantiate<GameObject>(sceneLevel_2);
			Debug.Log("克隆了第二关场景");
			break;

		default:
			break;
		}
		if (tempScene!=null) 
		{
			tempScene.transform.parent=sceneParent.transform;
		}
	





	}



	void Update () 
	{
		if (Manager.recordingDone)
		{
			
			ShowRecordDone();
			Manager.recordingDone=false;
		}
		if (sliderMoving) 
		{
			
			sliderMovingTimer+=Time.deltaTime;
			recordTimeSlider.value=Mathf.Lerp(0,1, sliderMovingTimer/LevelManager.currentLevelData.RecordTime);

			if (sliderMovingTimer>=LevelManager.currentLevelData.RecordTime) 
			{
//				sliderMovingTimer=LevelManager.currentLevelData.RecordTime;
				sliderMoving=false;
				sliderMovingTimer=0;
			}
		}

	}

	private void OnBackBtnClick(GameObject btn)
	{
		//先获取当前是第几关，在当前关卡上减一关，再修改当前关卡为减去一关的关卡

		if (currentLevelID==1) 
		{
			SceneManager.LoadSceneAsync("5_SelectLevel");
		}
		else
		{
			currentLevelID--;
			if (currentLevelID<=1) 
			{
				currentLevelID=1;
			}
			data =LevelManager.Instance.GetSingleLevelItem(currentLevelID);
			LevelManager.Instance.SetCurrentLevel(data);//保存当前关卡信息
			SceneManager.LoadSceneAsync("6_FormalScene_0");
		}

		////////////////////////for test...
		Manager._instance.bgMusicFadeOut=true;
		///////////////////////////
	
	}

	private void OnNextBtnClick(GameObject btn)
	{
		//切换到下一个场景界面  
		if (currentLevelID==9) 
		{
			mask.SetActive(true);
			winFrame.SetActive(true);

		}
		else
		{
			currentLevelID++;
			if (currentLevelID>=9) 
			{
				currentLevelID=9;
			}
			data =LevelManager.Instance.GetSingleLevelItem(currentLevelID);
			LevelManager.Instance.SetCurrentLevel(data);//保存当前关卡信息
			SceneManager.LoadSceneAsync("6_FormalScene_0");
		}





		////////////////////// for test....
		Manager._instance.bgMusicFadeIn=true;
		/////////////////////


	}

	private void OnRecordBtnClick(GameObject btn)
	{
		//录音按钮隐藏，录音提示框显示，蒙板显示
		recordBtn.transform.gameObject.SetActive(false);
		mask.SetActive(true);
		noticeToRecordFrame.SetActive(true);

	}

	private void OnShareBtnClick(GameObject btn)
	{
		//分享到微信朋友圈  to do...


	}

	private void OnAlbumBtnClick(GameObject btn)
	{
		//保存视频到相册，提示保存成功  to do...


	}

	private void OnCancelBtnClick(GameObject btn)
	{
		mask.SetActive(false);
		noticeToRecordFrame.SetActive(false);
		recordBtn.transform.gameObject.SetActive(true);

	}



	private void OnStartRecordBtnClick(GameObject btn)
	{
		mask.SetActive(false);
		noticeToRecordFrame.SetActive(false);
		recordingFrame.SetActive(true);
		subtitle.SetActive(true);
		//录音，slider倒计时，音量大小显示，
		//同步字幕，录屏  to do ...

		MicroPhoneInputSaveWav.getInstance().StartRecord();//开始录音

		sliderMoving=true;
	
	}
		

	void  ShowRecordDone()
	{
//		yield return new WaitForSeconds(3f);

		mask.SetActive(true);
		music.SetActive(true);
		recordingFrame.SetActive(false);
		subtitle.SetActive(false);
		recordDoneFrame.SetActive(true);

	}


	private void OnRecordAgainBtnClick(GameObject btn)
	{
		Debug.Log("---------------record  Again--");
		mask.SetActive(false);
		recordDoneFrame.SetActive(false);
		recordingFrame.SetActive(true);
		subtitle.SetActive(true);
		music.SetActive(false);


		MicroPhoneInputSaveWav.getInstance().StartRecord();//开始录音
		sliderMoving=true;


	}
		

	void OnPlayBtnClick(GameObject btn)
	{
		recordDoneFrame.SetActive(false);
		mask.SetActive(false);
		music.SetActive(false);
		shareBtn.transform.gameObject.SetActive(true);
		saveVideoToAlbumBtn.transform.gameObject.SetActive(true);
		subtitle.SetActive(true);


		//保存音频，并播放
		MicroPhoneInputSaveWav.getInstance().SaveMusic();
		MicroPhoneInputSaveWav.getInstance().PlayRecord();


	}

	void OnSaveVideoToAlbumBtnClick(GameObject btn)
	{

		//保存视频到相册，并弹出保存成功提示框   to do...
	}


	void OnConfirmBtnClick(GameObject btn)
	{
		SceneManager.LoadSceneAsync("5_SelectLevel");

	}


}
