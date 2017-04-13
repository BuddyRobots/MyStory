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

	private List<int> indexList=new List<int>();//用来存储destMouseSprites每一个元素的指数（比如destMouseSprites有3张图，那么指数就是0，1，2）
	List<int> randomList=new List<int>();//随机生成的3个不重复的数字的集合
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
		ClearList();
		//确定可以选择的图片共有多少张，并把指数存储在数组中（因为要随机出来3个指数）
		int count=destMouseSprites.Length;
		for (int i = 0; i < count; i++) 
		{
			indexList.Add(i);
		}

		GenerateListForRandomIndex();
		//列表中的随机数其实就是要从图片列表中选择的元素的下标，把这3张图片复制给需要界面UI列表demoImages
		for (int i = 0; i < randomList.Count; i++)
		{
			demoImages[i].sprite=destMouseSprites[randomList[i]];
		}
		//给虚线框图片赋值
		dottedImage.sprite=mouseDotted;

	}


	void ShowBallImages()
	{

		ClearList();
		int count=destBallSprites.Length;
		for (int i = 0; i < count; i++) 
		{
			indexList.Add(i);
		}

		GenerateListForRandomIndex();
		//列表中的随机数其实就是要从图片列表中选择的元素的下标，把这3张图片复制给需要界面UI列表demoImages
		for (int i = 0; i < randomList.Count; i++)
		{
			demoImages[i].sprite=destBallSprites[randomList[i]];
		}
		//给虚线框图片赋值
		dottedImage.sprite=ballDotted;

	}
	void ShowGarlandImages()
	{

		ClearList();
		int count=destGarlandSprites.Length;

		for (int i = 0; i < count; i++) 
		{
			indexList.Add(i);
		}

		GenerateListForRandomIndex();

		//列表中的随机数其实就是要从图片列表中选择的元素的下标，把这3张图片复制给需要界面UI列表demoImages
		for (int i = 0; i < randomList.Count; i++)
		{
			demoImages[i].sprite=destGarlandSprites[randomList[i]];
		}

		SetImagesForShowUI(destGarlandSprites);

		//给虚线框图片赋值
		dottedImage.sprite=garlandDotted;

	}

	void SetImagesForShowUI(Sprite[] showSpriteArr )
	{

		for (int i = 0; i < randomList.Count; i++)
		{
			demoImages[i].sprite=showSpriteArr[randomList[i]];
		}
	}

		
	/// <summary>
	/// 随机生成三个不重复的数，并加入列表中
	/// </summary>
	void GenerateListForRandomIndex()
	{
		//首先随机生成3个不相同的随机数，并用列表存储
		for (int i=0; randomList.Count<3;) 
		{
			int index=Random.Range(0,indexList.Count);
			if (randomList.Count==0)
			{
				randomList.Add(index);
			}
			else
			{
				bool isNumContain=JudgeIfNumberIsInList(index,randomList);
				if(isNumContain)
				{


				}
				else
				{
					randomList.Add(index);
				}
			}
		}
			
	}


	bool JudgeIfNumberIsInList(int num, List<int> list)
	{
		for (int i = 0; i < list.Count; i++) 
		{
			if (num.Equals(list[i])) 
			{
				return true;
			}
		}
		return false;

	}



	void ClearList()
	{
		randomList.Clear();
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
