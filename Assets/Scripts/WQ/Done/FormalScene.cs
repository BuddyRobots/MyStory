using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



/// <summary>
/// 该类负责控制故事场景的显示，界面UI的显示，界面切换，以及按钮的点击
/// </summary>
public class FormalScene : MonoBehaviour 
{
	public static FormalScene _instance;

	private Button backBtn;
	[HideInInspector]
	public  Button nextBtn;
	[HideInInspector]
	public  Button recordBtn;
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
	private GameObject blackMask;
	private GameObject noticeToRecordFrame;
	private GameObject recordingFrame;
	private GameObject recordDoneFrame;
	private GameObject winFrame;
	private GameObject subtitle;
	private GameObject sceneParent;

	private Slider recordTimeSlider;

	private bool sliderMoving=false;
	bool screenLightenDone;//屏幕是否亮完了的标志，值为true以后才能出现小手开始游戏

	private float sliderMovingTimer=0;
	float blackMaskFadingTimer;

	private int currentLevelID;


	private AudioSource audioSource;


	public LevelItemData data;

	public GameObject sceneLevel_1;
	public GameObject sceneLevel_2;
	public GameObject sceneLevel_3;
	public GameObject sceneLevel_4;

	[HideInInspector]
	public  bool screenGrowingDarkAndLight=false;
	bool blackMaskShow=false;
	[HideInInspector]
	public bool storyBegin=false;//故事场景是否开始的标志---得等屏幕亮完以后才开始（比如出现小手等）

	///这个标志用来判断---是否需要给老鼠，背景，摄像机等添加诸如Follow_WQ这种需要根据对象是否存在来动态设置变量的脚本
	/// 比如背景移动，背景需要跟随摄像机，摄像机需要跟随老鼠，需要等预制体被复制成对象显示在界面上才能手动设置
	[HideInInspector]
	public 	bool addComponentToGo=false;

	Vector3 blackMask_screenOutsidePos=new Vector3(0,1000f,0);//黑色遮罩在屏幕外面的位置
	Vector3 blackMask_screenInsidePos=Vector3.zero;//黑色遮罩在屏幕中间的位置



	void Awake()
	{
		_instance=this;
	}

	void Start () 
	{
//		Debug.Log("fomalScene --初始化");
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
		blackMask=transform.Find("BlackMask").gameObject;

		blackMask.transform.localPosition=blackMask_screenInsidePos;


		noticeToRecordFrame=transform.Find("NoticeToRecordFrame").gameObject;
		recordingFrame=transform.Find("RecordingFrame").gameObject;
		recordDoneFrame=transform.Find("RecordDoneFrame").gameObject;
		winFrame=transform.Find("WinFrame").gameObject;
		subtitle=transform.Find("Subtitle").gameObject;
		subtitle.SetActive(false);
		recordTimeSlider=transform.Find("RecordingFrame/RecordTimeSlider").GetComponent<Slider>();

		audioSource=GameObject.Find("Manager").GetComponent<AudioSource>();


		//for test ..
//		audioSource.clip=Resources.Load<AudioClip>("Audio/Seagulls");

		sceneParent=GameObject.Find("SceneParent");

		sliderMoving=false;
		storyBegin=false;

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


		Manager._instance.levelOneOver=false;



		//需要根据当前关卡信息来显示对应的关卡的故事情景
		currentLevelID=LevelManager.currentLevelData.LevelID;
		ShowSceneAccordingToLevelID(currentLevelID);
		if (currentLevelID==1) {
			nextBtn.gameObject.SetActive(false);
		}
	
		//屏幕变亮
		StartCoroutine(ScreenLighten());

	

	}


	public void ScreenDarkenThenReloadFormalScene()
	{

		StartCoroutine( ScreenDarkenThenLoadSceneAsync());
	}

	IEnumerator ScreenLighten()
	{
		//首先把blackmask透明度渐变成0,再移到屏幕外面
		blackMask.GetComponent<Image>().CrossFadeAlpha(0,Constant.SCREEN_FADINGTIME,true);
		yield return new WaitForSeconds(Constant.SCREEN_FADINGTIME);
		blackMask.transform.localPosition=blackMask_screenOutsidePos;

		storyBegin=true;

	}

	IEnumerator ScreenDarken()
	{
		//首先把blackmask移到屏幕中间，透明度渐变成1
		blackMask.transform.localPosition=blackMask_screenInsidePos;
		blackMask.GetComponent<Image>().CrossFadeAlpha(1,Constant.SCREEN_FADINGTIME,true);
		yield return new WaitForSeconds(Constant.SCREEN_FADINGTIME);


	}

	IEnumerator ScreenDarkenThenLoadSceneAsync()
	{
		//首先把blackmask移到屏幕中间，透明度渐变成1
		blackMask.transform.localPosition=blackMask_screenInsidePos;
		blackMask.GetComponent<Image>().CrossFadeAlpha(1,Constant.SCREEN_FADINGTIME,true);
		yield return new WaitForSeconds(Constant.SCREEN_FADINGTIME);
		SceneManager.LoadSceneAsync("6_FormalScene_0");

		Manager._instance.ChangeSceneToSetBgAudioVolumeNormal();


	}


	IEnumerator ChangeSceneToLeveSelect()
	{

		blackMask.transform.localPosition=blackMask_screenInsidePos;
		blackMask.GetComponent<Image>().CrossFadeAlpha(1,Constant.SCREEN_FADINGTIME,true);
		yield return new WaitForSeconds(Constant.SCREEN_FADINGTIME);
		SceneManager.LoadSceneAsync("5_SelectLevel");
		Manager._instance.ChangeSceneToSetBgAudioVolumeNormal();

	}


	IEnumerator ScreenDarkenThenLighten()
	{
		//只有第一关里从第一部分进入到第二部分的时候需要先变暗再变亮
		blackMask.transform.localPosition=blackMask_screenInsidePos;
		blackMask.GetComponent<Image>().CrossFadeAlpha(1,Constant.SCREEN_FADINGTIME,true);
		yield return new WaitForSeconds(Constant.SCREEN_FADINGTIME);


		//告诉第一关该隐藏录音按钮了

		LevelOne._instance.recordBtnHide=true;

		blackMask.GetComponent<Image>().CrossFadeAlpha(0,Constant.SCREEN_FADINGTIME,true);
		yield return new WaitForSeconds(Constant.SCREEN_FADINGTIME);
		blackMask.transform.localPosition=blackMask_screenOutsidePos;


		//告诉第一关，第一部分结束了，进入到第二部分
		LevelOne._instance.secondSceneShow=true;
		LevelOne._instance.showFingerOnMouse=false;
		Debug.Log("第一部分结束，进入到第二部分");

	}





	void ShowSceneAccordingToLevelID(int levelID)
	{


//		Debug.Log("----当前是第 "+levelID+" 关-----");


		//复制相应的预制体，并调用对应的初始化函数    

	
		///@ ToDO......
		GameObject tempScene=null;
		switch (levelID)
		{

		case 1:
			tempScene=Instantiate(sceneLevel_1) as GameObject;
//			Debug.Log("克隆了第一关场景");
			break;
		
		case 2:
			tempScene=Instantiate<GameObject>(sceneLevel_2);
//			Debug.Log("克隆了第二关场景");
			break;
		case 3:
			tempScene=Instantiate<GameObject>(sceneLevel_3);
			//			Debug.Log("克隆了第二关场景");
			break;
		case 4:
			tempScene=Instantiate<GameObject>(sceneLevel_4);

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
				sliderMoving=false;
				sliderMovingTimer=0;
			}
		}

		#region  第一关第一小节结束后，屏幕变暗再变亮，进入到第二小节
		if (LevelManager.currentLevelData.LevelID==1) 
		{
			if (screenGrowingDarkAndLight) 
			{
//				if (!blackMaskShow) 
				{
//					blackMaskShow=true;
					StartCoroutine(ScreenDarkenThenLighten());

				}
				screenGrowingDarkAndLight=false;
			}
		}
		#endregion

	}

	private void OnBackBtnClick(GameObject btn)
	{
		//先获取当前是第几关，在当前关卡上减一关，再修改当前关卡为减去一关的关卡

		if (currentLevelID==1) //如果是第一关，点击该按钮应该切换到选关界面
		{
			StartCoroutine(ChangeSceneToLeveSelect());
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
			ScreenDarkenThenReloadFormalScene();
		}




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
			if (currentLevelID==1) 
			{

				Manager._instance.levelOneOver=true;
				MousePlayBall._instance.OrderMouseToKickBallOutSide();	
	
			}
			else
			{
				UpgradeLevel();
				StartCoroutine(ScreenDarkenThenLoadSceneAsync());
			}	
		}
	}


	public void ChangeSceneAutomatically()
	{
		//切换到下一个场景界面  
		if (currentLevelID==9) 
		{
			mask.SetActive(true);
			winFrame.SetActive(true);

		}
		else
		{
			if (currentLevelID==1) 
			{

				Manager._instance.levelOneOver=true;
				MousePlayBall._instance.OrderMouseToKickBallOutSide();	

			}
			else
			{
				UpgradeLevel();
				StartCoroutine(ScreenDarkenThenLoadSceneAsync());
			}	
		}

	}

	public void UpgradeLevel()
	{
		currentLevelID++;
		if (currentLevelID>=9) 
		{
			currentLevelID=9;


		}
		data =LevelManager.Instance.GetSingleLevelItem(currentLevelID);
		LevelManager.Instance.SetCurrentLevel(data);//保存当前关卡信息

		PlayerPrefs.SetInt ("LevelID", currentLevelID);
		PlayerPrefs.SetInt ("LevelProgress", 0);

	}


	private void OnRecordBtnClick(GameObject btn)
	{
		//录音按钮隐藏，录音提示框显示，蒙板显示,且故事场景暂停
		recordBtn.transform.gameObject.SetActive(false);
		mask.SetActive(true);
		noticeToRecordFrame.SetActive(true);

		BussinessManager._instance.PauseStory();
		Manager ._instance.fingerMove=false;
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

		BussinessManager._instance.ResumeStory();//故事场景恢复
		Manager ._instance.fingerMove=true;
	}



	private void OnStartRecordBtnClick(GameObject btn)
	{
//		Manager._instance.bgMusicFadeOut=true;
		Manager.storyStatus=StoryStatus.Recording;

		mask.SetActive(false);
		noticeToRecordFrame.SetActive(false);
		recordingFrame.SetActive(true);
//		subtitle.SetActive(true);
		//录音，slider倒计时，音量大小显示，
		//同步字幕，录屏  to do ...

		HideSubtitle();
		ShowSubtitle();

		MicroPhoneInputSaveWav.getInstance().StartRecord();//开始录音
//		VideoRecManager._instance.StartRec();

		sliderMoving=true;

		BussinessManager._instance.StartStoryToRecordAudioAndVideo();


		Manager ._instance.fingerMove=true;

		Manager._instance.RecordingToSetBgAudioVolumeZero();
	
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

		Manager.storyStatus=StoryStatus.Recording;
		Debug.Log("---------------record  Again--");
		mask.SetActive(false);
		recordDoneFrame.SetActive(false);
		recordingFrame.SetActive(true);


		//字幕不是一开始录音就显示的
		subtitle.SetActive(true);
		music.SetActive(false);


		MicroPhoneInputSaveWav.getInstance().StartRecord();//开始录音
		sliderMoving=true;

		BussinessManager._instance.StartStoryToRecordAudioAndVideo();

	}
		

	void OnPlayBtnClick(GameObject btn)
	{
		Manager.storyStatus =StoryStatus.PlayRecord;
		recordDoneFrame.SetActive(false);
		mask.SetActive(false);
		music.SetActive(false);
		shareBtn.transform.gameObject.SetActive(true);
		saveVideoToAlbumBtn.transform.gameObject.SetActive(true);


		//保存音频，并播放
		MicroPhoneInputSaveWav.getInstance().SaveMusic();
		MicroPhoneInputSaveWav.getInstance().PlayRecord();

//		
//		LevelOne._instance.StartStory();
		HideSubtitle();
		ShowSubtitle();

		BussinessManager._instance.PlayStoryWithAudioRecording();


	}

	void OnSaveVideoToAlbumBtnClick(GameObject btn)
	{

		//保存视频到相册，并弹出保存成功提示框   to do...

//		VideoRecManager._instance.SaveVideoToPhotoAlbum();

	}


	void OnConfirmBtnClick(GameObject btn)
	{
		SceneManager.LoadSceneAsync("5_SelectLevel");

	}

	public void ShowSubtitle()
	{
//		Debug.Log("Formalscene------ShowSubtitle()");
		subtitle.SetActive(true);
//		Debug.Log("-ShowSubtitle()-----done");
	}

	public void HideSubtitle()
	{
//		Debug.Log("Formalscene------HideSubtitle()");

		subtitle.SetActive(false);
//		Debug.Log("-HideSubtitle()-----done");

	}

	void OnDestroy()
	{
		Manager._instance.fingerMove=true;
	}
}
