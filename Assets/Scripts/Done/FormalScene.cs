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
	private Button recordAgainBtn;
	private Button continueRecordBtn;
	private Button pauseBtn;

	private Button playBtn;
	private Button recordAgainBtn_RecordDoneFrame;
	private Button nextBtn_RecordDoneFrame;
	private Button shareBtn_RecordDoneFrame;
	private Button saveVideoToAlbumBtn_RecordDoneFrame;

	private GameObject music;
	private GameObject mask;
	private GameObject noticeToRecordFrame;
	private GameObject recordingFrame;
	private GameObject recordDoneFrame;
	private GameObject recordPauseFrame;


	void Start () 
	{
		backBtn=transform.Find("Back").GetComponent<Button>();
		nextBtn=transform.Find("Next").GetComponent<Button>();
		recordBtn=transform.Find("Record").GetComponent<Button>();
		shareBtn=transform.Find("Share").GetComponent<Button>();
		saveVideoToAlbumBtn=transform.Find("SaveVideoToAlbum").GetComponent<Button>();
		cancelBtn=transform.Find("NoticeToRecordFrame/Cancel").GetComponent<Button>();
		startRecordBtn=transform.Find("NoticeToRecordFrame/StartRecord").GetComponent<Button>();
		pauseBtn=transform.Find("Pause").GetComponent<Button>();
		recordAgainBtn=transform.Find("RecordPauseFrame/RecordAgain").GetComponent<Button>();
		continueRecordBtn=transform.Find("RecordPauseFrame/ContinueRecord").GetComponent<Button>();

		playBtn=transform.Find("RecordDoneFrame/Play").GetComponent<Button>();
		recordAgainBtn_RecordDoneFrame=transform.Find("RecordDoneFrame/RecordAgain").GetComponent<Button>();
		nextBtn_RecordDoneFrame=transform.Find("RecordDoneFrame/Next").GetComponent<Button>();
		shareBtn_RecordDoneFrame=transform.Find("RecordDoneFrame/Share").GetComponent<Button>();
		saveVideoToAlbumBtn_RecordDoneFrame=transform.Find("RecordDoneFrame/SaveToAlbum").GetComponent<Button>();

		music=transform.Find("Music").gameObject;
		mask=transform.Find("Mask").gameObject;
		noticeToRecordFrame=transform.Find("NoticeToRecordFrame").gameObject;
		recordingFrame=transform.Find("RecordingFrame").gameObject;
		recordDoneFrame=transform.Find("RecordDoneFrame").gameObject;
		recordPauseFrame=transform.Find("RecordPauseFrame").gameObject;


		EventTriggerListener.Get(backBtn.gameObject).onClick=OnBackBtnClick;
		EventTriggerListener.Get(nextBtn.gameObject).onClick=OnNextBtnClick;
		EventTriggerListener.Get(recordBtn.gameObject).onClick=OnRecordBtnClick;
		EventTriggerListener.Get(shareBtn.gameObject).onClick=OnShareBtnClick;
		EventTriggerListener.Get(saveVideoToAlbumBtn.gameObject).onClick=OnAlbumBtnClick;
		EventTriggerListener.Get(cancelBtn.gameObject).onClick=OnCancelBtnClick;
		EventTriggerListener.Get(startRecordBtn.gameObject).onClick=OnStartRecordBtnClick;
		EventTriggerListener.Get(pauseBtn.gameObject).onClick=OnPauseBtnClick;
		EventTriggerListener.Get(recordAgainBtn.gameObject).onClick=OnRecordAgainBtnClick;
		EventTriggerListener.Get(continueRecordBtn.gameObject).onClick=OnContinueRecordBtnClick;

		EventTriggerListener.Get(playBtn.gameObject).onClick=OnPlayBtnClick;
		EventTriggerListener.Get(recordAgainBtn_RecordDoneFrame.gameObject).onClick=OnRecordAgainBtnClick;
		EventTriggerListener.Get(nextBtn_RecordDoneFrame.gameObject).onClick=OnNextBtnClick;
		EventTriggerListener.Get(shareBtn_RecordDoneFrame.gameObject).onClick=OnShareBtnClick;
		EventTriggerListener.Get(saveVideoToAlbumBtn_RecordDoneFrame.gameObject).onClick=OnSaveVideoToAlbumBtnClick;


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

		///for test..
		SceneManager.LoadSceneAsync("6_FormalScene_0");

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
		music.SetActive(false);
		pauseBtn.transform.gameObject.SetActive(true);
		mask.SetActive(false);
		noticeToRecordFrame.SetActive(false);
		recordingFrame.SetActive(true);
		//录音，slider倒计时，音量大小显示，同步字幕，录屏
		//to do ...

		//录音结束后自动跳出配音成功界面
		//for test...
//		StartCoroutine(ShowRecordDone());

	}




	IEnumerator ShowRecordDone()
	{
		yield return new WaitForSeconds(3f);

		mask.SetActive(true);
		music.SetActive(true);
		pauseBtn.transform.gameObject.SetActive(false);
		recordingFrame.SetActive(false);
		recordDoneFrame.SetActive(true);

	}

	private void OnPauseBtnClick(GameObject btn)
	{

		recordingFrame.SetActive(false);
		recordPauseFrame.SetActive(true);
		mask.SetActive(true);
		//暂停录音（也会暂停录屏，但是用录屏软件RecordREC暂停录屏是不能暂停录音的，关于这个要怎么处理？？？）
		//to do...


	}

	private void OnRecordAgainBtnClick(GameObject btn)
	{
		//
		recordPauseFrame.SetActive(false);
		mask.SetActive(false);
		recordDoneFrame.SetActive(false);
		recordingFrame.SetActive(true);
		music.SetActive(false);
		pauseBtn.transform.gameObject.SetActive(true);
	}

	private void OnContinueRecordBtnClick(GameObject btn)
	{

		recordPauseFrame.SetActive(false);
		mask.SetActive(false);
		recordingFrame.SetActive(true);
		//继续录音，继续录屏   to do  ...


	}

	void OnPlayBtnClick(GameObject btn)
	{
		
		recordDoneFrame.SetActive(false);
		mask.SetActive(false);
		music.SetActive(false);
		shareBtn.transform.gameObject.SetActive(true);
		saveVideoToAlbumBtn.transform.gameObject.SetActive(true);


	}

	void OnSaveVideoToAlbumBtnClick(GameObject btn)
	{

		//保存视频到相册，并弹出保存成功提示框   to do...
	}

}
