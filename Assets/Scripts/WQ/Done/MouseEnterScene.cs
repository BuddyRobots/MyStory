using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEnterScene : MonoBehaviour {

	float speed_run=2f;
	float speed_walk=1f;
	float speed_walkSlow=0.42f;
	float speed_stop;



	public void SetWalkSpeed()
	{

		LevelTwo_new._instance.moveSpeed=speed_walk;


	}
	public void SetWalkSlowSpeed()
	{


		LevelTwo_new._instance.moveSpeed=speed_walkSlow;


	}
	public void SetStopSpeed()
	{

		LevelTwo_new._instance.moveSpeed=0;


	}


	public void StartToWalk()
	{
		LevelTwo_new._instance.startToWalk=true;

	}


}
