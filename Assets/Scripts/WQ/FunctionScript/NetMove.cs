using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetMove : MonoBehaviour 
{

	public static NetMove _instance;


	public Transform netOriginPos;
	public Transform netDestPos;

	[HideInInspector]
	public bool move;
	[HideInInspector]
	public bool isOver;
	float moveSpeed;

	void Awake()
	{
		_instance=this;
	}


	void Start () 
	{
		moveSpeed=5f;
		isOver=false;	
	}

	

	void Update () 
	{
		if (move) 
		{
			MoveTo(netDestPos.position);

		}
		
	}


	private void MoveTo(Vector3 tar)
	{

		if(!isOver)
		{
			Vector3 offSet = tar - transform.position;
			transform.position += offSet.normalized * moveSpeed * Time.deltaTime;
			if(Vector3.Distance(tar, transform.position)<=0.1f)
			{
				isOver = true;
				transform.position = tar;
			}
		}
			
	}

	public  void Reset()
	{
		isOver=false;
		transform.position=netOriginPos.position;

	}

	public void Move()
	{
		move=true;
	}


	public void StopMove()
	{
		
		move=false;
		
	}




}
