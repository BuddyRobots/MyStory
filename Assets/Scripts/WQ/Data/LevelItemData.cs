﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LevelItemData 
{
	//这是保存关卡的纯信息类，包括每一关的ID,关卡名字，描述文字,和完成进度
	private int m_LevelID;
	private int m_LevelNumber;//关卡数字
	private string m_LevelName;//关卡名字
	private LevelProgress progress;
	private int preLevelID;
	private int nextLevelID;
	private int recordTime;//录音时长
	private List<string> subtitleList;//字幕（以句为单位）
	private AudioClip audioAside;//旁白音频

	public AudioClip AudioAside
	{

		get
		{ 
			return audioAside;
		}
		set
		{ 
			audioAside = value;
		}

	}

	public int LevelID
	{

		get
		{ 
			return m_LevelID;
		}
		set
		{ 
			m_LevelID = value;
		}

	}
	public int LevelNumber
	{
		get
		{ 
			return m_LevelNumber;
		}
		set
		{ 
			m_LevelNumber = value;
		}
	}

	public string LevelName
	{
		get
		{ 
			return m_LevelName;
		}
		set
		{
			m_LevelName = value;
		}
	}


	public  LevelProgress Progress
	{
		get
		{ 
			return progress;
		}

		set
		{ 
		
			progress = value;
		}

	}
	public int PrelevelID
	{
		get 
		{ 
			return preLevelID;
		}
		set
		{ 

			preLevelID = value;
		}
	}
	public int NextLevelID
	{
		get 
		{ 
			return nextLevelID;
		}
		set
		{ 

			nextLevelID = value;
		}
	}

	public int RecordTime
	{
		get 
		{ 
			return recordTime;
		}
		set
		{ 

			recordTime = value;
		}
	}


	public List<string> SubtitleList
	{
		get
		{ 
			return subtitleList;
		}
		set
		{ 
			subtitleList = value;
		}
	}


}
