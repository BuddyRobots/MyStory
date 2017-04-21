using UnityEngine;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;


public class SendToServer {

	private string serverUrl;

	private Texture2D sourceTexture;
	public Texture2D resultTexture;


	public SendToServer(string url, Texture2D tex)
	{
		serverUrl = url;
		sourceTexture = tex;
	}		

	public IEnumerator UploadPNG()
	{
		// We should only read the screen after all rendering is complete
		yield return new WaitForEndOfFrame();

		byte[] sourceData = sourceTexture.EncodeToPNG();

		WWWForm form = new WWWForm();
		form.AddField("frameCount", Time.frameCount.ToString());
		form.AddBinaryData("file", sourceData, "mouse.png", "image/png");

		WWW w = new WWW(serverUrl, form);
		yield return w;
		if (!string.IsNullOrEmpty(w.error))
		{
			Debug.LogError(w.error);
		}
		else
		{
			resultTexture = w.texture;
			Debug.Log("Finished Uploading Screenshot and Recieved Server Response");
		}
	}		
}