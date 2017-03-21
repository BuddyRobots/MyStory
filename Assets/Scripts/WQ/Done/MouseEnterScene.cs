using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEnterScene : MonoBehaviour {

	float speed_run=2f;
	float speed_walk=1f;
	float speed_walkSlow=0.42f;
	float speed_stop;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetWalkSpeed()
	{

		LevelTwo_new._instance.mouseSpeed=speed_walk;


	}
	public void SetWalkSlowSpeed()
	{


		LevelTwo_new._instance.mouseSpeed=speed_walkSlow;


	}
	public void SetStopSpeed()
	{

		LevelTwo_new._instance.mouseSpeed=0;


	}



}
