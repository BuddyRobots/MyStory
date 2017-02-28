using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicBtnCtrl : MonoBehaviour {

	private Button musicBtn;
	private Image musicImage;
	private bool isMusicOnImgShow=true;
//	private bool isMusicOffImgShow=false;

	void Start () 
	{
		musicBtn=GetComponent<Button>();
		musicImage=GetComponent<Image>();
//		Debug.Log("----"+musicImage.mainTexture.name);
		EventTriggerListener.Get(musicBtn.gameObject).onClick=OnMusicBtnClick;
	}
	

	void Update () 
	{
		if (Manager.musicOn && isMusicOnImgShow==false)
		{
			isMusicOnImgShow=true;
			musicImage.sprite=Resources.Load<Sprite>("Sprites/MusicOn");

			Debug.Log("music is on");
		}
		else if (!Manager.musicOn && isMusicOnImgShow )
		{
			isMusicOnImgShow=false;
			musicImage.sprite=Resources.Load<Sprite>("Sprites/MusicOff");
			Debug.Log("music is off");

		}
	}

	void OnMusicBtnClick(GameObject go)
	{
		Manager.musicOn=!Manager.musicOn;
//		Debug.Log("--musicOn--"+Manager.musicOn);

	}
}
