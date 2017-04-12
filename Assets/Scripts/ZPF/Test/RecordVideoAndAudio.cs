using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordVideoAndAudio : MonoBehaviour {

	public iVidCapPro vr;
	public Slider slider;

	VideoRecorder recordVideo;


	void Awake()
	{
		recordVideo = new VideoRecorder(vr, true);
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