using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEnterScene : MonoBehaviour 
{

	float speed_run=2f;
	float speed_walk=1f;
	float speed_walkSlow=0.42f;
	float speed_stop;



	public void SetWalkSpeed()
	{

		LevelTwo._instance.moveSpeed=speed_walk;


	}
	public void SetWalkSlowSpeed()
	{


		LevelTwo._instance.moveSpeed=speed_walkSlow;


	}
	public void SetStopSpeed()
	{

		LevelTwo._instance.moveSpeed=0;


	}


	public void StartToWalk()
	{
		LevelTwo._instance.startToWalk=true;

	}


}
