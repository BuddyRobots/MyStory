using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicBtnCtrl : MonoBehaviour {

	private Button musicBtn;
	private Image musicImage;
	private bool isMusicOnImgShow=true;

	void Start () 
	{
		musicBtn=GetComponent<Button>();
		musicImage=GetComponent<Image>();
		EventTriggerListener.Get(musicBtn.gameObject).onClick=OnMusicBtnClick;
	}
	

	void Update () 
	{
		if (Manager.musicOn && isMusicOnImgShow==false)
		{
			isMusicOnImgShow=true;
			musicImage.sprite=Resources.Load<Sprite>("Pictures/Music/MusicOn");
		}
		else if (!Manager.musicOn && isMusicOnImgShow )
		{
			isMusicOnImgShow=false;
			musicImage.sprite=Resources.Load<Sprite>("Pictures/Music/MusicOff");
		
		}
	}

	void OnMusicBtnClick(GameObject go)
	{
		Manager.musicOn=!Manager.musicOn;
	}
}
