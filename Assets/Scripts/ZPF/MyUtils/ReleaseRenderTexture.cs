using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ReleaseRenderTexture : MonoBehaviour {

	void OnDisable()
	{		
		if (GetComponent<Camera>().targetTexture)
			GetComponent<Camera>().targetTexture.Release();
	}
}
