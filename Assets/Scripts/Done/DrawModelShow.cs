using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DrawModelShow : MonoBehaviour {

	private Button takePhotoBtn;
	private Button backBtn;

	public Sprite mouseDotted;
	public Sprite ballDotted;
	public Sprite garlandDotted;

	public  Sprite[] destMouseSprites;//事先准备好的老鼠的展示图（数目>=3）
	public  Sprite[] destBallSprites;//事先准备好的老鼠的展示图（数目>=3）
	public  Sprite[] destGarlandSprites;//事先准备好的老鼠的展示图（数目>=3）

	private List<Sprite> showSprites=new List<Sprite>();//3张需要展示的图片

	private List<int> indexList=new List<int>();//用来存储destMouseSprites每一个元素的指数（比如destMouseSprites有3张图，那么指数就是0，1，2）

	public Image[] demoImages;//界面用来显示展示图片的3个Image
	public Image dottedImage;//界面上的虚线框图片

	void Start () 
	{
		takePhotoBtn=transform.Find("TakePhotoBtn").GetComponent<Button>();
		backBtn=transform.Find("Back").GetComponent<Button>();
		EventTriggerListener.Get(takePhotoBtn.gameObject).onClick=OnTakePhotoBtnClick;
		EventTriggerListener.Get(backBtn.gameObject).onClick=OnBackBtnClick;


		ShowModelAccordingToModelChozen(Manager.modelType);
	}




	void ShowModelAccordingToModelChozen(ModelType ModelType)
	{
		switch (ModelType) 
		{
		case ModelType.Mouse:
			//显示3张老鼠图片与虚线框
			ShowMouseImages();
			break;
		case ModelType.Ball:
			//显示3张皮球图片与虚线框
			ShowBallImages();
			break;
		case ModelType.Garland:
			//显示3张花环图片与虚线框
			ShowGarlandImages();
			break;
		default:
			break;
		}

	}





	void ShowMouseImages()
	{
		Debug.Log("------ShowMouseImages()");
		ClearList();

		//确定可以选择的图片共有多少张，并把指数存储在数组中（因为要随机出来3个指数）
		int count=destMouseSprites.Length;
		for (int i = 0; i < count; i++) 
		{
			indexList.Add(i);
		}

		//随机选择3张图片保存到showSprites中
		for (int j = 3; j >0; j--) 
		{
			int index=Random.Range(0,indexList.Count);
			showSprites.Add(destMouseSprites[indexList[index]]);
			indexList.Remove(index);
		}	

		Debug.Log("需要展示的老鼠图片数量（正常为3）--"+showSprites.Count);


		//把随机出来的3张图赋值给界面UI
		for (int i = 0; i < showSprites.Count; i++) 
		{
			demoImages[i].sprite=showSprites[i];
		}
		//给虚线框图片赋值
		dottedImage.sprite=mouseDotted;

	}





	void ShowBallImages()
	{


		Debug.Log("------ShowBallImages()");


		ClearList();
		int count=destBallSprites.Length;
		for (int i = 0; i < count; i++) 
		{
			indexList.Add(i);
		}

		//随机选择3张图片保存到showSprites中
		for (int j = 3; j >0; j--) 
		{
			int index=Random.Range(0,indexList.Count);
			showSprites.Add(destBallSprites[indexList[index]]);
			indexList.Remove(index);
		}	



		Debug.Log("需要展示的皮球图片数量（正常为3）--"+showSprites.Count);



		//把随机出来的3张图赋值给界面UI
		for (int i = 0; i < showSprites.Count; i++) 
		{
			demoImages[i].sprite=showSprites[i];
		}
		//给虚线框图片赋值
		dottedImage.sprite=ballDotted;

	}

	void ShowGarlandImages()
	{


		Debug.Log("------ShowGarlandImages()");


		ClearList();
		int count=destGarlandSprites.Length;
		for (int i = 0; i < count; i++) 
		{
			indexList.Add(i);
		}

		//随机选择3张图片保存到showSprites中
		for (int j = 3; j >0; j--) 
		{
			int index=Random.Range(0,indexList.Count);
			showSprites.Add(destGarlandSprites[indexList[index]]);
			indexList.Remove(index);
		}	



		Debug.Log("需要展示的--花环--图片数量（正常为3）--"+showSprites.Count);



		//把随机出来的3张图赋值给界面UI
		for (int i = 0; i < showSprites.Count; i++) 
		{
			demoImages[i].sprite=showSprites[i];
		}
		//给虚线框图片赋值
		dottedImage.sprite=garlandDotted;

	}

	void ClearList()
	{
		showSprites.Clear();
		indexList.Clear();
	}

	void OnTakePhotoBtnClick(GameObject btn)
	{
		SceneManager.LoadSceneAsync("3_TakingPhoto");

	}

	private void OnBackBtnClick(GameObject btn)
	{

		SceneManager.LoadSceneAsync("1_ModelSelect");

	}
}
