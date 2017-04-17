using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity;
using MyStory;
using MyUtils;


public class test_Tensorflow : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Texture2D inputTexture = MyUtils.ReadPicture.ReadAsTexture2D("Pictures/Mouses/1487573118");
		Mat inputMat = new Mat(inputTexture.height, inputTexture.width, CvType.CV_8UC3);
		Utils.texture2DToMat(inputTexture, inputMat);

		Mouse mouse = new Mouse(inputMat);

		Texture2D displayTex = mouse.head.texture;
		GameObject.Find("Head").GetComponent<Renderer>().material.mainTexture = displayTex;

		displayTex = mouse.leftEar.texture;
		GameObject.Find("Left Ear").GetComponent<Renderer>().material.mainTexture = displayTex;

		displayTex = mouse.rightEar.texture;
		GameObject.Find("Right Ear").GetComponent<Renderer>().material.mainTexture = displayTex;

		displayTex = mouse.body.texture;
		GameObject.Find("Body").GetComponent<Renderer>().material.mainTexture = displayTex;

		displayTex = mouse.leftArm.texture;
		GameObject.Find("Left Arm").GetComponent<Renderer>().material.mainTexture = displayTex;

		displayTex = mouse.rightArm.texture;
		GameObject.Find("Right Arm").GetComponent<Renderer>().material.mainTexture = displayTex;

		displayTex = mouse.leftLeg.texture;
		GameObject.Find("Left Leg").GetComponent<Renderer>().material.mainTexture = displayTex;

		displayTex = mouse.rightLeg.texture;
		GameObject.Find("Right Leg").GetComponent<Renderer>().material.mainTexture = displayTex;
	
		displayTex = mouse.tail.texture;
		GameObject.Find("Tail").GetComponent<Renderer>().material.mainTexture = displayTex;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}