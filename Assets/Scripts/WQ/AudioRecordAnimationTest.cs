using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioRecordAnimationTest : MonoBehaviour {


	private Image volumeImage;

	private Sprite volumeImage_0;
	private Sprite volumeImage_1;
	private Sprite volumeImage_2;
	private Sprite volumeImage_3;
	private Sprite volumeImage_4;



	private bool isRecording=false;


	void Start () 
	{
		volumeImage=transform.Find("volumeImage").GetComponent<Image>();
//		Debug.Log(volumeImage.sprite.name);


		volumeImage_0=Resources.Load<Sprite>("Sprites/VolumeGrey");
		volumeImage_1=Resources.Load<Sprite>("Sprites/Volume_1");
		volumeImage_2=Resources.Load<Sprite>("Sprites/Volume_2");
		volumeImage_3=Resources.Load<Sprite>("Sprites/Volume_3");
		volumeImage_4=Resources.Load<Sprite>("Sprites/Volume_4");


//		volumeImage.sprite=volumeImage_4;//Resources.Load<Sprite>("Sprites/Volume_1");
		EventTriggerListener.Get(transform.Find("Button").gameObject).onClick=Record;
		
	}
	

	void Update () 
	{
		if (isRecording) 
		{
			float volume=MicroPhoneInput.getInstance().GetSoundVolume();
			Debug.Log("volume----"+volume);
			if (volume <=0) {
				volumeImage.sprite=volumeImage_0;
			}
			else if(volume >0 && volume <=1)
			{
				volumeImage.sprite=volumeImage_1;
			}
			else if(volume >1 && volume <=2)
			{
				volumeImage.sprite=volumeImage_2;
			}
			else if(volume >2 && volume <=3)
			{
				volumeImage.sprite=volumeImage_3;
			}
			else if(volume >3 )
			{
				volumeImage.sprite=volumeImage_4;
			}
		}
		
	}
		

	public void Record(GameObject go)
	{

		MicroPhoneInput.getInstance().StartRecord();
		isRecording=true;
		Debug.Log("recordBtnClick");
	}




}
