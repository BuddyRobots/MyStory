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

		Mouse mouse = new Mouse(inputTexture);

		Texture2D headTex = mouse.tail.texture;
		Mat image = new Mat(headTex.height, headTex.width, CvType.CV_8UC3);
		Utils.texture2DToMat(headTex, image);

		Texture2D displayTexture = new Texture2D(image.width(), image.height());
		Utils.matToTexture2D(image, displayTexture);
		gameObject.GetComponent<Renderer>().material.mainTexture = displayTexture;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
