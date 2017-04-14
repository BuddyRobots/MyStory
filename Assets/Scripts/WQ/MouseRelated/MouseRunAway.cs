using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRunAway : MonoBehaviour 
{

	public static MouseRunAway _instance;

	float speed;

	[HideInInspector]
	public bool move;

	void Awake()
	{

		_instance=this;
	}

	void Start () 
	{
		ResetSpeed();
	}

	void Update () 
	{
		if (move) 
		{
			transform.Translate(Vector3.left*speed*Time.deltaTime);
		}
		
	}


	public void  Run()
	{
		move=true;
		speed=1.5f;

	}

	public void BumpedToSlowSpeed()
	{
		speed=0f;

	}

	public void RunFaster()
	{
		speed=2.5f;

	}

	public void RunAwayEnd()
	{
		move=false;
		speed=1f;
		LevelFive._instance.aniDone=true;

	}


	public void ResetSpeed()
	{

		speed=1f;
	}
}
