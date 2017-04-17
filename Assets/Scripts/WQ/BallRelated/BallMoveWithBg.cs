using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMoveWithBg : MonoBehaviour 
{

	public static  BallMoveWithBg instance;
	/// <summary>
	/// 球和背景一起移动时的速度
	/// </summary>
	public float moveSpeedTogetherWithBg=8f;
	Vector3 offset;
	bool move;


	void Start () 
	{
		offset=new Vector3(0.01f,0,0);
	}
	


	void Update ()
	{
		move=Manager._instance.move;
		if (move) 
		{
			transform.localPosition+=offset*moveSpeedTogetherWithBg;
		}
		
	}


}
