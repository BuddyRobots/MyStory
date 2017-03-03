using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MyStory;

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

		getImage=gameObject.GetComponent<GetImage>();
		getImage.Init();



	}


	void Update()
	{
//		camQuad.GetComponent<Renderer>().material.mainTexture=getImage.webCamTexture;
		camQuad.GetComponent<MeshRenderer>().material.mainTexture=getImage.webCamTexture;


	}

	private void OnConfirmBtnClick(GameObject btn)
	{		
		Manager._instance.getImage=getImage;
		SceneManager.LoadSceneAsync("4_ModelAnimationShow");
	}

	private void OnBackBtnClick(GameObject btn)
	{
		SceneManager.LoadSceneAsync("2_DrawModelShow");
	}
}