using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OpenCVForUnity;


[RequireComponent(typeof(GetImage))]
public class HSVThreshold : MonoBehaviour {

	public GameObject sourceQuad;
	public GameObject binaryQuad;
	public GameObject croppedQuad;

	public Slider h_Min_Slider;
	public Slider h_Max_Slider;
	public Slider s_Min_Slider;
	public Slider s_Max_Slider;
	public Slider v_Min_Slider;
	public Slider v_Max_Slider;

	public Text h_Value;
	public Text s_Value;
	public Text v_Value;

	GetImage getImage;
	Texture2D binaryTexture;
	Texture2D croppedTexture;

	int THRES_H_MIN = 0;
	int THRES_H_MAX = 180;
	int THRES_S_MIN = 0;
	int THRES_S_MAX = 255;
	int THRES_V_MIN = 0;
	int THRES_V_MAX = 255;

	const int MODEL_WIDTH = 224;
	const int MODEL_HEIGHT = 224;

	void Start () {
		getImage = gameObject.GetComponent<GetImage>();
		getImage.Init();

		#if UNITY_IOS && !UNITY_EDITOR
		sourceQuad.transform.localScale = new Vector3(sourceQuad.transform.localScale.x, sourceQuad.transform.localScale.y * -1.0f, sourceQuad.transform.localScale.z);
		#endif

		if (PlayerPrefs.HasKey("THRES_H_MIN"))
		{
			THRES_H_MIN = PlayerPrefs.GetInt("THRES_H_MIN");
			THRES_H_MAX = PlayerPrefs.GetInt("THRES_H_MAX");
			THRES_S_MIN = PlayerPrefs.GetInt("THRES_S_MIN");
			THRES_S_MAX = PlayerPrefs.GetInt("THRES_S_MAX");
			THRES_V_MIN = PlayerPrefs.GetInt("THRES_V_MIN");
			THRES_V_MAX = PlayerPrefs.GetInt("THRES_V_MAX");
		}

		h_Min_Slider.value = THRES_H_MIN;
		h_Max_Slider.value = THRES_H_MAX;
		s_Min_Slider.value = THRES_S_MIN;
		s_Max_Slider.value = THRES_S_MAX;
		v_Min_Slider.value = THRES_V_MIN;
		v_Max_Slider.value = THRES_V_MAX;

	}
	
	// Update is called once per frame
	void Update () {
		if (getImage.isPlaying() && getImage.didUpdateThisFrame())
		{
			THRES_H_MIN = (int)h_Min_Slider.value;
			THRES_H_MAX = (int)h_Max_Slider.value;
			THRES_S_MIN = (int)s_Min_Slider.value;
			THRES_S_MAX = (int)s_Max_Slider.value;
			THRES_V_MIN = (int)v_Min_Slider.value;
			THRES_V_MAX = (int)v_Max_Slider.value;

			h_Value.text = "(" + THRES_H_MIN + ", " + THRES_H_MAX + ")";
			s_Value.text = "(" + THRES_S_MIN + ", " + THRES_S_MAX + ")";
			v_Value.text = "(" + THRES_V_MIN + ", " + THRES_V_MAX + ")";

			// Display Source Image
			sourceQuad.GetComponent<Renderer>().material.mainTexture = getImage.webCamTexture;

			// Display Result Image
			Mat sourceImage = getImage.RGBMat;
			Mat grayImage = MatBGR2Gray(sourceImage);
			if (binaryTexture == null || binaryTexture.width != grayImage.cols() || binaryTexture.height != grayImage.rows())
				binaryTexture = new Texture2D(grayImage.cols(), grayImage.rows());
			Utils.matToTexture2D(grayImage, binaryTexture);
			binaryQuad.GetComponent<Renderer>().material.mainTexture = binaryTexture;

			// Display Cropped Image
			Mat points = Mat.zeros(grayImage.size(), grayImage.type());
			Core.findNonZero(grayImage, points);
			OpenCVForUnity.Rect roi = Imgproc.boundingRect(new MatOfPoint(points));

			OpenCVForUnity.Rect bb = new OpenCVForUnity.Rect(
				new Point(Math.Max(roi.tl().x - 50.0, 0),
					Math.Max(roi.tl().y - 50.0, 0)),
				new Point(Math.Min(roi.br().x + 50.0, sourceImage.cols()),
					Math.Min(roi.br().y + 50.0, sourceImage.rows())));
			Mat croppedImage = new Mat(sourceImage, bb);

			Mat zoomedImage = ZoomCropped(croppedImage);

			if (croppedTexture == null || croppedTexture.width != zoomedImage.cols() || croppedTexture.height != zoomedImage.rows())
				croppedTexture = new Texture2D(zoomedImage.cols(), zoomedImage.rows());
			
			Utils.matToTexture2D(zoomedImage, croppedTexture);
			croppedQuad.GetComponent<Renderer>().material.mainTexture = croppedTexture;
		}
	}		

	private Mat MatBGR2Gray(Mat sourceImage)
	{
		// BGR to HSV
		Mat hsvImage = new Mat(sourceImage.rows(), sourceImage.cols(), CvType.CV_8UC3);
		Imgproc.cvtColor(sourceImage, hsvImage, Imgproc.COLOR_BGR2HSV);
		// InRange
		Mat grayImage = new Mat(sourceImage.rows(), sourceImage.cols(), CvType.CV_8UC1);
		Core.inRange(hsvImage,
			new Scalar(THRES_H_MIN, THRES_S_MIN, THRES_V_MIN),
			new Scalar(THRES_H_MAX, THRES_S_MAX, THRES_V_MAX),
			grayImage);
		Imgproc.morphologyEx(grayImage, grayImage, Imgproc.MORPH_OPEN,
			Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(7, 7)));
		Imgproc.morphologyEx(grayImage, grayImage, Imgproc.MORPH_CLOSE,
			Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(7, 7)));

		return grayImage;
	}

	private static Mat ZoomCropped(Mat croppedImage)
	{
		int croppedWidth = croppedImage.cols();
		int croppedHeight = croppedImage.rows();

		if (croppedWidth > croppedHeight)
		{
			int topMargin = (croppedWidth - croppedHeight)/2;
			int botMargin = topMargin;

			// Needed due to percision loss when /2
			if ((croppedHeight + topMargin*2) != croppedWidth)
				botMargin = croppedWidth - croppedHeight - topMargin;

			Core.copyMakeBorder(croppedImage, croppedImage, topMargin, botMargin, 0, 0, Core.BORDER_REPLICATE);
		}
		else if (croppedWidth < croppedHeight)
		{
			int lefMargin = (croppedHeight - croppedWidth)/2;
			int rigMargin = lefMargin;

			// Needed due to percision loss when /2
			if ((croppedWidth + lefMargin*2) != croppedHeight)
				rigMargin = croppedHeight - croppedWidth - lefMargin;

			Core.copyMakeBorder(croppedImage, croppedImage, 0, 0, lefMargin, rigMargin, Core.BORDER_REPLICATE);
		}

		Mat scaleImage = new Mat();
		Imgproc.resize(croppedImage, scaleImage, new Size(MODEL_HEIGHT, MODEL_WIDTH));

		// Return croppedImage[224*224*3]
		return scaleImage;
	}

	void OnApplicationQuit()
	{
		PlayerPrefs.SetInt("THRES_H_MIN", THRES_H_MIN);
		PlayerPrefs.SetInt("THRES_H_MAX", THRES_H_MAX);
		PlayerPrefs.SetInt("THRES_S_MIN", THRES_S_MIN);
		PlayerPrefs.SetInt("THRES_S_MAX", THRES_S_MAX);
		PlayerPrefs.SetInt("THRES_V_MIN", THRES_V_MIN);
		PlayerPrefs.SetInt("THRES_V_MAX", THRES_V_MAX);
	}
}