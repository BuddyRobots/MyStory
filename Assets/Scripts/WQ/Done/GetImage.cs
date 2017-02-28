using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCVForUnity;

[RequireComponent(typeof(WebCamTextureToMatHelper_Test))]
public class GetImage : MonoBehaviour 
{
	public static GetImage _instance;

	// Flag for taking several photos
//	public bool isTakingPhoto = false;
//	public List<Mat> frameImgList = new List<Mat>();

	public string deviceName;
	[HideInInspector]
	public  Texture2D texture;

	private WebCamTextureToMatHelper_Test webCamTextureToMatHelper_test;
	private WebCamTexture webcamTex;
	private Mat frameImg;
	private Color32[] colors;

	void Awake()
	{

		_instance=this;
	}

	void  Start () 
	{

		webCamTextureToMatHelper_test = gameObject.GetComponent<WebCamTextureToMatHelper_Test>();
		webCamTextureToMatHelper_test.Init();

	}



	void Update()
	{
		if (webCamTextureToMatHelper_test.isPlaying() && webCamTextureToMatHelper_test.didUpdateThisFrame())
		{
			frameImg = webCamTextureToMatHelper_test.GetMat();


			/////////////////////    这里是连续取多张图
			/*
			if (isTakingPhoto)
			{	

				frameImgList.Add(frameImg.clone());
				if (frameImgList.Count >= 1)
					isTakingPhoto = false; 

			}
			*/


			texture.Resize(frameImg.cols(), frameImg.rows());
			Utils.matToTexture2D(frameImg, texture, colors);
		}

	}



	/// <summary>
	/// Raises the web cam texture to mat helper inited event.
	/// </summary>
	public void OnWebCamTextureToMatHelperInited()
	{

		Mat webCamTextureMat = webCamTextureToMatHelper_test.GetMat();

		colors = new Color32[webCamTextureMat.cols() * webCamTextureMat.rows()];
		texture = new Texture2D(webCamTextureMat.cols(), webCamTextureMat.rows(), TextureFormat.RGBA32, false);


//		gameObject.transform.localScale = new Vector3(webCamTextureMat.cols(), webCamTextureMat.rows(), 1);


		gameObject.transform.localScale = new Vector3(15.6f,11.5f,10.9f);//


//		Debug.Log("Screen.width " + Screen.width + " Screen.height " + Screen.height + " Screen.orientation " + Screen.orientation);

		float width = 0;
		float height = 0;

		width = gameObject.transform.localScale.x;
		height = gameObject.transform.localScale.y;

		float widthScale =(float)Screen.width / width;
		float heightScale =(float)Screen.height / height;
		if (widthScale < heightScale) 
		{
			Camera.main.orthographicSize =(width *(float)Screen.height /(float)Screen.width) / 2;
		} 
		else 
		{
			Camera.main.orthographicSize = height / 2;
		}

		gameObject.GetComponent<Renderer>().material.mainTexture = texture;

	}


	public void OnWebCamTextureToMatHelperDisposed()
	{
		Debug.Log("OnWebCamTextureToMatHelperDisposed");

	}

}
