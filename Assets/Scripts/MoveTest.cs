﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour 
{
	private float speed=2f;
	private Rigidbody2D mouseRig2D;


	void Start()
	{

		mouseRig2D=GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{

			Vector2 movement=new Vector2(1f,0f);
			mouseRig2D.velocity=movement*speed;
		}

	}
}
