using UnityEngine;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;


public class SendToServer : MonoBehaviour {

	//public string serverURL= "http://192.168.0.100:8000/form";
	public GameObject cube;

	private Texture2D tex;


	void Awake()
	{
		tex = Resources.Load("image2") as Texture2D;
		//cube.GetComponent<Renderer>().material.mainTexture = tex;
	}

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(UploadPNG());
	}

	public IEnumerator UploadPNG(string serverURL)
	{
		// We should only read the screen after all rendering is complete
		yield return new WaitForEndOfFrame();

		//byte[] texData = tex.EncodeToPNG();
		byte[] texData = new byte[5];

		WWWForm form = new WWWForm();
		form.AddField("frameCount", Time.frameCount.ToString());
		form.AddBinaryData("file", texData, "screenShot.png", "image/png");

		WWW w = new WWW(serverURL, form);
		yield return w;
		if (!string.IsNullOrEmpty(w.error))
		{
			print(w.error);
		}
		else
		{
			if (w.responseHeaders.Count > 0)
			{
				foreach (KeyValuePair<string, string> entry in w.responseHeaders)
				{
					Debug.Log(entry.Key + "   =    " + entry.Value);
				}
			}

			cube.GetComponent<Renderer>().material.mainTexture = w.texture;

			print("Finished Uploading Screenshot");
		}
	}		
}