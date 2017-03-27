using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCtrl : MonoBehaviour 
{



	public void AniDoneLevelNine()
	{
		LevelNine._instance.aniDone=true;

	}

	#region level Seven
	public void StopWalkLevelSeven()
	{

		LevelSeven._instance.speed=0f;
	}

	public void WalkToLionAniOverLevelSeven()
	{
		LevelSeven._instance.walkToLionAniOver=true;
		Debug.Log("走路动画播完了");

	}

	public void AniDoneLevelSeven()
	{
		LevelSeven._instance.aniDone=true;
	}
	#endregion
}
