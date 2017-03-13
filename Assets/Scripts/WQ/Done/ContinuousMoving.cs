using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousMoving : MonoBehaviour 
{
	
	public float speed;
	public Vector3 leftPos;//左边界点
	public Vector3 rightPos;//右边界点
	Vector3 offset;

	public bool move;

	void Start () 
	{
		offset=new Vector3(0.01f,0,0);
//		speed=5f;
//		move =true;
	}
	

	void Update () 
	{
		move=Manager._instance.move;



		//往左移动
//		if (move) 
//		{
//			transform.localPosition-=offset*speed;
//			if (transform.localPosition.x<=leftPos.x) 
//			{
//				transform.localPosition=rightPos;
//			}
//
//
//		}
		//往右移动
		if (move) 
		{
			transform.localPosition+=offset*speed;
			if (transform.localPosition.x>=rightPos.x) 
			{
				transform.localPosition=leftPos;
			}


		}

	}


}
