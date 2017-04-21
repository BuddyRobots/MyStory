using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MyStory;
using MyUtils;
using OpenCVForUnity;
using System.Collections;


[RequireComponent(typeof(GetImage))]
public class TakingPhoto : MonoBehaviour 
{
	private Button confirmBtn;
	private Button backBtn;

	GetImage getImage;

	public GameObject camQuad;

	void Start () 
	{
		confirmBtn=transform.Find("Confirm").GetComponent<Button>();
		backBtn=transform.Find("Back").GetComponent<Button>();
		EventTriggerListener.Get(confirmBtn.gameObject).onClick=OnConfirmBtnClick;
		EventTriggerListener.Get(backBtn.gameObject).onClick=OnBackBtnClick;

		// Flip Quad vertically if on iPad
		#if !UNITY_EDITOR
		GameObject quad = GameObject.Find("Quad");
		quad.transform.localScale = new Vector3(quad.transform.localScale.x, quad.transform.localScale.y * -1.0f, quad.transform.localScale.z);
		#endif

		getImage=gameObject.GetComponent<GetImage>();
		getImage.Init();
	}

	void Update()
	{
		camQuad.GetComponent<MeshRenderer>().material.mainTexture=getImage.webCamTexture;
	}

	private void OnConfirmBtnClick(GameObject btn)
	{				
		StartCoroutine(OnConfirmBtnClickCoroutine());
	}

	private void OnBackBtnClick(GameObject btn)
	{
		SceneManager.LoadSceneAsync("2_DrawModelShow");
	}

	void OnDisable() {
		getImage.Dispose();
	}

	private IEnumerator OnConfirmBtnClickCoroutine()
	{
		Manager._instance.sourceMat=getImage.RGBMat;

		// Segmentation	
		//TODO For test.
		//#if !UNITY_EDITOR
		// Depracated.
		//Mat readMat = ReadPicture.ReadAsMat("Pictures/Mouses/1487573118");
		//Mouse mouse = new Mouse(readMat);



		////////////////////
		string serverUrl = "http://192.168.0.100:8000/";
		Texture2D sourceTexture = Resources.Load("Pictures/Mouses/1487573118") as Texture2D;

		SendToServer sendToServer = new SendToServer(serverUrl, sourceTexture);

		yield return StartCoroutine(sendToServer.UploadPNG());

		Texture2D resultTexture = sendToServer.resultTexture;

		Mouse mouse = new Mouse(sourceTexture, resultTexture);




		Texture2D displayTex = new Texture2D(mouse.testDisplayImage.cols(), mouse.testDisplayImage.rows());
		Utils.matToTexture2D(mouse.testDisplayImage, displayTex);

		GameObject.Find("Image").GetComponent<Renderer>().material.mainTexture = displayTex;
		///////////////////




		//Mouse mouse = new Mouse(Manager._instance.sourceMat);
		Manager._instance.mouse = mouse;
		//#endif

		SceneManager.LoadSceneAsync("4_ModelAnimationShow");
	}		
}