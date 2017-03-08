using UnityEngine;
using OpenCVForUnity;
using MyStory;


public class test_ShowMouse : MonoBehaviour {

	[HideInInspector]
	Mouse mouse;

	public GameObject modelMouse;

	public GameObject headQuad;
	public GameObject leftEarQuad;
	public GameObject rightEarQuad;
	public GameObject bodyQuad;
	public GameObject leftArmQuad;
	public GameObject rightArmQuad;
	public GameObject leftLegQuad;
	public GameObject rightLegQuad;
	public GameObject tailQuad;

	// Use this for initialization
	void Start () {
		#if !UNITY_EDITOR
		mouse = Manager._instance.mouse;

		// Model Size Mat
		Mat modelSizeMat = mouse.modelSizeMat;
		Texture2D modelSizeTex = new Texture2D(modelSizeMat.cols(), modelSizeMat.rows());
		Utils.matToTexture2D(modelSizeMat, modelSizeTex);
		modelMouse.GetComponent<Renderer>().material.mainTexture = modelSizeTex;

		// Mouse Body Parts
		headQuad.GetComponent<Renderer>().material.mainTexture = mouse.head.texture;
		leftEarQuad.GetComponent<Renderer>().material.mainTexture = mouse.leftEar.texture;
		rightEarQuad.GetComponent<Renderer>().material.mainTexture = mouse.rightEar.texture;
		bodyQuad.GetComponent<Renderer>().material.mainTexture = mouse.body.texture;
		leftArmQuad.GetComponent<Renderer>().material.mainTexture = mouse.leftArm.texture;
		rightArmQuad.GetComponent<Renderer>().material.mainTexture = mouse.rightArm.texture;
		leftLegQuad.GetComponent<Renderer>().material.mainTexture = mouse.leftLeg.texture;
		rightLegQuad.GetComponent<Renderer>().material.mainTexture = mouse.rightLeg.texture;
		tailQuad.GetComponent<Renderer>().material.mainTexture = mouse.tail.texture;
		#endif
	}
}
