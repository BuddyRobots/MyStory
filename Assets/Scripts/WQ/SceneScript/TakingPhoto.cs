using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MyStory;
using MyUtils;
using OpenCVForUnity;


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
		Manager._instance.sourceMat=getImage.RGBMat;

		// Segmentation	
		//TODO For test.
		#if !UNITY_EDITOR
		Mat readMat = ReadPicture.ReadAsMat("Pictures/Mouses/1487573118");
		Mouse mouse = new Mouse(readMat);


		//Mouse mouse = new Mouse(Manager._instance.sourceMat);
		Manager._instance.mouse = mouse;
		#endif

		SceneManager.LoadSceneAsync("4_ModelAnimationShow");
	}

	private void OnBackBtnClick(GameObject btn)
	{
		SceneManager.LoadSceneAsync("2_DrawModelShow");
	}

	void OnDisable() {
		getImage.Dispose();
	}
}