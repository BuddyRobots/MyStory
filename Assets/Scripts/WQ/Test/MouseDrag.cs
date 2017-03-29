using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour 
{

	private Vector3 lastMousePosition = Vector3.zero;
	private bool isMouseDown = false;  
	Animator mouseAnimator;

	bool isOnNet;
	bool closingFloor;
	bool changeAni;

	void Start()
	{
		mouseAnimator=GetComponent<Animator>();
		isOnNet=false;
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
					}
				}
			}

		}  
		if (Input.GetMouseButtonUp(0)) //如果松开了老鼠
		{
			isMouseDown = false;  
			lastMousePosition = Vector3.zero;

			gameObject.GetComponent<Rigidbody2D>().gravityScale=1;





		}  
		if (isMouseDown)  
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
			//如果点住了老鼠，老鼠在网上
			if (isOnNet) 
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
			if (transform.position.y<=-4.4f) 
			{
				closingFloor=true;
				Debug.Log("快到地面了");

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





	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.tag=="Net" && isMouseDown) 
		{
//			Debug.Log("collide the net");
//			mouseAnimator.CrossFade("8_Bite",0);
		}

		isOnNet=true;
	}

	void OnTriggerStay2D(Collider2D other)
	{


		isOnNet=true;

	}



	void OnTriggerExit2D(Collider2D other) 
	{

//		mouseAnimator.CrossFade("8_FallOnAir",0);
		isOnNet=false;
	}





}
