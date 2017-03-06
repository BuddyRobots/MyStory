using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//测试小球滚动

public class BallMove : MonoBehaviour
{
	public GameObject mouse;
	public float speed;

	private Rigidbody2D rig2D;
	private Rigidbody2D mouseRig2D;

	void Start () 
	{
		rig2D =gameObject.GetComponent<Rigidbody2D>();
		mouseRig2D=mouse.GetComponent<Rigidbody2D>();
	}


//	void FixedUpdate ()
//	{
//		if (Input.GetKeyDown(KeyCode.A)) 
//		{
//			Vector2 movement=new Vector2(Random.Range(1f,2f),Random.Range(2f,5f));
//			rig2D.velocity=movement*speed;
//		}
//		if (Input.GetKeyDown(KeyCode.S)) 
//		{
//			Vector2 movement=new Vector2(-Random.Range(1f,2f),Random.Range(2f,5f));
//			rig2D.velocity=movement*speed;
//		}
//
//	}


	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.name=="Mouse")
		{
			Vector2 movement=new Vector2(Random.Range(1f,2f),Random.Range(2f,5f));
			rig2D.velocity=movement*speed;
		}
	}



}
