using UnityEngine;
using System;
using System.Collections;
using OpenCVForUnity;


public class GetImage : MonoBehaviour
{
	WebCamTexture m_WebCamTexture;
	public WebCamTexture webCamTexture {
		get {
			return m_WebCamTexture;
		}
		private set {
			m_WebCamTexture = value;
		}
	}
		
	Mat m_RGBMat;
	public Mat RGBMat {
		get {
			if (m_RGBMat == null || m_RGBMat.rows() != webCamTexture.height || m_RGBMat.cols() != webCamTexture.width)
			{
				m_RGBMat = new Mat(webCamTexture.height, webCamTexture.width, CvType.CV_8UC3);			
				colors = new Color32[webCamTexture.width * webCamTexture.height];
			}
			Utils.webCamTextureToMat(webCamTexture, m_RGBMat, colors);
			return m_RGBMat;
		}
		private set {
			m_RGBMat = value;
		}
	}

	Mat m_RGBAMat;
	public Mat RGBAMat {
		get {
			if (m_RGBAMat == null || m_RGBAMat.rows() != webCamTexture.height ||	m_RGBAMat.cols() != webCamTexture.width)
			{
				m_RGBAMat = new Mat(webCamTexture.height, webCamTexture.width, CvType.CV_8UC4);			
				colors = new Color32[webCamTexture.width * webCamTexture.height];
			}
			Utils.webCamTextureToMat(webCamTexture, m_RGBAMat, colors);
			return m_RGBAMat;
		}
		private set {
			m_RGBAMat = value;
		}
	}		

	WebCamDevice webCamDevice;
	Color32[] colors;

	string m_RequestDeviceName = null;
	public string requestDeviceName {
		get {
			return m_RequestDeviceName;
		}
		private set {
			m_RequestDeviceName = value;
		}
	}

	int m_RequestWidth = 640;
	public int requestWidth {
		get {
			return m_RequestWidth;
		}
		private set {
			m_RequestWidth = value;
		}
	}

	int m_RequestHeight = 480;
	public int requestHeight {
		get {
			return m_RequestHeight;
		}
		private set {
			m_RequestHeight = value;
		}
	}

	bool m_RequestIsFrontFacing = false;
	public bool requestIsFrontFacing {
		get {
			return m_RequestIsFrontFacing;
		}
		private set {
			m_RequestIsFrontFacing = value;
		}
	}

	bool initDone = false;


	public void Init(bool requestIsFrontFacing = false, string deviceName = null, int requestWidth = 640, int requestHeight = 480)
	{
		this.requestIsFrontFacing = requestIsFrontFacing;
		this.requestDeviceName = deviceName;
		this.requestWidth = requestWidth;
		this.requestHeight = requestHeight;

		StartCoroutine(init());
	}

	private IEnumerator init()
	{
		if (initDone) Dispose();

		if (!String.IsNullOrEmpty(requestDeviceName))
		{
			webCamTexture = new WebCamTexture(requestDeviceName, requestWidth, requestHeight);		
		}
		else
		{
			// Checks all the cameras available on the device
			for(int cameraIndex = 0; cameraIndex < WebCamTexture.devices.Length; cameraIndex++)
				if (WebCamTexture.devices[cameraIndex].isFrontFacing == requestIsFrontFacing)
				{
					webCamDevice = WebCamTexture.devices[cameraIndex];
					webCamTexture = new WebCamTexture(webCamDevice.name, requestWidth, requestHeight);

					break;
				}			
		}

		if (webCamTexture == null)
		{
			if (WebCamTexture.devices.Length > 0)
			{
				webCamDevice = WebCamTexture.devices[0];
				webCamTexture = new WebCamTexture(webCamDevice.name, requestWidth, requestHeight);
			}
			else
			{
				webCamTexture = new WebCamTexture(requestWidth, requestHeight);
			}
		}

		// Starts the camera
		webCamTexture.Play();

		while(true)
		{
			// If you want to use webcamTexture.width and webcamTexture.height on iOS,
			// you have to wait until webcamTexture.didUpdateThisFrame == 1,
			// otherwise these two values will be equal to 16.(http://forum.unity3d.com/threads/webcamtexture-and-error-0x0502.123922/)
			#if UNITY_IOS && !UNITY_EDITOR &&(UNITY_4_6_3 || UNITY_4_6_4 || UNITY_5_0_0 || UNITY_5_0_1)
			if (webCamTexture.width > 16 && webCamTexture.height > 16) {
			#else
			if (webCamTexture.didUpdateThisFrame) {
			#if UNITY_IOS && !UNITY_EDITOR && UNITY_5_2
			    while(webCamTexture.width <= 16)
			    {
			        webCamTexture.GetPixels32();
			        yield return new WaitForEndOfFrame();
			    }
			#endif
			#endif										

				initDone = true;
				break;
			}
			else
			{
				yield return 0;
			}
		}
	}

	public bool isInited()
	{
		return initDone;
	}
		
	public void Play()
	{
		if (initDone)
			webCamTexture.Play();
	}

	public void Pause()
	{
		if (initDone)
			webCamTexture.Pause();
	}

	public void Stop()
	{
		if (initDone)
			webCamTexture.Stop();
	}

	public bool isPlaying()
	{
		if (!initDone)
			return false;
		return webCamTexture.isPlaying;
	}

	public bool didUpdateThisFrame ()
	{
		if (!initDone)
			return false;

		#if UNITY_IOS && !UNITY_EDITOR && (UNITY_4_6_3 || UNITY_4_6_4 || UNITY_5_0_0 || UNITY_5_0_1)
		if (webCamTexture.width > 16 && webCamTexture.height > 16) {
		    return true;
		} else {
		    return false;
		}
		#else
		return webCamTexture.didUpdateThisFrame;
		#endif
	}

	public void Dispose()
	{
		initDone = false;

		if (webCamTexture != null) {
			webCamTexture.Stop ();
			webCamTexture = null;
		}
		if (RGBMat != null) {
			RGBMat.Dispose ();
			RGBMat = null;
		}
		if (RGBAMat != null) {
			RGBAMat.Dispose ();
			RGBAMat = null;
		}
		colors = null;
	}
}