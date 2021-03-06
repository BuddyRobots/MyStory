﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour 
{
	public static MouseDrag _instance;

	Animator mouseAnimator;

    Vector3 lastMousePosition = Vector3.zero;
	float y_MouseLowestLimit;

	bool closingFloor;
	bool changeAni;
	bool mouseClicked;

	[HideInInspector]
	public bool isMouseDown = false;  //鼠标是不是按下
	[HideInInspector]
	public bool mouseDraging;
	[HideInInspector]
	public bool isOnNet;


	void Awake()
	{

		_instance=this;
	}

	void Start()
	{
		mouseAnimator=GetComponent<Animator>();
		isOnNet=false;
		mouseClicked=false;
		y_MouseLowestLimit=-4.4f;
	}

	void OnMouseDown() 
	{
		mouseDraging=true;
	}

	void OnMouseDrag()
	{
		mouseDraging=true;
	}
	void OnMouseUp()
	{
		mouseDraging=false;
	}

	void Update () 
	{  
		if (Input.GetMouseButtonDown(0))  
		{  
			Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			if (col.Length>0) 
			{
				foreach (Collider2D c in col) 
				{
					if (c.tag=="Player") 
					{
						isMouseDown = true; 
						mouseClicked=true;
						mouseDraging=true;
					}
				}
			}
		}  
		if (Input.GetMouseButtonUp(0) && LevelEight._instance.mouseClicked) //如果松开了老鼠
		{
			isMouseDown = false;  
			lastMousePosition = Vector3.zero;
			gameObject.GetComponent<Rigidbody2D>().gravityScale=1;

		}  
		if (LevelEight._instance.mouseClicked)//如果点击了老鼠（销毁了小手）
		{
			if (isMouseDown)  //且鼠标是按下的
			{  

				if (lastMousePosition != Vector3.zero)  
				{  
					Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastMousePosition;  
					this.transform.position += offset;  
				}  
				lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);  

				gameObject.GetComponent<Rigidbody2D>().gravityScale=0;

				closingFloor=false;
				changeAni=false;

				if (isOnNet) //如果点住了老鼠，老鼠在网上
				{
					mouseAnimator.CrossFade("8_Bite",0);

				}
				else//如果点住了老鼠，老鼠不在网上
				{
					mouseAnimator.CrossFade("8_StruggleOnAir",0);
				}
			} 
			else
			{
				if (mouseClicked) 
				{
					if (transform.position.y<=y_MouseLowestLimit) 
					{
						closingFloor=true;
					}
					if (closingFloor) 
					{
						if (!changeAni) 
						{
							mouseAnimator.CrossFade("8_FallClosingFloor",0);

							changeAni=true;
						}
					}
					else
					{
						mouseAnimator.CrossFade("8_FallOnAir",0);

					}
				}
			}	
		}
	} 
}
