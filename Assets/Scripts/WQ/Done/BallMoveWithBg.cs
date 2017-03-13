using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMoveWithBg : MonoBehaviour 
{

	public static  BallMoveWithBg instance;
	/// <summary>
	/// 球和背景一起移动时的速度
	/// </summary>
	public float speedWithBgMove=5f;
	Vector3 offset;
	private Rigidbody2D rig2D;
	bool move;


	void Start () 
	{
		offset=new Vector3(0.01f,0,0);
		rig2D =gameObject.GetComponent<Rigidbody2D>();
	}
	


	void Update ()
	{
		move=Manager._instance.move;
		if (move) 
		{
			transform.localPosition+=offset*speedWithBgMove;

		}
		
	}

//	public void Move()
//	{
//		Vector2 movement=new Vector2(Random.Range(1f,2f),Random.Range(2f,5f));
//		rig2D.velocity=movement*speed;
//
//	}
		

}
