using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassMoveCtrl : MonoBehaviour 
{
	public float speed;
	public Vector3 leftPos;//左边界点
	public Vector3 rightPos;//右边界点
	Vector3 offset;

	public bool move;

	void Start () 
	{
		offset=new Vector3(0.01f,0,0);
		speed=3f;
		move =true;
	}
	

	void Update () 
	{
		if (move) 
		{
			transform.localPosition-=offset*speed;
			if ((transform.localPosition-leftPos).x<=0.01f) 
			{
				transform.localPosition=rightPos;
			}
			
		}

	}


}
