using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;


/// <summary>
///管理类----从文件中读取信息，初始化关卡列表信息
///该脚本挂在camera上面，游戏一开始就运行（初始化数据，加载本地关卡数据，等等）
/// </summary>
public class LevelManager : AllSceneSinglton<LevelManager>
{	



	/// <summary>
	/// 存储关卡数据的集合
	/// </summary>
	public List<LevelItemData> levelItemDataList = new List<LevelItemData>();

	public static LevelItemData currentLevelData;//这个数据可以被failurePanel，photoTakingPanel,photoRecognizingPanel 拿去获取当前关卡名字

	private List<List<string>> allLevelSubTitleList=new List<List<string>>();

	//json字符串，保存关卡的信息（这里的信息字段名和levelItemData里的属性保持一致）
	string leveljsonstr = @"
            {
 				""levelData"":[
				{
           			""levelID"": 1,
           			""levelName"": ""第一关"",
           			""levelsubtitle"":  ""有一天，小老鼠正在踢皮球，小皮球真淘气，滚来滚去捉迷藏。。。"" ,
           			""progress"":1,
           			""preLevelID"": 0,
           			""nextLevelID"": 2,
					""recordTime"":8,
					""sentenceTimeString"":""""

				},
				{
           			""levelID"": 2,
           			""levelName"": ""第二关"",
           			""levelsubtitle"":  ""咦，小皮球藏进了草丛里，小老鼠快去捉住它。"" ,
           			""progress"":0,
           			""preLevelID"": 1,
           			""nextLevelID"": 3,
					""recordTime"":9
				},
				{
           			""levelID"": 3,
           			""levelName"": ""第三关"",
           			""levelsubtitle"":  ""”嗷呜~~！是谁在我头上跳来跳去？“”哎呀坏了，原来是只大狮子。“”嗯！原来是个小不点，好大的胆子，看我吃了你！“"" ,
           			""progress"":0,
           			""preLevelID"": 2,
           			""nextLevelID"": 4,
					""recordTime"":10
				},
				{
           			""levelID"": 4,
           			""levelName"": ""第四关"",
           			""levelsubtitle"":  ""  "" ,
           			""progress"":0,
           			""preLevelID"": 3,
           			""nextLevelID"": 5,
					""recordTime"":11
				},
				{
           			""levelID"": 5,
           			""levelName"": ""第五关"",
           			""levelsubtitle"":  ""”狮子先生别生气，今天您要是放我走，将来有一天我一定会报答您的！“小老鼠苦苦哀求着。”哈哈哈哈，报答我？就凭你？你这个小家伙还真有趣，今天我心情好，就放你走吧。哈哈哈哈！“狮子捂着肚皮笑得满地打滚。"" ,
           			""progress"":0,
           			""preLevelID"": 4,
           			""nextLevelID"": 6,
					""recordTime"":12
				},
				{
           			""levelID"": 6,
           			""levelName"": ""第六关"",
           			""levelsubtitle"":  ""过了一个星期，狮子在散步时不慎落入了猎人布下的陷阱。"" ,
           			""progress"":0,
           			""preLevelID"": 5,
           			""nextLevelID"": 7,
					""recordTime"":13
				},
				{
           			""levelID"": 7,
           			""levelName"": ""第七关"",
           			""levelsubtitle"":  ""“狮子先生，别怕，我来救你了！”呀，原来是小老鼠出现了！”别说大话了，我这么强壮就没办法，你一个小不点能做什么呢？“狮子绝望地说。"" ,
           			""progress"":0,
           			""preLevelID"": 6,
           			""nextLevelID"": 8,
					""recordTime"":14
				},
				{
           			""levelID"": 8,
           			""levelName"": ""第八关"",
           			""levelsubtitle"":  ""   "" ,
           			""progress"":0,
           			""preLevelID"": 7,
           			""nextLevelID"": 9,
					""recordTime"":15
				},
				{
           			""levelID"": 9,
           			""levelName"": ""第九关"",
           			""levelsubtitle"":  ""”小老鼠，你是我的救命恩人，真是太谢谢你啦！“狮子得救了，高兴地为小老鼠带上了一顶花冠。从那以后，他们成了形影不离的好朋友。"" ,
           			""progress"":0,
           			""preLevelID"": 8,
           			""nextLevelID"": 10,
					""recordTime"":16
				}
				
				

				]
            }";



	void Awake()
	{
//		code for test...
		PlayerPrefs.SetInt ("LevelID",3);
		PlayerPrefs.SetInt ("LevelProgress",0);

		ReadSubtitleText();
		ParseLevelItemInfo();
		SetSubtitleForLevel();
		LoadLocalLevelProgressData ();
	}



	public void ReadSubtitleText()
	{
		Debug.Log("--ReadSubtitleText()");
		string 	allSubtitle=Resources.Load("Txt/Subtitle").ToString();//读取字幕文本文件
		string[] singleLevelSubtitle=allSubtitle.Split('|');//每一关的字幕
//		Debug.Log("一共有 "+singleLevelSubtitle.Length+" 段话");
		for (int i = 0; i < singleLevelSubtitle.Length; i++) 
		{
			string[] sentences=singleLevelSubtitle[i].Split('*');//每一句的字幕
			List<string> temp=new List<string>();
//			Debug.Log("第 "+i+" 段话有 "+sentences.Length+" 句话，分别是：");
			for (int j = 0; j < sentences.Length; j++)
			{
//				Debug.Log("第 "+j+" 句是："+sentences[j]);
				temp.Add(sentences[j]);
			}
			allLevelSubTitleList.Add(temp);

			Debug.Log("----------------------------------");

		}
	}

	void SetSubtitleForLevel()
	{
		Debug.Log("--SetSubtitleForLevel()");
		for (int i = 0; i < levelItemDataList.Count; i++) 
		{
			levelItemDataList[i].SubtitleList=allLevelSubTitleList[i];
			Debug.Log("************levelItemDataList[i].SubtitleList.count---"+levelItemDataList[i].SubtitleList.Count);
		}

	}
		
	/// <summary>
	/// 解析json字符串，并把信息存到levelitemdata里面
	/// </summary>
	public void ParseLevelItemInfo()
	{
		Debug.Log("----ParseLevelItemInfo()");
		JsonData jd = JsonMapper.ToObject(leveljsonstr);   
		JsonData jdLevelItems = jd["levelData"]; 

		if (jdLevelItems.IsArray) 
		{

			for (int i = 0; i < jdLevelItems.Count; i++) 
			{
				LevelItemData levelItemData = new LevelItemData ();
				levelItemData.LevelID = (int)jdLevelItems [i] ["levelID"];
				levelItemData.LevelName = (string)jdLevelItems [i] ["levelName"];
				levelItemData.LevelSubtitle = (string)jdLevelItems [i] ["levelsubtitle"];




				levelItemData.Progress = (LevelProgress)((int)jdLevelItems [i] ["progress"]);
				levelItemData.PrelevelID = (int)jdLevelItems [i] ["preLevelID"];
				levelItemData.NextLevelID = (int)jdLevelItems [i] ["nextLevelID"];
				//				Debug.Log("*******000"+levelItemData.NextLevelID);
				levelItemData.RecordTime=(int)jdLevelItems[i]["recordTime"];
				//				Debug.Log("*********"+levelItemData.RecordTime);
				levelItemDataList.Add (levelItemData);

			}

		}

//		Manager._instance.ReadSubtitleText();
//		for (int i = 0; i < levelItemDataList.Count; i++) 
//		{
//			Debug.Log("levelItemDataList[i].SubtitleList.Count---"+levelItemDataList[i].SubtitleList.Count);
//		}

	}
	/// <summary>
	/// 加载本地已经完成的关卡
	/// </summary>
	public void LoadLocalLevelProgressData()
	{
		Debug.Log("----LoadLocalLevelProgressData");
		int levelID = 0;
		int levelPro = 0;
		if (PlayerPrefs.HasKey ("LevelID"))
		{
			//如果本地存储中有LevelID这个字段，表示玩家有闯关记录，则需要去拿到这个数据
			levelID = PlayerPrefs.GetInt ("LevelID");
//			Debug.Log ("levelID==" + levelID);
		}
		else 
		{
			//如果没有，就创建这样一个ID
			PlayerPrefs.SetInt ("LevelID", 0);
			levelID = PlayerPrefs.GetInt ("LevelID");

		}
		if (PlayerPrefs.HasKey ("LevelProgress")) 
		{

			levelPro = PlayerPrefs.GetInt ("LevelProgress");
//			Debug.Log ("levelPro==" + levelPro);
		}
		else
		{

			PlayerPrefs.SetInt ("LevelProgress", 0);
			levelPro = PlayerPrefs.GetInt ("LevelProgress");
		}

		//获取到已完成的关卡后需要更新list数据
		UpdateLevelItemDataList (levelID,levelPro);

	}

	/// <summary>
	/// 更新关卡数据信息
	/// </summary>
	/// <param name="levelID">关卡ID</param>
	/// <param name="levelPro">关卡进度</param>
	public void UpdateLevelItemDataList(int levelID,int levelPro)
	{

		if (levelID == 0) //表示一关都没有玩过，是第一次玩
		{
			//Debug.Log ("updateLevelItemDataListAA");
			levelItemDataList [0].Progress = LevelProgress.Done;
			for (int i = 1; i < levelItemDataList.Count; ++i) 
			{

				levelItemDataList [i].Progress = LevelProgress.Todo;

			}

		} 
		else 
		{	//有关卡记录，不是第一次玩
			//修改已完成的最高关卡之前的所有关卡进度---都是已完成
			for (int i = 0; i < levelID; ++i) 
			{

				levelItemDataList [i].Progress = LevelProgress.Done;

			}

			//设置当前要完成的关卡进度
			if (levelID < 9) 
			{
				levelItemDataList [levelID].Progress = LevelProgress.Todo;
			} 
			else 
			{
				levelItemDataList [levelID - 1].Progress = LevelProgress.Done;
			}

		}
	}



	/// <summary>
	/// 根据关卡数字获取关卡数据,LevelManager提供这样一个获得关卡数据的方法，方便在关卡界面点击按钮时调用该方法来获取关卡数据
	/// </summary>
	/// <returns>返回一条关卡数据</returns>
	/// <param name="levelID">参数是关卡数字，也就是关卡ID</param>
	public LevelItemData GetSingleLevelItem(int levelID)
	{
		LevelItemData itemData = null;
		foreach (LevelItemData itemTemp in levelItemDataList)
		{
			if(itemTemp.LevelID==levelID)
			{
				itemData = itemTemp;
				break;

			}

		}
		return itemData;
	}
	/// <summary>
	/// 保存当前关卡数据，方便其他界面调用
	/// </summary>
	/// <param name="data">Data.</param>
	public void SetCurrentLevel(LevelItemData data)
	{
		Debug.Log("----SetCurrentLevel");
		currentLevelData = data;
		Debug.Log("currentLevelData.RecordTime---"+currentLevelData.RecordTime);

	}

}
