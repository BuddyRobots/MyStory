using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using com.mob;

public class DemoTest : MonoBehaviour {

	private bool _hasRecord;

	// Use this for initialization
	void Start () 
	{
		ShareREC.registerApp("1ae63a4932688");
		ShareREC.setSyncAudioComment(true);
	}



	void OnGUI ()
	{
		//GUI.skin = demoSkin;

		float scale = 1.0f;

		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			scale = Screen.width / 320;
		}

		float btnWidth = 200 * scale;
		float btnHeight = 45 * scale;
		float btnTop = 20 * scale;
		GUI.skin.button.fontSize = Convert.ToInt32(16 * scale);

		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), _hasRecord ? "Stop" : "Start"))
		{
			_hasRecord = !_hasRecord;

			if (_hasRecord)
			{
				ShareREC.startRecoring();
			}
			else
			{
				FinishedRecordEvent evt = new FinishedRecordEvent(recordFinishedHandler);
				ShareREC.stopRecording(evt);
			}
		}

		btnTop += btnHeight + 20 * scale;
		if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnTop, btnWidth, btnHeight), "Social"))
		{
			Hashtable userData = new Hashtable();
			userData["score"] = "10000";
			ShareREC.editLastingRecording("我在XX游戏中跑了XX米赶紧来吧", userData, null);
		}
	}

	void recordFinishedHandler(Exception ex)
	{
		ShareREC.playLastRecording();
	}
}
