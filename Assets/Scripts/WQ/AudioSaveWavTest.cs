using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSaveWavTest : MonoBehaviour {



	/*
	void Start()
	{
		Debug.Log("Application.persistentDataPath---"+Application.persistentDataPath);  ///Users/WangQian/Library/Application Support/DefaultCompany/LionAndMouse
		Debug.Log("Application.DataPath---"+Application.dataPath);  ///Users/WangQian/Documents/GitWorkspace/MyStory/Assets
		Debug.Log("Application.streamingAssetsPath---"+Application.streamingAssetsPath);  ///Users/WangQian/Documents/GitWorkspace/MyStory/Assets/StreamingAssets
		Debug.Log("Application.temporaryCachePath---"+Application.temporaryCachePath);  ///var/folders/vd/mrl8tn715dgffdg7p5pw8mq80000gn/T/DefaultCompany/LionAndMouse
	}
	*/

	void OnGUI()
	{
		if(GUILayout.Button("Record"))
		{
			MicroPhoneInputSaveWav.getInstance().StartRecord();
		}
		if(GUILayout.Button("Play"))
		{
			MicroPhoneInputSaveWav.getInstance().PlayRecord();
		}
		if(GUILayout.Button("SaveWav"))
		{
			MicroPhoneInputSaveWav.getInstance().SaveMusic();
		}
	}

}
