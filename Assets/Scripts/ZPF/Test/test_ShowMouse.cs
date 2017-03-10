using UnityEngine;
using OpenCVForUnity;
using MyStory;


public class test_ShowMouse : MonoBehaviour {

	[HideInInspector]
	Mouse mouse;

	public GameObject modelMouse;

	// Use this for initialization
	void Start () {
		#if !UNITY_EDITOR
		mouse = Manager._instance.mouseGo;

		// Model Size Mat
		Mat modelSizeMat = mouse.modelSizeMat;
		Texture2D modelSizeTex = new Texture2D(modelSizeMat.cols(), modelSizeMat.rows());
		Utils.matToTexture2D(modelSizeMat, modelSizeTex);
		modelMouse.GetComponent<Renderer>().material.mainTexture = modelSizeTex;	
		#endif
	}
}
