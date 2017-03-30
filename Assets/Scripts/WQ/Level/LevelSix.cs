using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSix : MonoBehaviour 
{
	public static LevelSix _instance;

	bool pause;
	bool startStory;
	bool changeScene;


	void Awake()
	{

		_instance=this;
	}

	void Start () 
	{
		Manager.storyStatus=StoryStatus.Normal;
		Init();
	}


	void Init()
	{

		pause=false;
		startStory=false;
		Manager._instance.isSubtitleShowOver=false;
	}

	void Update () 
	{
		if (FormalScene._instance.storyBegin) 
		{

			if (!pause)
			{

				//播放旁白，字幕，网移动

				if (!startStory) 
				{
					startStory=true;

					if (Manager.storyStatus==StoryStatus.Normal) 
					{
						BussinessManager._instance.PlayAudioAside();
					}
					FormalScene._instance.ShowSubtitle();


					NetMove._instance.Move();
				}

				if (Manager._instance.isSubtitleShowOver && NetMove._instance.isOver && Manager.storyStatus!=StoryStatus.Recording) 
				{
					
						if (!changeScene) 
						{
							FormalScene._instance.ChangeSceneAutomatically();
							changeScene=true;

						}
					}

				}
			}
				
		}
		




	public void StartStoryToRecordAudioAndVideo()
	{
		if (BussinessManager._instance.finger!=null) 
		{
			Destroy(BussinessManager._instance.finger);

		}

		Init();
		NetMove._instance.Reset();
	}

	public void PlayStoryWithAudioRecording()
	{
		Init();
		NetMove._instance.Reset();

	}

	public void PauseStory()
	{

		NetMove._instance.StopMove();
		BussinessManager._instance.PauseAudioAside();
		SubtitleShow._instance.pause=true;

	}

	public void ResumeStory()
	{
		NetMove._instance.Move();
		BussinessManager._instance.ResumeAudioAside();
		SubtitleShow._instance.pause=false;
	}


}
