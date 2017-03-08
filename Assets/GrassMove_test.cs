using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassMove_test : MonoBehaviour {

	public float speed;
	public Vector3 leftPos;//左边界点
	public Vector3 rightPos;//右边界点
	Vector3 offset;

	public bool move;

	void Start () 
	{
		offset=new Vector3(0.01f,0,0);
		speed=1f;
		move =true;
	}


	void Update () 
	{
		if (move) 
		{
			transform.Translate(Vector3.left*Time.deltaTime*speed);

			if (transform.localPosition.x<=leftPos.x) 
			{
				transform.position=rightPos;
			}

		}

	}
}
