using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ModelAnimationShow : MonoBehaviour 
{

	private Button confirmBtn;
	private Button backBtn;
	private Button reDrawBtn;

	//for test...
//	public Sprite textureSprite;

	void Start () 
	{
		confirmBtn=transform.Find("Confirm").GetComponent<Button>();
		backBtn=transform.Find("Back").GetComponent<Button>();
		reDrawBtn=transform.Find("ReDraw").GetComponent<Button>();

		EventTriggerListener.Get(confirmBtn.gameObject).onClick=OnConfirmBtnClick;
		EventTriggerListener.Get(backBtn.gameObject).onClick=OnBackBtnClick;
		EventTriggerListener.Get(reDrawBtn.gameObject).onClick=OnReDrawBtnClick;
		//for test....
//		textureSprite=GameObject.Find("TextureSprite").GetComponent<Sprite>();
//		SetTex();

	}

	//for test....
//	void SetTex()
//	{
//		textureSprite =Sprite.Create(Manager._instance.texture,new Rect(0,0,300,400),new Vector2(0.5f,0.5f),100);
//
//	}

	private void OnConfirmBtnClick(GameObject btn)
	{
		SceneManager.LoadSceneAsync("1_ModelSelect");

	}

	private void OnBackBtnClick(GameObject btn)
	{

		SceneManager.LoadSceneAsync("3_TakingPhoto");

	}
	private void OnReDrawBtnClick(GameObject btn)
	{

		SceneManager.LoadSceneAsync("2_DrawModelShow");

	}
}
