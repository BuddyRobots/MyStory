using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordVideoAndAudio : MonoBehaviour {

	public iVidCapPro vr;
	public Slider slider;

	RecordVideo recordVideo;


	void Awake()
	{
		recordVideo = new RecordVideo(vr, true);
	}

	void Start()
	{
		StartCoroutine(recordVideo.RecordForSeconds(10));
	}

	void Update()
	{
		float micLoudness = recordVideo.micLoudness;
		slider.value = micLoudness*100;
	}
}